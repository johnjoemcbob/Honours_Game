using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script will destroy objects outside of the volume of the box collider related to it
// NOTE: May need extra functionality for more than one volume, allowing for different shapes

public class ExternalCullingVolumeScript : MonoBehaviour
{
	public List<GameObject> IgnoreObjects;

	void OnTriggerExit( Collider other )
	{
		if ( !IgnoreObjects.Contains( other.gameObject ) )
		{
			Destroy( other.gameObject );
		}
	}
}
