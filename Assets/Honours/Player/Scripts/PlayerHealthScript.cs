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

	override protected void HandleTakeDamage( int damage )
	{
		Text_Health.text = string.Format( "{0} / {1}", Health, MaxHealth );
		Image_Health.fillAmount = (float) Health / MaxHealth;
    }

	override protected void HandleDeath()
	{
		GameLogic.CheckForGameEnd( true );
	}

	public void Reset()
	{
		Health = MaxHealth;
		HandleTakeDamage( 0 ); // Update UI
    }
}
