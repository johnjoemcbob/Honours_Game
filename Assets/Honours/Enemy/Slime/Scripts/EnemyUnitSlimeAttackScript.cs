using UnityEngine;
using System.Collections;

// The slime enemy attack script
// 
// Matthew Cormack
// 13/01/16 - 04:41

public class EnemyUnitSlimeAttackScript : EnemyAttackBaseScript
{
	[Header( "Slime Attack" )]
	// Attack start variables
	public float ChargeUpSpeed = 5;

	// Prefab to spit when attack is charged and ready
	public GameObject ProjectilePrefab;

	// Base logic for this enemy
	public EnemyUnitBaseScript UnitMovement;

	// 
	enum AttackStates
	{
		ChargingUp,
		Attacking
	}
	private AttackStates State = AttackStates.ChargingUp;

	// The time from starting attack until damage
	private float ChargeTime = 0;

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
		UpdateUnitChargeUp( direction );
		UpdateUnitAttacking( ( RouteStart.transform.position - transform.position ).normalized );
	}

	private void UpdateUnitChargeUp( Vector3 direction )
	{
		if ( State != AttackStates.ChargingUp ) return;

		// Shake violently
		transform.localPosition += new Vector3( Random.Range( -0.01f, 0.01f ), 0, Random.Range( -0.01f, 0.01f ) );

		// Move to attacking
		ChargeTime += Time.deltaTime * ChargeUpSpeed;
        if ( ChargeTime >= 1 )
		{
			State = AttackStates.Attacking;
		}
	}

	private void UpdateUnitAttacking( Vector3 direction )
	{
		if ( State != AttackStates.Attacking ) return;

		Quaternion lookattarget = Quaternion.LookRotation( direction );

		GameObject projectile = (GameObject) Instantiate( ProjectilePrefab, transform.position, lookattarget );
		projectile.transform.LookAt( transform.position + ( direction * 10 ) );
		projectile.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
		projectile.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<AudioSource>().pitch = Random.Range( 0.9f, 1.1f );

		// Return to moving
		HasControl = false;
		UnitMovement.HasControl = true;
		State = AttackStates.ChargingUp;
		ChargeTime = 0;
    }
}
