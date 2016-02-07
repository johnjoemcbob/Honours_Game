using UnityEngine;
using System.Collections;

public class DelayedActivationScript : MonoBehaviour
{
    public float Delay = 0.5f;
    public GameObject ActivatedObject;

    private float ActivateTime;

    void Start()
    {
        if ( !ActivatedObject )
        {
            print( "DelayedActivationScript: Requires ActivatedObject" );
            return;
        }

        ActivateTime = Time.time + Delay;
        ActivatedObject.SetActive( false );
    }

    void Update()
    {
        if ( !ActivatedObject ) return;

        if ( ActivateTime <= Time.time )
        {
            ActivatedObject.SetActive( true );
        }
    }
}
