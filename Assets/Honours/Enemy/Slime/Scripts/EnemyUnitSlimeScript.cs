using UnityEngine;
using System.Collections;

// The slime enemy logic script
// 
// Matthew Cormack
// 06/02/16

public class EnemyUnitSlimeScript : EnemyUnitBaseScript
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
