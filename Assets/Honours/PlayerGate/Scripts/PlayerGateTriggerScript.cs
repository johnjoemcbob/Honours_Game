using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// The player's gate trigger, which they must protect
// 
// Matthew Cormack
// 13/01/16 - 10:42

public class PlayerGateTriggerScript : MonoBehaviour
{
	public int MaxHealth = 10;
	public int Health = 10;

	public Text Text_Health;
	public GameLogicScript GameLogic;

	void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.layer == LayerMask.NameToLayer( "Enemy" ) )
		{
			TakeHealth( 1 );

			// Kill enemy
			EnemyUnitBaseScript enemyunit = other.GetComponent<EnemyUnitBaseScript>();
            if ( enemyunit )
			{
				enemyunit.Die_DamageGate();
				Destroy( other.gameObject );
            }

			// Check for game lose conditions
			if ( Health <= 0 )
			{
				GameLogic.CheckForGameEnd( true );
            }
        }
	}

	public void TakeHealth( int damage )
	{
		Health -= damage;
		Health = Mathf.Max( 0, Health );

		// Update UI text
		Text_Health.text = string.Format( "{0} / {1}", Health, MaxHealth );
    }
}
