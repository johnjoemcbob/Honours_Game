using UnityEngine;
using System.Collections;

public class LerpToTransformScript : MonoBehaviour
{
    public Transform Target;
    public float Speed = 5;
    public float LargeDistance = 1;
    public float RotateSpeed = 15;
    public float LargeAngle = 80;

    void Update()
    {
        float speed, distance;

        // Move to target position
        speed = Speed;
        distance = Vector3.Distance( transform.position, Target.position );
        if ( distance > LargeDistance )
        {
            speed *= 2;
        }
        //transform.position = Vector3.MoveTowards( transform.position, Target.position, speed * Time.deltaTime );
        transform.position = Vector3.Lerp( transform.position, Target.position, speed * Time.deltaTime );

        // Move to target rotation
        speed = RotateSpeed;
        distance = Quaternion.Angle( transform.rotation, Target.rotation );
        if ( distance > LargeAngle )
        {
            speed *= 2;
        }
        //transform.rotation = Quaternion.RotateTowards( transform.rotation, Target.rotation, speed * Time.deltaTime );
        transform.rotation = Quaternion.Lerp( transform.rotation, Target.rotation, speed * Time.deltaTime );
    }
}
