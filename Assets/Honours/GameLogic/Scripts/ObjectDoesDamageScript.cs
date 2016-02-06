using UnityEngine;
using System.Collections;

// The base damaging script
// Will apply damage to anything it triggers/collises with
// NOTE: Next damage delay cooldown applies to ALL objects
// Matthew Cormack
// 15/01/16 - 02:48

public class ObjectDoesDamageScript : MonoBehaviour
{
	// The team the damage is allied with (and will not affect) based on layers
	public string TeamTag = "Enemy";
	// The damage to apply per damage time the object is in collision/within the trigger
	public int Damage = 1;
	// The delay before applying more damage while the object is in collision/within the trigger
	public float BetweenDamage = 0.5f;
    // Only deals damage once
    public bool OneShot = false;

	private float NextDamage = 0;

	void OnTriggerEnter( Collider other )
	{
		TryDealDamage( other.gameObject );
	}

	void OnTriggerStay( Collider other )
	{
		TryDealDamage( other.gameObject );
	}

	void OnCollisionEnter( Collision collision )
	{
		TryDealDamage( collision.gameObject );
	}

	void OnCollisionStay( Collision collision )
	{
		TryDealDamage( collision.gameObject );
	}

	private void TryDealDamage( GameObject victim )
	{
		// Check that the victim can take damage
		ObjectHealthScript healthhandle = victim.GetComponent<ObjectHealthScript>();
		if ( healthhandle )
		{
			// Isn't on the same team
			if ( !victim.CompareTag( TeamTag ) )
			{
				// Can damage currently
				if ( NextDamage <= Time.time )
				{
					healthhandle.TakeHealth( Damage );

					// Delay before next damage to any victim
					NextDamage = Time.time + BetweenDamage;

                    // Destroy if it was a one shot
                    if ( OneShot )
                    {
                        Destroy( this );
                    }
				}
			}
		}
	}
}
