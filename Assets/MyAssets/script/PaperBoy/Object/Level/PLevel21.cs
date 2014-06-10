using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class PLevel21 : PLevel {

	public GameObject group1;
	public GameObject group2;
	private Vector3 group1Pos = Global.staticPosition;
	private Vector3 group2Pos = Global.staticPosition;



	public override int GetLevelID()
	{
		return 21;
	}

	public override void SwitchIn()
	{

		HOTween.To( group1.transform
		           , Global.SwitchTime
		           , "localPosition"
		           , group1Pos
		           , false
		           , EaseType.EaseInOutQuart
		           , 0);
		
		HOTween.To( group2.transform
		           , Global.SwitchTime
		           , "localPosition"
		           , group2Pos
		           , false
		           , EaseType.EaseInOutQuart
		           , 0);

//		HOTween.To( group1.transform 
//		           , 1f 
//		           , new TweenParms()
//		           .Prop( "localposition" , new Vector3( 0 , 0 , 50f ) , true ).Loops(1)
//		           .Ease( EaseType.EaseInOutQuart ));
//		
//		HOTween.To( group2.transform 
//		           , 1f 
//		           , new TweenParms()
//		           .Prop( "localposition" , new Vector3( 0 , 0 , 50f ) , true ).Loops(1)
//		           .Ease( EaseType.EaseInOutQuart ));

	}

	public override void SwitchOut()
	{
		HOTween.To( group1.transform
		           , Global.SwitchTime
		           , "localPosition"
		           , new Vector3( 0 , 0 , 50f ) 
		           , true
		           , EaseType.EaseInOutQuart
		           , 0);
		
		HOTween.To( group2.transform
		           , Global.SwitchTime
		           , "localPosition"
		           , new Vector3( 0 , 0 , 50f )
		           , true
		           , EaseType.EaseInOutQuart
		           , 0);

//		HOTween.To( group1.transform 
//		           , 1f 
//		           , new TweenParms().Prop( "localposition" , new Vector3( 0 , 0 , -50f ) , true )
//		           .Loops(1)
//		           .Ease( EaseType.EaseInOutQuart ));
//		
//		HOTween.To( group2.transform 
//		           , 1f 
//		           , new TweenParms().Prop( "localposition" , new Vector3( 0 , 0 , -50f ) , true )
//		           .Loops(1)
//		           .Ease( EaseType.EaseInOutQuart ));
	}

}
