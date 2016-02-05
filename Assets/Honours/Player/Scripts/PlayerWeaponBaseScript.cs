using UnityEngine;
using System.Collections;

// The base player weapon script
// Functionality for creating and moving a prefab
// Matthew Cormack
// 13/01/16 - 06:20

public class PlayerWeaponBaseScript : MonoBehaviour
{
	public GameObject Projectile;
	public float ProjectileSpeed = 30;

	public PlayerHandAnimationScript Hand_Left;
	public PlayerHandAnimationScript Hand_Right;

    private bool LeftHandNext;

    // Update is called once per frame
    void Update()
	{
		if ( Input.GetButtonDown( "Fire1" ) )
		{
            // Create projectile and fire it
            Vector3 randomhand = transform.right;
            {
				if ( LeftHandNext )
                {
                    randomhand *= -1;
                }
				LeftHandNext = !LeftHandNext;
            }
            GameObject projectile = (GameObject) Instantiate( Projectile, transform.position + transform.forward + randomhand - ( transform.up / 2 ), transform.rotation );
			projectile.transform.LookAt( transform.position + ( transform.forward * 10 ) );
			projectile.GetComponentInChildren<Rigidbody>().velocity = projectile.transform.forward * ProjectileSpeed;
			projectile.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
            projectile.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<AudioSource>().pitch = Random.Range( 0.9f, 1.1f );

			// Play animation
			PlayerHandAnimationScript hand = Hand_Left;
			{
				if ( LeftHandNext )
				{
					hand = Hand_Right;
				}
			}
			hand.PopAnimation();
			hand.PopAnimation();
			hand.PopAnimation();
			hand.PushAnimation( 1, 0.1f, 0.05f );
			hand.PushAnimation( 2, 0.1f, 0.05f );
			hand.PushAnimation( 1, 0.1f, 0.05f );

			// Play fire sound
			//GetComponent<AudioSource>().volume = Random.Range( 0.2f, 0.3f );
			//GetComponent<AudioSource>().pitch = Random.Range( 0.2f, 0.2f );
			//GetComponent<AudioSource>().Play();
		}
	}
}
