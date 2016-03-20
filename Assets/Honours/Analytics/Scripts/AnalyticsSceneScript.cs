using UnityEngine;
using System.Collections;
using System.IO;

public class AnalyticsSceneScript : MonoBehaviour
{
	public AnalyticTrackingScript Tracking = new AnalyticTrackingScript();
	public AnalyticOverallTrackingScript OverallTracking = new AnalyticOverallTrackingScript();

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
        Tracking.Save( Path.Combine( Application.dataPath, "analytic_heatmap.xml" ) );
		OverallTracking.Save( Path.Combine( Application.dataPath, "analytic_overall.xml" ) );
	}
}
