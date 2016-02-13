using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot( "RoundData" )]
public class AnalyticTrackingScript
{
	public struct AnalyticDataIndividual
	{
		public float Timestamp;
		public string Key;
		public string Value;
	}

	[XmlArray( "Data" )]
	[XmlArrayItem( "AnalyticDataIndividual" )]
	public List<AnalyticDataIndividual> RoundData = new List<AnalyticDataIndividual>();

	public void Start()
	{

	}

	public void Update()
	{

	}

	public void TrackEvent( float time, string key, string value )
	{
		AnalyticDataIndividual data;
		{
			data.Timestamp = time;
			data.Key = key;
			data.Value = value;
		}
		RoundData.Add( data );
	}

	public void TrackEvent( AnalyticDataIndividual data )
	{
		if ( ( data.Key.Length == 0 ) && ( data.Value.Length == 0 ) ) return;

		RoundData.Add( data );
	}

	public void Save( string path )
	{
		var serializer = new XmlSerializer( typeof( AnalyticTrackingScript ) );
		using ( var stream = new FileStream( path, FileMode.Create ) )
		{
			serializer.Serialize( stream, this );
		}
	}

	public static AnalyticTrackingScript Load( string path )
	{
		var serializer = new XmlSerializer( typeof( AnalyticTrackingScript ) );
		using ( var stream = new FileStream( path, FileMode.Open ) )
		{
			return serializer.Deserialize( stream ) as AnalyticTrackingScript;
		}
	}
}
