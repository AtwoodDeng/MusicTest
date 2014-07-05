using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Spining : MonoBehaviour {

	public bool ifStartSpin;

	public float SpinDuration;
	public EaseType SpinEaseType;
	public LoopType SpinLoopType;
	public int SpinLoopTimes; 

	// Update is called once per frame
	void Update () {
		if ( ifStartSpin )
		{
			ifStartSpin = false;
			StartSpin();
		}
	}

	void StartSpin()
	{
		HOTween.To( transform.rotation
		           , SpinDuration 
		           , "eulerAngles" 
		           , new TweenParms()
		           .Prop( "eulerAngles" , new Vector3( 0 , 0 , 360f ) , true )
		           .Loops( SpinLoopTimes , SpinLoopType )
		           );
	}
}
