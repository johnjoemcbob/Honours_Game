using UnityEngine;
using System.Collections;

public class EnemyHealthScript : ObjectHealthScript
{
    override protected void HandleDeath()
    {
        Destroy( gameObject );

        GetComponent<EnemyUnitBaseScript>().Die_Killed();
    }
}
