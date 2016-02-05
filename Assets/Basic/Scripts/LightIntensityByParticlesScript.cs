using UnityEngine;
using System.Collections;

public class LightIntensityByParticlesScript : MonoBehaviour
{
	public float MaxIntensity = 8;
	public int MaxParticles = 100;
	public Light SpecificLight = null;
	public ParticleSystem SpecificParticles = null;

	void Start()
	{
		if ( !SpecificLight )
		{
			SpecificLight = GetComponent<Light>();
		}
		if ( !SpecificParticles )
		{
			SpecificParticles = GetComponent<ParticleSystem>();
		}

		if ( ( !SpecificLight ) || ( !SpecificParticles ) )
		{
			print( "LightIntensityByParticlesScript: Needs both Light and ParticleSystem components" );
			Destroy( gameObject );
		}
	}

	void Update()
	{
		float intensity = 1;
		{
			intensity = MaxIntensity * Mathf.Min( 1, SpecificParticles.particleCount / (float) MaxParticles );
		}
		SpecificLight.intensity = intensity;
	}
}
