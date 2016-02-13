﻿using UnityEngine;
using System.Collections;

// The floating skull enemy logic script
// 
// Matthew Cormack
// 13/01/16 - 04:51

public class EnemyUnitFloatSkullScript : EnemyUnitBaseScript
{
	// Update is called once per frame
	void Update()
	{
		// temp path testing
		Vector3 direction = Vector3.Normalize( RouteStart.transform.position - transform.position );
        GetComponent<Rigidbody>().velocity = Vector3.Lerp( GetComponent<Rigidbody>().velocity, direction * Speed, Time.deltaTime * LerpSpeed );

		// if close then move on to next node
		float distance = Vector3.Distance( transform.position, RouteStart.transform.position );
		if ( distance < 1.5f )
		{
			GameObject nextnode = RouteStart.GetComponent<EnemyPathNodeScript>().NextNode;
			if ( nextnode )
			{
				RouteStart = nextnode;
			}
		}

		UpdateFall();
		UpdateHat();
	}
}
