using UnityEngine;
using System.Collections;

// The enemy path node script
// Used on each node of each possible path, to store information about the next nodes of the path
// Matthew Cormack
// 13/01/16 - 04:46

public class EnemyPathNodeScript : MonoBehaviour
{
	public GameObject NextNode;
	public GameObject NextNodeParticlePrefab;
	public Color RouteVisualColour = Color.red;

	private GameObject NextNodeParticle;

	void Start()
	{
		if ( !NextNode ) return;

		// Calculate the length of the path between the nodes
		float distance = Vector3.Distance( transform.position, NextNode.transform.position );
		float distancemultiplier = distance / 15 * 3;

		// Initialize the particle prefab instance
		NextNodeParticle = (GameObject) Instantiate( NextNodeParticlePrefab );
		NextNodeParticle.transform.SetParent( transform );
		NextNodeParticle.transform.localPosition = new Vector3( 0, -0.8f, 0 );
		NextNodeParticle.transform.LookAt( NextNode.transform );
		NextNodeParticle.transform.eulerAngles = new Vector3( 0, NextNodeParticle.transform.eulerAngles.y, 0 );

		// Alter the particle system to reflect the path
		ParticleSystem system = NextNodeParticle.GetComponent<ParticleSystem>();
		system.startRotation = NextNodeParticle.transform.eulerAngles.y * Mathf.Deg2Rad;
		system.startLifetime = distancemultiplier;
		system.startColor = RouteVisualColour;
    }
}
