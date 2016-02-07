using UnityEngine;
using System.Collections;

[RequireComponent( typeof( ParticleSystem ) )]
public class ParticleSystemSpeedScript : MonoBehaviour
{
    private void LateUpdate()
    {
        int particlecount;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
        {
            particlecount = GetComponent<ParticleSystem>().GetParticles( particles );
        }
        for ( int particle = 0; particle < particlecount; particle++ )
        {
            particles[particle].velocity = ( transform.position - particles[particle].position ).normalized * Time.deltaTime * 10000;
        }
        GetComponent<ParticleSystem>().SetParticles( particles, particlecount );
    }
}
