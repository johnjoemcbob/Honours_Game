using UnityEngine;
using System.Collections;

// The slime enemy logic script
// 
// Matthew Cormack
// 06/02/16

public class EnemyUnitSlimeScript : EnemyUnitBaseScript
{
	[Header( "Slime" )]
	public float MaxJumpDistance = 5;
    public Vector3 AnimateJumpStartTarget = Vector3.zero;
    public Vector3 AnimateJumpEndTarget = Vector3.zero;
	public AudioClip Sound_JumpStart;
	public AudioClip Sound_JumpEnd;

    private Transform Projectile;
    private Vector3 TargetPosition = Vector3.zero;
    private bool AnimateJumpStart = true;
    private bool AnimateJumpEnd = false;
    private bool HasAnimateJumpEnd = false;
    private Vector3 AnimateJumpStartScale = Vector3.zero;
    private Vector3 AnimateJumpEndScale = Vector3.zero;
    private Vector3 DefaultScale;
    private bool Launched = false;

    void Start()
	{
		GetComponent<AudioSource>().volume = Random.Range( 0.2f, 0.4f );
		GetComponent<AudioSource>().pitch = Random.Range( 0.45f, 0.6f );

		UniqueTimeOffset = Random.Range( 0.01f, 1.25f );

        Projectile = transform;

        DefaultScale = transform.GetChild( 0 ).localScale;
    }

	void Update()
	{
		if ( HasControl )
		{
			UpdateUnit();
		}
		UpdateAttack();

		UpdateFall();
        UpdateHat();
		UpdateAnalytic();
	}

	void UpdateUnit()
	{
        if ( !RouteStart ) { Destroy( gameObject ); return; }

        // Wobble & inflate/compress
        Animate();

		// Get the forward direction for travelling in
		Vector3 direction = RouteStart.transform.position - transform.position;
		{
			direction.y = 0;
		}
		direction.Normalize();

        // Jump towards the next node
        if ( ( !AnimateJumpEnd ) && ( TargetPosition == Vector3.zero ) && Physics.Raycast( transform.position, -transform.up, 0.4f ) )
        {
            if ( !HasAnimateJumpEnd )
            {
                AnimateJumpEnd = true;
                HasAnimateJumpEnd = true;
            }
            else
            {
                float distanceleft = Vector3.Distance( RouteStart.transform.position, transform.position );
                if ( distanceleft > MaxJumpDistance )
                {
                    distanceleft = MaxJumpDistance;
                }
                TargetPosition = transform.position + ( direction * distanceleft );
                Launched = false;
                AnimateJumpStart = false;
            }
        }
        if ( AnimateJumpStart && ( !Launched ) )
        {
            float distanceleft = Vector3.Distance( RouteStart.transform.position, transform.position );
            if ( distanceleft > MaxJumpDistance )
            {
                distanceleft = MaxJumpDistance;
            }
            TargetPosition = transform.position + ( direction * distanceleft );
            Launched = true;
            HasAnimateJumpEnd = false;
            StartCoroutine( SimulateProjectile() );
        }

        // if close then move on to next node
        Vector3 temppos = transform.position;
        {
            temppos.y = RouteStart.transform.position.y;
        }
        float distance = Vector3.Distance( temppos, RouteStart.transform.position );
		if ( distance < 1.5f )
		{
			GameObject nextnode = RouteStart.GetComponent<EnemyPathNodeScript>().NextNode;
			if ( nextnode )
			{
				RouteStart = nextnode;
			}
		}
	}

