using UnityEngine;
using System.Collections;
using System.IO;

public class AnalyticsSceneScript : MonoBehaviour
{
	public AnalyticTrackingScript Tracking = new AnalyticTrackingScript();

	void Start()
	{
		Tracking.Start();
	}

	void Update()
	{
		Tracking.Update();
	}

	void OnApplicationQuit()
	{
        Tracking.Save( Path.Combine( Application.dataPath, "testanalytic.xml" ) );
	}
}
