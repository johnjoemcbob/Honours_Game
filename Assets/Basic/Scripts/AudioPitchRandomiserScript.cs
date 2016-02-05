using UnityEngine;
using System.Collections;

public class AudioPitchRandomiserScript : MonoBehaviour
{
	public float LowerLimit = 0.8f;
	public float UpperLimit = 1.2f;
	public AudioSource SpecificSource = null;

	void Start()
	{
		AudioSource audio = GetComponent<AudioSource>();
		{
			if ( SpecificSource )
			{
				audio = SpecificSource;
			}
		}
		if ( audio )
		{
			audio.pitch = Random.Range( LowerLimit, UpperLimit );
		}
	}
}
