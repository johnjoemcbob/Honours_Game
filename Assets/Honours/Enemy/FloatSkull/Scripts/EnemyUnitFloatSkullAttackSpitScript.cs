using UnityEngine;
using System.Collections;

// The float skull enemy test logic script
// 
// Matthew Cormack
// 13/01/16 - 04:41

public class EnemyUnitFloatSkullAttackSpitScript : EnemyUnitBaseScript
{
	// Attack start variables
	public float ChargeUpSpeed = 5;
	public float ProjectileSpeed = 1;

	// Translations of jaws when opened and closed for ANIMATING
	public Transform Jaw_Open;
	public Transform Jaw_Close;

	// Prefab to spit when attack is charged and ready
	public GameObject ProjectilePrefab;

	// Base logic for this enemy
	public EnemyUnitBaseScript UnitMovement;

	// Cube pounce attack has Rotating, ChargingUp, & Attacking states
	enum AttackStates
	{
		Rotating,
		ChargingUp,
		Attacking,
		ChargingDown
	}
	private AttackStates State = AttackStates.Rotating;

	void Start()
	{
		UniqueTimeOffset = Random.Range( 0.01f, 1.25f );
	}

	void Update()
	{
		if ( HasControl )
		{
			// Override to ensure the parent has no control
			UnitMovement.HasControl = false;

			RouteStart = GameObject.Find( "Player" );
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
		UpdateUnitAttacking( ( RouteStart.transform.position - transform.position ).normalized );
		UpdateUnitChargeDown( direction );
	}

	private void UpdateUnitRotate( Vector3 direction )
	{
		if ( State != AttackStates.Rotating ) return;

		// Reset on all but yaw axis
		transform.eulerAngles = new Vector3( 0, transform.eulerAngles.y, 0 );

		// Rotate towards the victim player
		Vector3 lookdirection = Vector3.RotateTowards( transform.forward, direction, Time.deltaTime * LerpSpeed / 2.0f, 0.0F );
		transform.rotation = Quaternion.LookRotation( lookdirection );

		float distance = Vector3.Distance( transform.forward, direction );
		if ( distance < 0.1f )
		{
			State = AttackStates.ChargingUp;
        }
	}

	private void UpdateUnitChargeUp( Vector3 direction )
	{
		if ( State != AttackStates.ChargingUp ) return;

		// Mouth widen
		{
			Hat.transform.rotation = Quaternion.Lerp(
				Hat.transform.rotation,
				Jaw_Open.rotation,
				Time.deltaTime * ChargeUpSpeed
			);
		}
		// Shake violently
		transform.localPosition += new Vector3( Random.Range( -0.01f, 0.01f ), 0, Random.Range( -0.01f, 0.01f ) );

		// Move to attacking
		if ( Quaternion.Angle( Hat.transform.rotation, Jaw_Open.rotation ) < 1 )
		//if ( close )
		{
			State = AttackStates.Attacking;
		}
	}

	private void UpdateUnitAttacking( Vector3 direction )
	{
		if ( State != AttackStates.Attacking ) return;

		GameObject projectile = (GameObject) Instantiate( ProjectilePrefab, transform.position, transform.rotation );
		projectile.transform.LookAt( transform.position + ( direction * 10 ) );
		projectile.GetComponentInChildren<Rigidbody>().velocity = direction * ProjectileSpeed;
		projectile.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
		projectile.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<AudioSource>().pitch = Random.Range( 0.9f, 1.1f );

		State = AttackStates.ChargingDown;
	}

	private void UpdateUnitChargeDown( Vector3 direction )
	{
		if ( State != AttackStates.ChargingDown ) return;

		// Mouth widen
		{
			Hat.transform.rotation = Quaternion.Lerp(
				Hat.transform.rotation,
				Jaw_Close.rotation,
				Time.deltaTime * ChargeUpSpeed
			);
		}
		// Shake violently
		transform.localPosition += new Vector3( Random.Range( -0.01f, 0.01f ), 0, Random.Range( -0.01f, 0.01f ) );

		// Return control to movement
		if ( Quaternion.Angle( Hat.transform.rotation, Jaw_Close.rotation ) < 1 )
		//if ( close )
		{
			HasControl = false;
			UnitMovement.HasControl = true;
			State = AttackStates.Rotating;
		}
	}
}
