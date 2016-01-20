using UnityEngine;
using System.Collections;

// The cube enemy test logic script
// 
// Matthew Cormack
// 13/01/16 - 04:41

public class EnemyUnitCubeAttackPounceScript : EnemyUnitBaseScript
{
	public GameObject[] Eyes;
	public GameObject[] Jaws;
	public GameObject Damager;
	public float EyeChargeAffect = 0;
	public float JawChargeAffect = 0;
	public float ChargeUpRate = 10;

	public EnemyUnitBaseScript UnitMovement;

	// Cube pounce attack has Rotating, ChargingUp, & Attacking states
	enum PounceStates
	{
		Rotating,
		ChargingUp,
		Attacking
	}
	private PounceStates State = PounceStates.Rotating;
	// Store the initial transforms of the ChargingUp affected features for reseting
	private Vector3 DefaultEyeScale;
	private Quaternion[] DefaultJawOffset = new Quaternion[2];
	private Vector3 StartPouncePosition;
	private bool Pounced = false;
	private bool PounceLanded = false;

	void Start()
	{
		UniqueTimeOffset = Random.Range( 0.01f, 1.25f );

		// Store default feature transforms for resetting after ChargingUp
		DefaultEyeScale = Eyes[0].transform.localScale;
		DefaultJawOffset[0] = Jaws[0].transform.rotation;
		DefaultJawOffset[1] = Jaws[1].transform.rotation;
	}

	void Update()
	{
		if ( HasControl )
		{
			UpdateUnit();
		}
	}

	void UpdateUnit()
	{
		if ( !RouteStart ) return;

		// Get the forward direction for travelling in
		// NOTE: RouteStart in this instance is the target of the attack
		Vector3 direction = RouteStart.transform.position - transform.position;
		{
			direction.y = 0;
		}
		direction.Normalize();

		// Update the current state
		UpdateUnitRotate( direction );
		UpdateUnitChargeUp( direction );
		UpdateUnitAttacking( direction );

		// Ensure it hasn't fallen over
		float distancepos = Vector3.Distance( transform.right, new Vector3( 0, 1, 0 ) );
		float distanceneg = Vector3.Distance( -transform.right, new Vector3( 0, 1, 0 ) );
		if ( ( distancepos < 0.1f ) || ( distanceneg < 0.1f ) )
		{
			transform.rotation = Quaternion.LookRotation( direction );
		}
	}

	private void UpdateUnitRotate( Vector3 direction )
	{
		if ( State != PounceStates.Rotating ) return;

		// Reset on all but yaw axis
		transform.eulerAngles = new Vector3( 0, transform.eulerAngles.y, 0 );

		// Rotate towards the victim player
		if ( GetComponent<Rigidbody>().angularVelocity.magnitude < 0.001f )
		{
			GetComponent<Rigidbody>().isKinematic = true;

			Vector3 lookdirection = Vector3.RotateTowards( transform.forward, direction, Time.deltaTime * LerpSpeed / 2.0f, 0.0F );
			transform.rotation = Quaternion.LookRotation( lookdirection );

			float distance = Vector3.Distance( transform.forward, direction );
			if ( distance < 0.1f )
			{
				State = PounceStates.ChargingUp;
				StartPouncePosition = transform.localPosition;
            }
		}
	}

	private void UpdateUnitChargeUp( Vector3 direction )
	{
		if ( State != PounceStates.ChargingUp ) return;

		bool close = false;

		// Eyes widen
		for ( int eye = 0; eye < 2; eye++ )
		{
			Eyes[eye].transform.localScale = Vector3.Lerp(
				Eyes[eye].transform.localScale,
				DefaultEyeScale * EyeChargeAffect,
				Time.deltaTime * ChargeUpRate
			);
			if ( Vector3.Distance( Eyes[eye].transform.localScale, DefaultEyeScale * EyeChargeAffect ) < 0.1f )
			{
				close = true;
            }
		}
		// Mouth widen
		int multiplier = 1;
        for ( int jaw = 0; jaw < 2; jaw++ )
		{
			Jaws[jaw].transform.rotation = Quaternion.Lerp(
				Jaws[jaw].transform.rotation,
				DefaultJawOffset[jaw] * Quaternion.Euler( Jaws[jaw].transform.forward * JawChargeAffect * multiplier ),
				Time.deltaTime * ChargeUpRate
			);
			multiplier = 1;
        }
		// Shake violently
		transform.localPosition += new Vector3( Random.Range( -0.01f, 0.01f ), 0, Random.Range( -0.01f, 0.01f ) );

		// Move to attacking
		if ( close )
		{
			State = PounceStates.Attacking;
			// Reset position after shaking
			transform.localPosition = StartPouncePosition;
		}
	}

	private void UpdateUnitAttacking( Vector3 direction )
	{
		if ( State != PounceStates.Attacking ) return;

		if ( !Pounced )
		{
			// Particle trail
			// Force toward player
			Rigidbody body = GetComponent<Rigidbody>();
			body.isKinematic = false;
			body.WakeUp();
			body.AddForce( ( direction + ( Vector3.up / 2 ) ) * 50000 );
			// Enable damage trigger
			Damager.SetActive( true );
			Pounced = true;
        }
		else if ( PounceLanded )
		{
			// Eyes return to normal
			for ( int eye = 0; eye < 2; eye++ )
			{
				Eyes[eye].transform.localScale = Vector3.Lerp(
					Eyes[eye].transform.localScale,
					DefaultEyeScale,
					Time.deltaTime * ChargeUpRate * 10
				);
			}
			// Mouth snap closed on landing
			for ( int jaw = 0; jaw < 2; jaw++ )
			{
				Jaws[jaw].transform.rotation = Quaternion.Lerp(
					Jaws[jaw].transform.rotation,
					DefaultJawOffset[jaw],
					Time.deltaTime * ChargeUpRate * 10
				);
			}
			// Disable damage trigger
			Damager.SetActive( false );

			PounceLanded = false;

			// Return control to movement
			HasControl = false;
			UnitMovement.HasControl = true;
			UnitMovement.AttackCooldown = Time.time + 5;
        }
	}

	void OnCollisionEnter( Collision collision )
	{
		if ( State != PounceStates.Attacking ) return;

		// Strong force applied
		if ( Vector3.Magnitude( collision.relativeVelocity ) > 1 )
		{
			PounceLanded = true;
		}
	}
}
