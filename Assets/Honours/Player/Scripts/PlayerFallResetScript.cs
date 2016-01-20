using UnityEngine;
using System.Collections;

// The player fall off world logic
// Lose the game and allow for reset if the player falls off the world
// Matthew Cormack
// 14/01/16 - 10:30

public class PlayerFallResetScript : MonoBehaviour
{
	// Reference to the main game logic
	public GameLogicScript GameLogic;

	void Update()
	{
		if ( transform.position.y < -15 )
		{
			GameLogic.CheckForGameEnd( true );
        }
	}
}
