using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot( "RoundData" )]
public class AnalyticTrackingScript
{
	public const uint MAXOCCURRENCES = 1000;

	public struct AnalyticDataIndividual
	{
		public float Timestamp;
		public string Key;
		public string Value;
	}

	[XmlArray( "Data" )]
	[XmlArrayItem( "AnalyticDataIndividual" )]
	public List<AnalyticDataIndividual> RoundData = new List<AnalyticDataIndividual>();

	// Times each data key has been output to file during this session
	private List<AnalyticDataIndividual> Occurrences = new List<AnalyticDataIndividual>();

	public void Start()
	{

	}

	public void Update()
	{

	}

	public void TrackEvent( float time, string key, string value )
	{
		AnalyticDataIndividual data = new AnalyticDataIndividual(); ;
		{
			data.Timestamp = time;
			data.Key = key;
			data.Value = value;
		}
		TrackEvent( data );
	}

	public void TrackEvent( AnalyticDataIndividual data )
	{
		if ( ( data.Key.Length == 0 ) && ( data.Value.Length == 0 ) ) return;

		int occur = -1;
		bool add = true;
		{
			// Try to find data within occurrences
			{
				for ( int key = 0; key < Occurrences.Count; key++ )
				{
					if ( Occurrences[key].Key == data.Key )
					{
						occur = key;
						break;
					}
				}
			}
			// If not add and continue
			if ( occur == -1 )
			{
				AnalyticDataIndividual datakey = new AnalyticDataIndividual();
				{
					datakey.Key = data.Key;
					data.Timestamp = 0;
				}
				Occurrences.Add( datakey );
				occur = Occurrences.Count - 1;
			}
			// Otherwise add and check for over max
			else if ( Occurrences[occur].Timestamp > MAXOCCURRENCES )
			{
				add = false;
			}
		}
		if ( add )
		{
			AnalyticDataIndividual temp = Occurrences[occur];
			{
				temp.Timestamp++;
			}
			Occurrences.RemoveAt( occur );
			Occurrences.Insert( occur, temp );
            RoundData.Add( data );
		}
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
