using UnityEngine;
using System.Collections;

public class DelayedRemovalScript : MonoBehaviour
{
    public float Delay = 0.5f;

    private float ActivateTime;

    void Start()
    {
        ActivateTime = Time.time + Delay;
    }

    void Update()
    {
        if ( ActivateTime <= Time.time )
        {
            Destroy( gameObject );
        }
    }
}
