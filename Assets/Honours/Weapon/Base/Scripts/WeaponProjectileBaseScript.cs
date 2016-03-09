using UnityEngine;
using System.Collections;

// The base player weapon projectile script
// Functionality for colliding
// Matthew Cormack
// 13/01/16 - 06:29

public class WeaponProjectileBaseScript : MonoBehaviour
{
	public string AllyTag = "Player";
    public GameObject DeathEffectPrefab;

	private bool HasHitUnit = false;

	void Update()
	{
		// Cull bullets outside of the world
		if ( ( transform.position.y > 10 ) || ( transform.position.y < -15 ) )
		{
			OnCollide( null );
		}
	}

	void OnCollisionEnter( Collision collision )
	{
		OnCollide( collision );
	}

	virtual protected void OnCollide( Collision collision )
	{
		if ( ( !collision.gameObject ) || collision.gameObject.CompareTag( AllyTag ) ) return;

        // Kill protectile but save effects
        SaveEffects();
		Destroy( transform.parent.gameObject );

		// Spawn projectile death effect
		GameObject effect = (GameObject) Instantiate( DeathEffectPrefab, transform.position, Quaternion.Euler( Vector3.zero ) );
		effect.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );
	}

	virtual protected void OnUnitHit( Collision collision )
	{

	}

    private void SaveEffects()
    {
        if ( transform.childCount == 0 ) return;

        // Remove parent so it's not deleted
        Transform effects = transform.GetChild( 0 );
        effects.SetParent( null );

        // Stop audio from looping
        AudioSource audio = effects.GetComponent<AudioSource>();
        if ( audio )
        {
            audio.loop = false;
        }

        // Stop particles from looping
        ParticleSystem particle = effects.GetComponent<ParticleSystem>();
        if ( particle )
        {
            particle.loop = false;
            particle.Stop();
        }
    }
}
