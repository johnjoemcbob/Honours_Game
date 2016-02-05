using UnityEngine;
using System.Collections;

public class HideRenderersScript : MonoBehaviour
{
	void Start()
	{
		foreach ( Renderer renderer in GetComponentsInChildren<Renderer>() )
		{
			renderer.enabled = false;
		}
	}
}
