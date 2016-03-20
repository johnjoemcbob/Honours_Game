using UnityEngine;
using System.Xml;
using System.Collections;

public class LevelLoadingScript : MonoBehaviour
{
    // Reference to the prefab for individual route nodes
    public GameObject RouteNodePrefab;

    // Internal variables for stepping through the level loading
    //private int CurrentRoute = 0;
    //private int CurrentRouteNode = 0;
    // Store references to each of the created routes and their associated nodes for linking after spawning
    private ArrayList LoadedRoutes = new ArrayList();

    // Use this for initialization
    void Start()
    {
        // Ensure everything is reset
        //CurrentRoute = 0;
        //CurrentRouteNode = 0;
        LoadedRoutes.Clear();

        // Begin loading
        XmlReader xmlreader = XmlReader.Create( System.IO.Path.Combine( Application.streamingAssetsPath, "Levels/" + "test.xml" ) );
        while ( xmlreader.Read() )
        {
            // Child of level, parent of node
            if ( IsElementName( xmlreader, "route" ) )
            {
                // Add this new route to the list
                LoadedRoutes.Add( new ArrayList() );
            }

            // Get current routelist for the rest of the elements
            ArrayList routelist = null;
            if ( LoadedRoutes.Count > 0 )
            {
                routelist = (ArrayList) LoadedRoutes[LoadedRoutes.Count - 1];
            }

            // Child of route, parent of nodeattribues (x,y,z)
            if ( IsElementName( xmlreader, "node" ) )
            {
                // Add any nodes to this route
                routelist.Add( new Vector3( float.Parse( xmlreader.GetAttribute( "x" ) ), float.Parse( xmlreader.GetAttribute( "y" ) ), float.Parse( xmlreader.GetAttribute( "z" ) ) ) );
            }
        }

        // Instantiate the route parent and node game objects
        foreach ( ArrayList list in LoadedRoutes )
        {
            // Create the parent
            GameObject routeparent = new GameObject( "Route CUSTOM LOADED" );
            routeparent.transform.parent = GameObject.Find( "Paths" ).transform;

            // Create the nodes
            EnemyPathNodeScript lastnode = null;
            for ( int node = list.Count - 1; node >= 0; node-- )
            {
                // Instantiate the prefab
                GameObject routenode = (GameObject) Instantiate( RouteNodePrefab );
                routenode.transform.parent = routeparent.transform;
                routenode.transform.position = (Vector3) list[node];

                // Link to the next node
                EnemyPathNodeScript pathnode = routenode.GetComponent<EnemyPathNodeScript>();
                if ( lastnode )
                {
                    pathnode.NextNode = lastnode.gameObject;
                }
                lastnode = pathnode;
            }
        }
    }

    // Check that there is an element, & it is the one sought for
    private bool IsElementName( XmlReader xmlreader, string name )
    {
        return ( xmlreader.NodeType == XmlNodeType.Element ) && ( xmlreader.Name == name );
    }
}
