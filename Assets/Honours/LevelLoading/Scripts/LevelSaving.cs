using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot( "Level" )]
public class LevelSaving
{
	public struct LevelObject
	{
		public string Mesh;
		public Vector3 Position;
		public Vector3 Rotation;
	}

	[XmlArray( "LevelObjects" )]
	[XmlArrayItem( "LevelObject" )]
	public List<LevelObject> LevelObjects = new List<LevelObject>();

	public void Save( string path )
	{
		var serializer = new XmlSerializer( typeof( LevelSaving ) );
		using ( var stream = new FileStream( path, FileMode.Create ) )
		{
			serializer.Serialize( stream, this );
		}
	}

	public static LevelSaving Load( string path )
	{
		var serializer = new XmlSerializer( typeof( LevelSaving ) );
		using ( var stream = new FileStream( path, FileMode.Open ) )
		{
			return serializer.Deserialize( stream ) as LevelSaving;
		}
	}
}
