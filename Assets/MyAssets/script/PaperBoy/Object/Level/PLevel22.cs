using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class PLevel22 : PLevel {
	
	public GameObject group1;
	public GameObject group2;
	private Vector3 group1Pos = Global.staticPosition;
	private Vector3 group2Pos = Global.staticPosition;

	
	public override int GetLevelID()
	{
		return 22;
	}
	
	public override void SwitchIn()
	{
		HOTween.To( group1.transform 
		           , 1f 
		           , new TweenParms()
		           .Prop( "localPosition" , group1Pos , false )
		           .Ease( EaseType.EaseInOutQuart ));
		
		HOTween.To( group2.transform 
		           , 1f 
		           , new TweenParms()
		           .Prop( "localPosition" , group2Pos , false )
		           .Ease( EaseType.EaseInOutQuart ));
		
	}
	
	public override void SwitchOut()
	{
		HOTween.To( group1.transform 
		           , 1f 
		           , new TweenParms()
		           .Prop( "localPosition" , new Vector3( 0 , 0 , 50f ) , true )
		           .Ease( EaseType.EaseInOutQuart ));
		
		HOTween.To( group2.transform 
		           , 1f 
		           , new TweenParms()
		           .Prop( "localPosition" , new Vector3( 0 , 0 , 50f ) , true )
		           .Ease( EaseType.EaseInOutQuart ));
	}
	
}
