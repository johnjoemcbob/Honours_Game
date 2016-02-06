using UnityEngine;
using System.Collections;

// The cube enemy test logic script
// 
// Matthew Cormack
// 13/01/16 - 04:41

public class EnemyUnitCubeScript : EnemyUnitBaseScript
{
	// Cubes have Moving & Rotating states
	private bool Moving = false;

	void Start()
	{
		GetComponent<AudioSource>().volume = Random.Range( 0.01f, 0.02f );
		GetComponent<AudioSource>().pitch = Random.Range( 0.85f, 1.15f );

		UniqueTimeOffset = Random.Range( 0.01f, 1.25f );
	}

	void Update()
	{
		if ( HasControl )
		{
			UpdateUnit();
		}
		UpdateAttack();

		UpdateFall();
        UpdateHat();
	}

	void UpdateUnit()
	{
        if ( !RouteStart ) { Destroy( gameObject ); return; }

		// Get the forward direction for travelling in
		Vector3 direction = RouteStart.transform.position - transform.position;
		{
			direction.y = 0;
		}
		direction.Normalize();

		// Move
		if ( Moving )
		{
			// temp path testing
			GetComponent<Rigidbody>().angularVelocity = Vector3.Lerp(
				GetComponent<Rigidbody>().angularVelocity,
				( transform.right * Speed ),
				Time.deltaTime * LerpSpeed
			);

			// if close then move on to next node
			float distance = Vector3.Distance( transform.position, RouteStart.transform.position );
			if ( distance < 1.5f )
			{
				GameObject nextnode = RouteStart.GetComponent<EnemyPathNodeScript>().NextNode;
				if ( nextnode )
				{
					RouteStart = nextnode;

					// Reset movement of cube, allow for turning
					Moving = false;
				}
			}
		}
		// Rotate
		else
		{
			if ( GetComponent<Rigidbody>().angularVelocity.magnitude < 0.1f )
			{
				GetComponent<Rigidbody>().isKinematic = true;

				Vector3 lookdirection = Vector3.RotateTowards( transform.forward, direction, Time.deltaTime * LerpSpeed / 2.0f, 0.0F );
				transform.rotation = Quaternion.LookRotation( lookdirection );

				float distance = Vector3.Distance( transform.forward, direction );
				if ( distance < 0.1f )
				{
					Moving = true;
					GetComponent<Rigidbody>().isKinematic = false;
				}
			}
		}

		// Ensure it hasn't fallen over
		float distancepos = Vector3.Distance( transform.right, new Vector3( 0, 1, 0 ) );
		float distanceneg = Vector3.Distance( -transform.right, new Vector3( 0, 1, 0 ) );
		if ( ( distancepos < 0.1f ) || ( distanceneg < 0.1f ) )
		{
			transform.rotation = Quaternion.LookRotation( direction );
		}
	}

	private void UpdateAttack()
	{
		if ( AttackCooldown > Time.time ) return;

		foreach ( Collider posvictim in Physics.OverlapSphere( transform.position, 5 ) )
		{
			if ( posvictim.gameObject.CompareTag( "Player" ) )
			{
				HasControl = false;
				Moving = false;
				Attacks[0].GetComponent<EnemyUnitCubeAttackPounceScript>().RouteStart = posvictim.gameObject;
				Attacks[0].HasControl = true;
			}
        }
	}
}
