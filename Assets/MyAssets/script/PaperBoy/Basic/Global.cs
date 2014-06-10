using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Global {

	public static int SECOND_TO_TICKS = 10000000;

	public static float SwitchTime = 1f ;
	public static float MouseDestroyTime = 1f; 

	public static Vector3 LevelStartPosition = new Vector3( 0 , 0 , 200f );

	public static Vector3 staticPosition = new Vector3( 0 , 0 , 50f );
	public static Vector3 BstaticPosition = new Vector3( 0 , 0 , 0 );
	public static Vector3 BCameraPosition = new Vector3( 0 , 0 , -1f );
	public static Vector3 BCursorPosition = new Vector3( 0 , 0 , -0.5f );
	public static Vector3 BHeroArmOff = new Vector3( 0 , 0 , 0.5f );


	public static string HandStayTag = "ROCK" ;

	public static string HeroTag = "HERO";

	public static string MouseLeft = "MOUSE_LEFT";
	public static int MouseLeftInt = 0;
	
	public static string MouseRight = "MOUSE_RIGHT";
	public static int MouseRightInt = 1;
	
	public static string MouseMid = "MOUSE_MID";
	public static int MouseMidInt = 2;

	public static string[] CollidableTag = {"ROCK","OUTER"};

	public static Dictionary<string,string> CatchEffectResourceDict
		= new Dictionary<string, string> {
		{"SpinCW" , "Effect/Hero/FeatherCW"},
		{"SpinAntiCW" , "Effect/Hero/FeatherAntiCW"},
		{"Pull" , "Effect/Hero/FeatherCW"}};
	
}
