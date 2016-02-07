using UnityEngine;
using System.Collections;

public class StopAudioAfterParticlesScript : MonoBehaviour
{
    void Update()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        if ( particle && ( particle.particleCount == 0 ) )
        {
            AudioSource audio = GetComponent<AudioSource>();
            if ( audio )
            {
                audio.loop = false;
                audio.Stop();
            }
        }
    }
}
