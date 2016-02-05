using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// The player's health handling
// Also calls the game lose if the player dies
// Matthew Cormack
// 15/01/16 - 02:33

public class PlayerHealthScript : ObjectHealthScript
{
	public GameLogicScript GameLogic;
	public Text Text_Health;
	public Image Image_Health;

	void Start()
	{
		if ( !Text_Health )
		{
			print( "PlayerHealthScript: Requires Text UI object for health" );
		}
		if ( !GameLogic )
		{
			print( "PlayerHealthScript: Requires reference to GameLogicScript" );
		}
	}

	override protected void HandleTakeDamage( int damage )
	{
		if ( !Text_Health ) return;

		Text_Health.text = string.Format( "{0} / {1}", Health, MaxHealth );
		Image_Health.fillAmount = (float) Health / MaxHealth;
    }

	override protected void HandleDeath()
	{
		if ( !GameLogic ) return;

		GameLogic.CheckForGameEnd( true );
	}

	public void Reset()
	{
		Health = MaxHealth;
		HandleTakeDamage( 0 ); // Update UI
    }
}
