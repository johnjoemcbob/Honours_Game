using UnityEngine;
using System.Collections;

public class ScaleInScript : MonoBehaviour
{
    public Vector3 StartScale = Vector3.zero;
    public Vector3 TargetScale = Vector3.zero;
    public bool UseDefaultScaleAsTarget = true;
    public float LerpTime = 0.5f;

    private float LerpComplete;

    void Start()
    {
        if ( UseDefaultScaleAsTarget )
        {
            TargetScale = transform.localScale;
        }

        OnEnable();
    }

    void OnEnable()
    {
        LerpComplete = Time.time + LerpTime;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp( StartScale, TargetScale, Mathf.Clamp( 1 - ( ( LerpComplete - Time.time ) / LerpTime ), 0, 1 ) );
    }
}
