using UnityEngine;
using System.Collections;

public class ParticleInheritRotationScript : MonoBehaviour
{
	public Transform InheritFrom;

	void Update()
	{
		GetComponent<ParticleSystem>().startRotation3D += InheritFrom.eulerAngles;
	}
}
