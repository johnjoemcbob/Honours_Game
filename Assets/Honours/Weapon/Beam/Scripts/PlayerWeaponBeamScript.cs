using UnityEngine;
using System.Collections;

// A beam player weapon script
// Functionality for creating and moving a prefab
// Matthew Cormack
// 13/01/16 - 06:20

public class PlayerWeaponBeamScript : PlayerWeaponBaseScript
{
	private GameObject ProjectileInstance;
	private GameObject ImpactEffectInstance;
	private GameObject Hand;

	// Update is called once per frame
	new protected void Update()
	{
		base.Update();

		if ( ProjectileInstance )
		{
			// Raycast forward
			RaycastHit hitinfo;
			{
				Physics.Raycast( Hand.transform.position, transform.forward, out hitinfo );
				// Ensure the beam is projected by making up information if the ray does not hit
				if ( !hitinfo.collider )
				{
					hitinfo.distance = 20;
					hitinfo.point = Hand.transform.position + ( transform.forward * hitinfo.distance );
				}
			}
			ProjectileInstance.transform.LookAt( hitinfo.point );
			ProjectileInstance.transform.SetParent( Hand.transform );

			// Scale and position the beam
			Transform beam = ProjectileInstance.transform.GetChild( 0 );
			Vector3 target = new Vector3( 1, 1, hitinfo.distance / 2 );
			if ( target.magnitude < beam.localScale.magnitude )
			{
				// Target is closer, set immediately
				beam.localScale = target;
			}
			else
			{
				// Target is further, lerp
				beam.localScale = Vector3.Lerp( beam.localScale, target, Time.deltaTime * 10 );
			}
			beam.position = Hand.transform.position + ( ( hitinfo.point - Hand.transform.position ) / 2 );

			// Position the impact effect
			if ( ImpactEffectInstance )
			{
				ImpactEffectInstance.transform.position = hitinfo.point;
			}
			else
			{
				// Create impact effect after the build up animation is finished
				if ( Hand.GetComponent<PlayerHandAnimationScript>().GetAnimation() == 4 )
				{
					ImpactEffectInstance = (GameObject) Instantiate( ImpactEffect, new Vector3( 0, -100, 0 ), transform.rotation );
					ImpactEffectInstance.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
				}
			}

			// Update looping animation
			PlayerHandAnimationScript hand = Hand.GetComponent<PlayerHandAnimationScript>();
            if ( hand.GetAnimationCount() == 2 )
			{
				hand.PushAnimation( 5, Random.Range( 0.9f, 1.2f ), 0.0f );
				hand.PushAnimation( 4, Random.Range( 0.9f, 1.2f ), 0.0f );
			}
		}
	}

	override protected void FireFromHand( Vector3 offset, PlayerHandAnimationScript hand )
	{
		// Create projectile and fire it
		ProjectileInstance = (GameObject) Instantiate( Projectile, hand.transform.position, transform.rotation );
		ProjectileInstance.transform.LookAt( transform.position + ( transform.forward * 10 ) );
		//projectile.GetComponentInChildren<Rigidbody>().velocity = projectile.transform.forward * ProjectileSpeed;
		ProjectileInstance.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
		//projectile.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<AudioSource>().pitch = Random.Range( 0.9f, 1.1f );
		Hand = hand.gameObject;

		// Play animation
		hand.PopAnimation();
		hand.PopAnimation();
		hand.PopAnimation();
		hand.PushAnimation( 3, 0.1f, 0.05f );
		hand.PushAnimation( 4, 0.3f, 0.2f );
		hand.PushAnimation( 3, 0.1f, 2 );
	}

	override protected void StopFireFromHand( Vector3 offset, PlayerHandAnimationScript hand )
	{
		hand.PopAnimation();
		hand.PopAnimation();
		hand.PopAnimation();
		hand.PushAnimation( 3, 0.1f, 0.05f );

		Destroy( ProjectileInstance );
		if ( ImpactEffectInstance )
		{
			ImpactEffectInstance.GetComponent<ParticleSystem>().loop = false;
		}
	}

	void OnApplicationFocus( bool focus )
	{
		if ( !focus && ProjectileInstance )
		{
			Destroy( ProjectileInstance );
			ProjectileInstance = null;
		}
	}
}
