using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// The main over-arching game logic
// 
// Matthew Cormack
// 13/01/16 - 12:12

public class GameLogicScript : MonoBehaviour
{
	public GameObject MenuUI;
	public GameObject GameUI;
	public Text Text_Lose;
	public Text Text_Win;

	public EnemySpawnerScript[] Spawners;
	public PlayerGateTriggerScript PlayerGate;
	public GameObject GameWinPrefab;
	public GameObject Player;

	private bool FinishedRoundSpawning = false;
	private bool RoundOutcomeLose = false;

	void Update()
	{
		// Once all enemies have spawned, check for them dying
		if ( FinishedRoundSpawning && ( !RoundOutcomeLose ) )
		{
			if ( !GameObject.FindWithTag( "Enemy" ) )
			{
				CheckForGameEnd( false );
            }
		}
	}

	public void CheckForGameEnd( bool lose )
	{
		// If the UI is already active then this has already been run, don't run multiple times
		if ( MenuUI.activeSelf ) return;

		// Turn on menu ui for both outcomes
		MenuUI.SetActive( true );
		GameUI.SetActive( false );
		GameObject.Find( "Player" ).GetComponent<PlayerLockCursorScript>().SetCursor( true );
		// Turn on the menu with special 'you lost/won' text
		Text_Lose.enabled = lose;
		Text_Win.enabled = !lose;

		// Store for stopping a quick win if the last enemy alive takes the last health point of the gate
		RoundOutcomeLose = lose;

		if ( !lose )
		{
			GameObject effect = (GameObject) Instantiate( GameWinPrefab );
			effect.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
		}
    }

	public void Button_PlayAgain()
	{
		if ( !Cursor.visible ) return;

		// Reenable spawners
		if ( Spawners.Length > 0 )
		{
			for ( int spawner = 0; spawner < Spawners.Length; spawner++ )
			{
				Spawners[spawner].Reset();
            }
        }

		// Cleanup old round specific gameobjects
		foreach( Transform oldtransform in GameObject.Find( "GameObjectContainer" ).GetComponentsInChildren<Transform>() )
		{
			if ( oldtransform.gameObject.name != "GameObjectContainer" )
			{
				Destroy( oldtransform.gameObject );
			}
        }

		// Hide UI now
		MenuUI.SetActive( false );
		GameUI.SetActive( true );

		// Reset round end flag
		FinishedRoundSpawning = false;
		RoundOutcomeLose = false;

		// Reset the health of the gate
		PlayerGate.Health = PlayerGate.MaxHealth;
		PlayerGate.TakeHealth( 0 ); // (to update the ui text)

		// Reset the player's position
		Player.transform.position = Vector3.zero;

		// Reset the player's health
		Player.GetComponent<PlayerHealthScript>().Reset();

		GameObject.Find( "Player" ).GetComponent<PlayerLockCursorScript>().ToggleCursor();
	}

	public void Button_Exit()
	{
		if ( !Cursor.visible ) return;

		Application.Quit();

		GameObject.Find( "Player" ).GetComponent<PlayerLockCursorScript>().ToggleCursor();
	}

	public void SetFinishedRoundSpawning( bool finish )
	{
		FinishedRoundSpawning = finish;
    }
}
