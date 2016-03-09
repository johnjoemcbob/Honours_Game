using UnityEngine;
using System.Collections;

// The enemy spawn point script
// Will receive spawning information (i.e. enemy type,  number, delay) from the main round logic
// Matthew Cormack
// 13/01/16 - 04:07

// The structure for holding the current prefab to spawn, number of prefabs, and delay before moving on
public struct SpawnFormation
{
	public GameObject Prefab;
	public int NumberOfPrefabs;
	public int Route;
	public float UnitDelay;
	public float NextDelay;
}

public class EnemySpawnerScript : MonoBehaviour
{
	public bool SpawningActive = false;
	public GameObject[] testprefab;
	public SpawnFormation[] SpawnFormations = new SpawnFormation[8];
	public int CurrentFormation = 0;
	// The selection of possible routes for this spawner
	public GameObject[] RouteStart;

	// Reference to the main game logic script
	public GameLogicScript GameLogic;

	private float NextFormationSpawnTime = -1;
	private float NextUnitSpawnTime = -1;
	private int CurrentUnit = 0;
	private bool HasSpawnedFormation = false;

	void Start()
	{
		int formation = 0;
		int unit_slime = 0;
		int unit_floatskull = 1;
		int unit_bomb = 2;

		// 5 Cubes; Straight
		SpawnFormations[formation].Prefab = testprefab[unit_slime];
        SpawnFormations[formation].NumberOfPrefabs = 5;
		SpawnFormations[formation].Route = 0;
		SpawnFormations[formation].UnitDelay = 0.4f;
		SpawnFormations[formation].NextDelay = 4;
        formation++;

		// 1 Sphere; Figure Eight
		SpawnFormations[formation].Prefab = testprefab[unit_floatskull];
		SpawnFormations[formation].NumberOfPrefabs = 1;
		SpawnFormations[formation].Route = 1;
		SpawnFormations[formation].UnitDelay = 0.2f;
		SpawnFormations[formation].NextDelay = 2;
		formation++;

		// 10 Cubes; Straight
		SpawnFormations[formation].Prefab = testprefab[unit_slime];
		SpawnFormations[formation].NumberOfPrefabs = 10;
		SpawnFormations[formation].Route = 0;
		SpawnFormations[formation].UnitDelay = 0.4f;
		SpawnFormations[formation].NextDelay = 2;
		formation++;

		// 10 Cubes; Figure Eight
		SpawnFormations[formation].Prefab = testprefab[unit_slime];
		SpawnFormations[formation].NumberOfPrefabs = 10;
		SpawnFormations[formation].Route = 1;
		SpawnFormations[formation].UnitDelay = 0.4f;
		SpawnFormations[formation].NextDelay = 2;
		formation++;

		// 5 Spheres; Figure Eight
		SpawnFormations[formation].Prefab = testprefab[unit_floatskull];
		SpawnFormations[formation].NumberOfPrefabs = 5;
		SpawnFormations[formation].Route = 1;
		SpawnFormations[formation].UnitDelay = 0.6f;
		SpawnFormations[formation].NextDelay = 2;
		formation++;

		// 1 Bomb; Straight
		SpawnFormations[formation].Prefab = testprefab[unit_bomb];
		SpawnFormations[formation].NumberOfPrefabs = 1;
		SpawnFormations[formation].Route = 0;
		SpawnFormations[formation].UnitDelay = 0.4f;
		SpawnFormations[formation].NextDelay = 2;
		formation++;

		// 15 Cubes; Straight
		SpawnFormations[formation].Prefab = testprefab[unit_slime];
		SpawnFormations[formation].NumberOfPrefabs = 15;
		SpawnFormations[formation].Route = 0;
		SpawnFormations[formation].UnitDelay = 0.4f;
		SpawnFormations[formation].NextDelay = 2;
		formation++;

		// 20 Spheres; Straight
		SpawnFormations[formation].Prefab = testprefab[unit_floatskull];
		SpawnFormations[formation].NumberOfPrefabs = 20;
		SpawnFormations[formation].Route = 0;
		SpawnFormations[formation].UnitDelay = 0.8f;
		SpawnFormations[formation].NextDelay = 2;
		formation++;

		// Ensure the last formation's delay is 0 for round ending on time
		SpawnFormations[formation-1].NextDelay = 0;
	}

	void Update()
	{
		if ( SpawningActive )
		{
			if ( ( !HasSpawnedFormation ) && ( Time.time >= NextUnitSpawnTime ) )
			{
				// Pick a random path
				GameObject routestart = RouteStart[SpawnFormations[CurrentFormation].Route]; // RouteStart[Random.Range( 0, RouteStart.Length - 1 )];

				// Spawn each unit in the formation at the spawn point
				GameObject unit = (GameObject) Instantiate( SpawnFormations[CurrentFormation].Prefab, transform.position, transform.rotation );
				unit.GetComponent<EnemyUnitBaseScript>().RouteStart = routestart;
				unit.transform.SetParent( GameObject.Find( "GameObjectContainer" ).transform );

				// Move onto the next unit after a delay
				CurrentUnit++;
				NextUnitSpawnTime = Time.time + SpawnFormations[CurrentFormation].UnitDelay;

				// Flag the formation as spawned
				if ( CurrentUnit >= SpawnFormations[CurrentFormation].NumberOfPrefabs )
				{
					HasSpawnedFormation = true;
				}
			}

			// Initialize the time until next formation on the first spawn
			if ( NextFormationSpawnTime == -1 )
			{
				NextFormationSpawnTime = Time.time + SpawnFormations[CurrentFormation].NextDelay;
            }

			// Check for delay surpassed
			if ( NextFormationSpawnTime <= Time.time )
			{
				CurrentFormation++;

				// Break out at the end
				if ( SpawnFormations.Length <= CurrentFormation )
				{
					CurrentFormation = 0;
					SpawningActive = false;
					GameLogic.SetFinishedRoundSpawning( true );
                    return;
				}

				// Update the delay until the next formation
				NextFormationSpawnTime = Time.time + SpawnFormations[CurrentFormation].NextDelay;
				CurrentUnit = 0;
				HasSpawnedFormation = false;
            }
		}
	}

	public void Reset()
	{
		CurrentFormation = 0;
		SpawningActive = true;
		NextFormationSpawnTime = -1;
		NextUnitSpawnTime = -1;
		HasSpawnedFormation = false;
	}
}
