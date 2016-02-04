using UnityEngine;
using System.Collections;

public class FloatyObjectScript : MonoBehaviour
{
    public float RotateSpeed = 1;
    public float HoverSpeed = 1;
    public float HoverAmount = 0.2f;

    private Vector3 DefaultPosition;

    void Start()
    {
        DefaultPosition = transform.localPosition;
    }

    void Update()
    {
        transform.localEulerAngles += new Vector3( 0, RotateSpeed * Time.deltaTime, 0 );
        transform.localPosition = DefaultPosition + new Vector3( 0, Mathf.Sin( HoverSpeed * Time.time ) * HoverAmount, 0 );
    }
}
