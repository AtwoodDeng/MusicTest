
using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class TransEffect : MonoBehaviour {
	
	public bool ifStartUpdate = false;
	
	public float Duration;
	public EaseType EaseType;
	public LoopType LoopType;
	public int LoopTimes; 
	public Vector3 value = Vector3.zero;
	public bool isRelative = true;
	public string parmsName = "localScale";
	
	// Update is called once per frame
	void Update () {
		if ( ifStartUpdate )
		{
			ifStartUpdate = false;
			StartScale();
		}
	}
	
	void StartScale()
	{
		HOTween.To( transform
		           , Duration 
		           , new TweenParms()
		           .Prop( parmsName , value , isRelative )
		           .Loops( LoopTimes , LoopType )
		           );
	}
}
