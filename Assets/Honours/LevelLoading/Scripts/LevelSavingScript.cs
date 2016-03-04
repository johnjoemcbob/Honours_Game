using UnityEngine;
using System.Text;
using System.Collections;
using System.IO;

public class LevelSavingScript : MonoBehaviour
{
	private LevelSaving LevelSave = new LevelSaving();

	void Start()
	{
		foreach( MeshFilter mesh in GetComponentsInChildren<MeshFilter>() )
		{
			LevelSaving.LevelObject item;
			{
				item.Mesh = GetName( mesh );
				item.Position = mesh.transform.position;
				item.Rotation = mesh.transform.eulerAngles;
			}
			LevelSave.LevelObjects.Add( item );
			// Remove bracketed numbers from end
		}
		LevelSave.Save( Path.Combine( Application.dataPath, "testlevel.xml" ) );
	}

	private string GetName( MeshFilter mesh )
	{
		string name = mesh.transform.parent.gameObject.name;
		{
			for ( int i = 1; i < 50; i++ )
			{
				string old = " (" + i.ToString() + ")";
                name = name.Replace( old, "" );
			}
		}
		return name;
    }
}
