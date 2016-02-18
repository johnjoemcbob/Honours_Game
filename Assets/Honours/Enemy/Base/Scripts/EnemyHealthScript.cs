using UnityEngine;
using System.Collections;

public class EnemyHealthScript : ObjectHealthScript
{
	public GameObject EnemyContainer;

    override protected void HandleDeath()
    {
		if ( EnemyContainer )
		{
			Destroy( EnemyContainer );
			EnemyContainer.GetComponent<EnemyUnitBaseScript>().Die_Killed();
		}
		else
		{
			Destroy( gameObject );
			GetComponent<EnemyUnitBaseScript>().Die_Killed();
		}
    }
}
