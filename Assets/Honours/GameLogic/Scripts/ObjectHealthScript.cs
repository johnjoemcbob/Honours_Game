using UnityEngine;
using System.Collections;

// The base script for objects having health
// 
// Matthew Cormack
// 15/01/16 - 02:38

public class ObjectHealthScript : MonoBehaviour
{
	// The base health value
	public int MaxHealth = 100;

	protected int Health;

	void Start()
	{
		Health = MaxHealth;
	}

	// Takes a number of health away from the player, returns true if they died
	public bool TakeHealth( int health )
	{
		Health -= health;
		Health = Mathf.Max( 0, Health );

		HandleTakeDamage( health );

		bool dead = CheckDeath();
		if ( dead )
		{
			HandleDeath();
		}
		return dead;
	}

	protected bool CheckDeath()
	{
		return ( Health == 0 );
	}

	virtual protected void HandleTakeDamage( int damage )
	{

	}

	virtual protected void HandleDeath()
	{

	}
}
