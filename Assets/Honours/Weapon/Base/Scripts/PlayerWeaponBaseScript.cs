using UnityEngine;
using System.Collections;

// The base player weapon script
// Functionality for creating and moving a prefab
// Matthew Cormack
// 13/01/16 - 06:20

public class PlayerWeaponBaseScript : MonoBehaviour
{
	public GameObject Projectile;
    public GameObject ImpactEffect; // Optional
    public float ProjectileSpeed = 30;

	public PlayerHandAnimationScript Hand_Left;
	public PlayerHandAnimationScript Hand_Right;

    // Update is called once per frame
    protected void Update()
    {
        if ( Input.GetButtonDown( "Fire1" ) && Hand_Left )
        {
            // Offset to left hand
            FireFromHand( -transform.right, Hand_Left );
        }
        if ( Input.GetButtonDown( "Fire2" ) && Hand_Right )
        {
            // Offset to right hand
            FireFromHand( transform.right, Hand_Right );
        }
        if ( Input.GetButtonUp( "Fire1" ) && Hand_Left )
        {
            // Offset to left hand
            StopFireFromHand( -transform.right, Hand_Left );
        }
        if ( Input.GetButtonUp( "Fire2" ) && Hand_Right )
        {
            // Offset to right hand
            StopFireFromHand( transform.right, Hand_Right );
        }
    }

    virtual protected void FireFromHand( Vector3 offset, PlayerHandAnimationScript hand )
    {
        // Create projectile and fire it
        GameObject projectile = (GameObject) Instantiate( Projectile, hand.transform.position, transform.rotation );
        projectile.transform.LookAt( transform.position + ( transform.forward * 10 ) );
        projectile.GetComponentInChildren<Rigidbody>().velocity = projectile.transform.forward * ProjectileSpeed;
        projectile.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
        projectile.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<AudioSource>().pitch = Random.Range( 0.9f, 1.1f );

        // Play animation
        hand.PopAnimation();
        hand.PopAnimation();
        hand.PopAnimation();
        hand.PushAnimation( 3, 0.1f, 0.05f );
        hand.PushAnimation( 4, 0.3f, 0.05f );
        hand.PushAnimation( 3, 0.1f, 0.05f );
    }

    virtual protected void StopFireFromHand( Vector3 offset, PlayerHandAnimationScript hand )
    {
        
    }
}
