using UnityEngine;
using System.Collections;

public class CleanupAfterEffectsScript : MonoBehaviour
{
	public float WaitTime = 0.5f;

	private float ElapsedTime = 0;

    void Update()
    {
		ElapsedTime += Time.deltaTime;
		if ( ElapsedTime < WaitTime ) return;

        AudioSource audio = GetComponent<AudioSource>();
        ParticleSystem particle = GetComponent<ParticleSystem>();
        // Still playing
        if ( ( audio && audio.isPlaying ) || ( particle && ( particle.particleCount > 0 ) ) )
        {
            // Don't delete
        }
        // All effects have stopped
        else
        {
            // Cleanup remaining objects
            Destroy( gameObject );
        }
    }
}
