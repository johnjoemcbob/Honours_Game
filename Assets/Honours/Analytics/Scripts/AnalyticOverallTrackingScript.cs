using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot( "RoundData" )]
public class AnalyticOverallTrackingScript
{
	public struct AnalyticOverallDataIndividual
	{
		public string Key;
		public float Timestamp;
	}

	[XmlArray( "Data" )]
	[XmlArrayItem( "AnalyticDataIndividual" )]
	public List<AnalyticOverallDataIndividual> RoundData = new List<AnalyticOverallDataIndividual>();

	public void Start()
	{

	}

	public void Update()
	{

	}

	public void TrackEvent( string key )
	{
		AnalyticOverallDataIndividual data = new AnalyticOverallDataIndividual();
		{
			data.Key = key;
			data.Timestamp = 1;
		}
		TrackEvent( data );
	}

	public void TrackEvent( AnalyticOverallDataIndividual data )
	{
		if ( data.Key.Length == 0 ) return;

		// If exists simply add one to the value
		int occur = -1;
		{
			// Try to find data within occurrences
			{
				for ( int key = 0; key < RoundData.Count; key++ )
				{
					if ( RoundData[key].Key == data.Key )
					{
						occur = key;
						break;
					}
				}
			}
		}
		if ( occur != -1 )
		{
			AnalyticOverallDataIndividual temp = RoundData[occur];
			{
				temp.Timestamp++;
			}
			RoundData.RemoveAt( occur );
			RoundData.Insert( occur, temp );
		}
		else
		{
			RoundData.Add( data );
		}
	}

	public void Save( string path )
	{
		// Load the old information and append
		AnalyticOverallTrackingScript old = Load( path );
		{
			foreach ( AnalyticOverallDataIndividual data in RoundData )
			{
				old.RoundData.Add( data );
			}
		}

		// Save
		var serializer = new XmlSerializer( typeof( AnalyticOverallTrackingScript ) );
		using ( var stream = new FileStream( path, FileMode.Create ) )
		{
			serializer.Serialize( stream, old );
		}
	}

	public static AnalyticOverallTrackingScript Load( string path )
	{
		if ( File.Exists( path ) )
		{
			var serializer = new XmlSerializer( typeof( AnalyticOverallTrackingScript ) );
			using ( var stream = new FileStream( path, FileMode.Open ) )
			{
				return serializer.Deserialize( stream ) as AnalyticOverallTrackingScript;
			}
		}
		else
		{
			return ( new AnalyticOverallTrackingScript() );
		}
	}
}
