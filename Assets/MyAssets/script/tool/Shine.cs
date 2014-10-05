using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Shine : MonoBehaviour {

	public float ScaleTurn = 2f;
	public float AngleTurn = 1f;
	public float PosTurn = 1f;

	public float maxScale = 1.33f;
	public float MaxAngle = 45f;
	public Vector3 MaxPos = Vector3.zero;

	public float ScaleDelay = 0f;
	public float AngleDelay = 0f;
	public float PosDelay = 0f;

//	private float dST = 1;
//	private float dRT = 1;

	private Vector3 oriScale;
	private Vector3 oriAngle;
	private Vector3 oriPos;

	public bool ifStartOnAwake = true;
	public bool isEnter = false;
	public float enterSizeRate = 10f;

	public EaseType scaleEaseType = EaseType.EaseInOutQuart;
	public EaseType angleEaseType = EaseType.EaseInOutQuart;
	public EaseType posEaseType = EaseType.EaseInOutQuart;

	public LoopType scaleLoopType = LoopType.Yoyo;
	public LoopType angleLoopType = LoopType.Yoyo;
	public LoopType posLoopType = LoopType.Yoyo;

	public int scaleLoopTime = 999;
	public int angleLoopTime = 999;
	public int posLoopTime = 999;

	// Use this for initialization
	void Awake () {
		oriScale = transform.localScale;
		oriAngle = transform.eulerAngles;
		oriPos = transform.position;
//		dST = 1;
//		dRT = 1;

		if ( ifStartOnAwake )
			DoShine();
	}

	void DoEnter() {
		if ( enterSizeRate > 1 )
			enterSizeRate = 1 / enterSizeRate;
		transform.localScale *= enterSizeRate;

		HOTween.To( transform 
		           , ScaleTurn / 2 
		           , new TweenParms()
		           .Prop("localScale" 
		      		, oriScale , false)
		           .Ease(EaseType.EaseInOutQuart)
		           .Loops( 1 , LoopType.Restart )
		           .Delay( 0 )
		          
		           );
	}

	public void DoShine() {
		if ( isEnter )
		{
			DoEnter();

			isEnter = false;
			Invoke("DoShine" , ScaleTurn / 2 );
		}else {

			HOTween.To( transform 
			           , ScaleTurn / 2 
			           , new TweenParms()
			           .Prop("localScale" 
			      		, oriScale * maxScale)
			           .Ease(scaleEaseType)
			           .Loops( scaleLoopTime , scaleLoopType )
			           .Delay( ScaleDelay )
			           );
		
			HOTween.To( transform 
			           , AngleTurn / 2 
			           , new TweenParms()
			           .Prop("eulerAngles" 
			      		, new Vector3( 0 , 0 , MaxAngle )
			      		, true)
			           .Ease(angleEaseType)
			           .Loops( angleLoopTime , angleLoopType )
			           .Delay( AngleDelay ) );

			HOTween.To( transform 
			           , PosTurn / 2 
			           , new TweenParms()
			           .Prop("position" 
			     	 	, MaxPos 
			      		, true )
			           .Ease(posEaseType)
			           .Loops( posLoopTime , posLoopType )
			           .Delay( PosDelay )
			           );
			
		}
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
//		dST = 1;
//		dRT = 1;
	}

	void OnBecameInvisible()
	{
		enabled = false;
	}
}
