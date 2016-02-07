using UnityEngine;
using System.Collections;

public class PlayerHandAnimationScript : MonoBehaviour
{
	public struct AnimationInfo
	{
		public int Index;
		public float LerpTime;
		public float LerpCompleteTime;
		public float HoldTime;
		public float HoldCompleteTime;
	}

	public bool IsLeft = false;
	public GameObject[] AnimationKeyFrames;

	// Stack of animations to play, 0 is idle
	private ArrayList AnimationStack = new ArrayList();
	private AnimationInfo CurrentAnimationInfo;
	// The index of the currently playing animation
	private int CurrentAnimation = -1;
    // The time at which the last animation change occured
    private float LastAnimationChange = 0;

	void Start()
	{
		AnimationInfo idle = new AnimationInfo();
		{
			idle.Index = 0;
			idle.LerpTime = 1;
			idle.HoldTime = -1; // Hold until a new animation is added
		}
		AnimationStack.Add( idle );

        if ( IsLeft )
        {
            LastAnimationChange += 1000;
        }
	}

	void Update()
	{
		if ( AnimationStack.Count == 0 ) return;

        TryIdle();

		// Check for a new animation
		int maxanim = AnimationStack.Count - 1;
		if ( CurrentAnimation != maxanim )
		{
			CurrentAnimation = maxanim;

			// Store the information about this animation for rendering currently
			CurrentAnimationInfo = (AnimationInfo) AnimationStack.ToArray()[CurrentAnimation];
			CurrentAnimationInfo.LerpCompleteTime = Time.time + CurrentAnimationInfo.LerpTime;
			CurrentAnimationInfo.HoldCompleteTime = -1;

            LastAnimationChange = Time.time;
        }

		// Lerp to new animation position
		if ( !AnimationKeyFrames[CurrentAnimationInfo.Index] ) return;
		// For each part of this arm, lerp to new key frame position
		Transform[] keyframes = AnimationKeyFrames[CurrentAnimationInfo.Index].GetComponentsInChildren<Transform>();
		int index = 0;
		float progress = Mathf.Clamp( 1 - ( CurrentAnimationInfo.LerpTime * ( CurrentAnimationInfo.LerpCompleteTime - Time.time ) ), 0, 1 );
		foreach ( Transform transform in GetComponentsInChildren<Transform>() )
		{
			if ( index >= ( keyframes.Length ) ) break;

			transform.localPosition = Vector3.Lerp(
				transform.localPosition,
				keyframes[index].localPosition,
				progress
			);
			transform.localRotation = Quaternion.Lerp(
				transform.localRotation,
				keyframes[index].localRotation,
				progress
			);
			index++;
		}
		// Begin holding the animation until the end
		if ( ( progress == 1 ) && ( CurrentAnimationInfo.HoldCompleteTime == -1 ) )
		{
			if ( CurrentAnimationInfo.HoldTime != -1 )
			{
				CurrentAnimationInfo.HoldCompleteTime = Time.time + CurrentAnimationInfo.HoldTime;
			}
		}

		// Pop animation when finished
		if ( ( CurrentAnimationInfo.HoldCompleteTime != -1 ) && ( CurrentAnimationInfo.HoldCompleteTime <= Time.time ) )
		{
			PopAnimation();
		}
	}

    private void TryIdle()
    {
        if ( AnimationStack.Count > 1 ) return;

        float time = Time.time - LastAnimationChange;
        float chance = Random.Range( 0.0f, 10000.0f );
        if ( chance < time )
        {
            PushAnimation( 2, 1, 0.4f );
            PushAnimation( 1, 1, 0.4f );
            LastAnimationChange = Time.time;
        }
    }

	public void PushAnimation( int animindex, float animlerptime, float animholdtime )
	{
		AnimationInfo newanim = new AnimationInfo();
		{
			newanim.Index = animindex;
			newanim.LerpTime = animlerptime;
			newanim.HoldTime = animholdtime;
		}
		AnimationStack.Add( newanim );
	}

	public void PopAnimation()
	{
		if ( AnimationStack.Count <= 1 ) return;

		AnimationStack.RemoveAt( AnimationStack.Count - 1 );
	}

    public int GetAnimation()
    {
        return CurrentAnimationInfo.Index;
    }
}