    private void Animate()
    {
        Vector3 scale = DefaultScale;
        {
            float deadzone = 0;// 0.2f;
            // Jump Start
            if ( !AnimateJumpStart ) // Play animation
            {
                //AnimateJumpStartScale = AnimateJumpStartScale + ( ( AnimateJumpStartTarget - AnimateJumpStartScale ) * Time.deltaTime * LerpSpeed );
                AnimateJumpStartScale = Vector3.MoveTowards( AnimateJumpStartScale, AnimateJumpStartTarget, Time.deltaTime * LerpSpeed );

                float distance = Vector3.Distance( AnimateJumpStartScale, AnimateJumpStartTarget );
                if ( distance <= deadzone )
                {
                    AnimateJumpStartScale = AnimateJumpStartTarget;
                    AnimateJumpStart = true;
                }
            }
            else if ( AnimateJumpStartScale != Vector3.zero ) // Undo animation
            {
                //AnimateJumpStartScale = AnimateJumpStartScale + ( ( Vector3.zero - AnimateJumpStartScale ) * Time.deltaTime * LerpSpeed );
                AnimateJumpStartScale = Vector3.MoveTowards( AnimateJumpStartScale, Vector3.zero, Time.deltaTime * LerpSpeed );

                float distance = Vector3.Distance( AnimateJumpStartScale, Vector3.zero );
                if ( distance <= deadzone )
                {
                    AnimateJumpStartScale = Vector3.zero;
                }
            }
            scale += AnimateJumpStartScale;

            // Jump End
            if ( AnimateJumpEnd ) // Play animation
            {
                //AnimateJumpEndScale = AnimateJumpEndScale + ( ( AnimateJumpEndTarget - AnimateJumpEndScale ) * Time.deltaTime * LerpSpeed );
                AnimateJumpEndScale = Vector3.MoveTowards( AnimateJumpEndScale, AnimateJumpEndTarget, Time.deltaTime * LerpSpeed );

                float distance = Vector3.Distance( AnimateJumpEndScale, AnimateJumpEndTarget );
                if ( distance <= deadzone )
                {
                    AnimateJumpEndScale = AnimateJumpEndTarget;
                    AnimateJumpEnd = false;
                }
            }
            else if ( AnimateJumpEndScale != Vector3.zero ) // Undo animation
            {
                //AnimateJumpEndScale = AnimateJumpEndScale + ( ( Vector3.zero - AnimateJumpEndScale ) * Time.deltaTime * LerpSpeed );
                AnimateJumpEndScale = Vector3.MoveTowards( AnimateJumpEndScale, Vector3.zero, Time.deltaTime * LerpSpeed );

                float distance = Vector3.Distance( AnimateJumpEndScale, Vector3.zero );
                if ( distance <= deadzone )
                {
                    AnimateJumpEndScale = Vector3.zero;
                }
            }
            scale += AnimateJumpEndScale;

            // Always wobble the scale a little
            float sin = Mathf.Sin( Time.time ) / 10;
            float cos = Mathf.Cos( Time.time ) / 10;
            scale += new Vector3( sin + cos, sin, sin * cos );
        }
        transform.GetChild( 0 ).localScale = scale;
    }

    // From: http://forum.unity3d.com/threads/throw-an-object-along-a-parabola.158855/
    IEnumerator SimulateProjectile()
    {
        //Projectile.GetComponent<Rigidbody>().isKinematic = true;
		// Play start sound
		GetComponent<AudioSource>().clip = Sound_JumpStart;
		GetComponent<AudioSource>().Play();

        // Move projectile to the position of throwing object + add some offset if needed.
        //Projectile.position = myTransform.position + new Vector3( 0, 1.0f, 0 );
        float gravity = 9.8f;
        float firingAngle = 20;

        // Calculate distance to target
        float target_Distance = Vector3.Distance( Projectile.position, TargetPosition );

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = ( target_Distance / ( Mathf.Sin( 2 * firingAngle * Mathf.Deg2Rad ) / gravity ) );

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sign( projectile_Velocity ) * Mathf.Sqrt( Mathf.Abs( projectile_Velocity ) ) * Mathf.Cos( firingAngle * Mathf.Deg2Rad );
        float Vy = Mathf.Sign( projectile_Velocity ) * Mathf.Sqrt( Mathf.Abs( projectile_Velocity ) ) * Mathf.Sin( firingAngle * Mathf.Deg2Rad );

        // Calculate flight time.
        float flightDuration = ( target_Distance / Vx );

        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation( TargetPosition - Projectile.position );

        Projectile.GetComponent<Rigidbody>().velocity = new Vector3( Vx * Projectile.transform.forward.x, Vy, Vx * Projectile.transform.forward.z );
		TargetPosition = Vector3.zero;

		// Play end sound
		GetComponent<AudioSource>().clip = Sound_JumpEnd;
		GetComponent<AudioSource>().Play();

        yield return true;
    }
}
