using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

// Used in build copies to lock and unlock the cursor
// 
// Matthew Cormack
// 13/01/16 - 09:12

public class PlayerLockCursorScript : MonoBehaviour
{
	public GameObject UI = null;

	private Vector2 MouseLookSensitivity = Vector2.zero;

	void Start()
	{
		//OnApplicationFocus( true );
	}

	void OnApplicationFocus( bool focus )
	{
		if ( Application.isEditor ) return;

		Cursor.lockState = CursorLockMode.Locked;
        if ( !focus )
		{
			Cursor.lockState = CursorLockMode.None;
		}
		Cursor.visible = !focus;

		LockCamera( Cursor.visible );
	}

	void Update()
	{
		if ( Application.isEditor ) return;

		if ( Input.GetKeyDown( KeyCode.Escape ) )
		{
			ToggleCursor();
		}
	}

	public void SetCursor( bool cursor )
	{
		if ( cursor )
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Enable/disable UI
		if ( UI )
		{
			UI.SetActive( Cursor.visible );
		}

		LockCamera( Cursor.visible );
	}

	public void ToggleCursor()
	{
		SetCursor( Cursor.lockState == CursorLockMode.Locked );
	}

	private void LockCamera( bool locktoggle )
	{
		MouseLook look = GetComponent<FirstPersonController>().m_MouseLook;

		if ( locktoggle == false )
		{
			if ( ( MouseLookSensitivity.x != 0 ) || ( MouseLookSensitivity.y != 0 ) )
			{
				look.XSensitivity = MouseLookSensitivity.x;
				look.YSensitivity = MouseLookSensitivity.y;
			}
		}
		else
		{
			// Store sensitivity for reseting
			MouseLookSensitivity.x = look.XSensitivity;
			MouseLookSensitivity.y = look.YSensitivity;

			look.XSensitivity = 0;
			look.YSensitivity = 0;
		}
	}
}
