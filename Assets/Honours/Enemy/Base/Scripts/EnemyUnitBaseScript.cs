using UnityEngine;
using System.Collections;

// The base enemy logic script
// 
// Matthew Cormack
// 13/01/16 - 04:34

public class EnemyUnitBaseScript : MonoBehaviour
{
	// Units switch between moving and attacking scripts
	public bool HasControl = true;
	// The current node of the route this unit will take, begins being set as the start node
	public GameObject RouteStart;
	public float Speed = 1;
	public float LerpSpeed = 1;
	// The possible attacks this enemy can perform
	public EnemyUnitBaseScript[] Attacks;
	public float AttackCooldown = 0;
	// The gameobject child of this unit representing its hat
	public GameObject Hat;
	// The prefab to spawn when this enemy damages the player's gate
	public GameObject DamageDiePrefab;
	// The prefab to spawn when this enemy is killed by the player
	public GameObject KilledDiePrefab;

	protected float UniqueTimeOffset = 0;

	protected void UpdateHat()
	{
		if ( !Hat ) return;

		Vector3 lookat = transform.right;
		{
			lookat.y = 0;
		}

		// Move hat to always be above the body
		Hat.transform.position = transform.position;
		Hat.transform.LookAt( transform.position + lookat );
		Hat.transform.eulerAngles += new Vector3( 0, 90, 0 );

		// Lerp the hat backward and forth with the unit's movement
		//Hat.transform.eulerAngles += Mathf.Sin( ( Time.time + UniqueTimeOffset ) * 50 ) * 5 * transform.right;
	}

	protected void UpdateFall()
	{
		if ( transform.position.y < -15 )
		{
			Die_Killed();
		}
	}

	public void Die_DamageGate()
	{
		GameObject effect = (GameObject) Instantiate( DamageDiePrefab, transform.position, Quaternion.Euler( Vector3.zero ) );
		effect.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
		Destroy( gameObject );
	}

	public void Die_Killed()
	{
		GameObject effect = (GameObject) Instantiate( KilledDiePrefab, transform.position, Quaternion.Euler( Vector3.zero ) );
		effect.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
		Destroy( gameObject );
	}
}
