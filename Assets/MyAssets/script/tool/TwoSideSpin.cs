using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class TwoSideSpin : MonoBehaviour {

	public int state = 0;
	public float SpinDuration = 1f;
	public EaseType SpinEaseType ;
	public float SpinAngle = 180f;

	public int toState = 0;
	public bool isSpin = false;


	float originalAngle = 0;
	void Awake()
	{
		originalAngle = transform.eulerAngles.z;
	}

	public void Spin( int _toState )
	{
		toState = _toState;
	}

	public void Spin()
	{
		toState = (state + 1) % 2;
	}


	// Update is called once per frame
	void Update () {
		GUIDebug.add(ShowType.label , "state " + state );
		GUIDebug.add(ShowType.label , "2state " + toState );
		if ( state != toState && !isSpin )
		{
			isSpin = true;

			HOTween.To( transform
			           , SpinDuration 
			           , new TweenParms()
			           .Prop( "eulerAngles" , new Vector3( 0 , 0 , State2Angle(toState) ) , false )
			           .Ease( SpinEaseType )
			           .OnComplete( SpinComplete )
			           );
		}
	}

	public float State2Angle( int state )
	{
		return originalAngle + state * SpinAngle;
	}

	public void SpinComplete(){
		isSpin = false;
		state = toState ;
	}
}
