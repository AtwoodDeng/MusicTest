using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Shine : MonoBehaviour {

	public float ScaleTurn = 2f;
	public float AngleTurn = 1f;

	public float maxScale = 1.33f;
	public float MaxAngle = 45f;

	public float ScaleDelay = 0f;
	public float AngleDelay = 0f;

	private float dST = 1;
	private float dRT = 1;
	private Vector3 oriScale;
	private Vector3 oriAngle;

	public bool ifStartOnAwake = true;


	// Use this for initialization
	void Awake () {
		oriScale = transform.localScale;
		oriAngle = transform.eulerAngles;
		dST = 1;
		dRT = 1;

		if ( ifStartOnAwake )
			DoShine();
	}

	void DoShine() {
		HOTween.To( transform 
	           , ScaleTurn / 2 
	           , new TweenParms()
	           .Prop("localScale" 
	      		, oriScale * maxScale)
	           .Ease(EaseType.EaseInOutQuart)
	           .Loops( 999 , LoopType.Yoyo )
	           .Delay( ScaleDelay )
	           );
		
		HOTween.To( transform 
		           , AngleTurn / 2 
		           , new TweenParms()
		           .Prop("eulerAngles" 
		      		, new Vector3( 0 , 0 , MaxAngle )
		      		, true)
		           .Ease(EaseType.EaseInOutQuart)
		           .Loops( 999 , LoopType.Yoyo )
		           .Delay( AngleDelay ) );

	}
	
	// Update is called once per frame
	void Update () {
//		if ( enabled )
//		{
//			dST += Time.deltaTime / ScaleTurn;
//			if ( dST > 1f ) 
//			{
//				dST = 0;
//				HOTween.To( transform 
//				           , ScaleTurn / 2 
//				           , new TweenParms()
//				           .Prop("localScale" 
//				      		, oriScale * maxScale)
//				           .Ease(EaseType.EaseInOutQuart)
//				           .Loops( 2 , LoopType.Yoyo )
//				           .Delay( ScaleDelay )
//				           );
//				      		
//			}
//			dRT += Time.deltaTime / AngleTurn;
//			if ( dRT > 1f )
//			{
//				dRT = 0;
//				HOTween.To( transform 
//				           , AngleTurn / 2 
//				           , new TweenParms()
//				           .Prop("eulerAngles" 
//				      		, new Vector3( 0 , 0 , MaxAngle )
//				      		, true)
//				           .Ease(EaseType.EaseInOutQuart)
//				           .Loops( 2 , LoopType.Yoyo )
//				           .Delay( AngleDelay ) );
//			}
//
//
//
//		}
	}



	void OnBecameVisible()
	{
		enabled = true;
		dST = 1;
		dRT = 1;
	}

	void OnBecameInvisible()
	{
		enabled = false;
	}
}
