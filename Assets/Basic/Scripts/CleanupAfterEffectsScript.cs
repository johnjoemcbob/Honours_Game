using UnityEngine;
using System.Collections;

public class CleanupAfterEffectsScript : MonoBehaviour
{
    void Update()
    {
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
