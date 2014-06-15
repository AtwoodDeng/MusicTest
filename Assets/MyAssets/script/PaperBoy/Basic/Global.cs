using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Global {

	public static int SECOND_TO_TICKS = 10000000;

	public static float SwitchTime = 1f ;
	public static float MouseDestroyTime = 1f; 
	public static float ObjDestroyTime = 5f;

	public static Vector3 LevelStartPosition = new Vector3( 0 , 0 , 200f );

	public static Vector3 staticPosition = new Vector3( 0 , 0 , 50f );
	public static Vector3 BstaticPosition = new Vector3( 0 , 0 , 0 );
	public static Vector3 BHeroPosition = new Vector3( 0 , 0 , 0 );
	public static Vector3 BCameraPosition = new Vector3( 0 , 0 , -1f );
	public static Vector3 BCursorPosition = new Vector3( 0 , 0 , -0.75f );
	public static Vector3 BHeroArmOff = new Vector3( 0 , 0 , 0.5f );
	public static Vector3 BEffecPosition = new Vector3( 0 , 0 , -0.5f );
	public static Vector3 BTipsPosition = new Vector3( 0 , 1f , 0f );



	public static string HandStayTag = "ROCK" ;

	public static string HeroTag = "HERO";

	public static string MouseLeft = "MOUSE_LEFT";
	public static int MouseLeftInt = 0;
	
	public static string MouseRight = "MOUSE_RIGHT";
	public static int MouseRightInt = 1;
	
	public static string MouseMid = "MOUSE_MID";
	public static int MouseMidInt = 2;

	public static string[] CollidableTag = {"ROCK","OUTER"};

	public static Dictionary<string,string> HandCatchEffectDict
		= new Dictionary<string, string> {
		{"SpinCW" , "Effect/Hero/DotSmall"},
		{"SpinAntiCW" , "Effect/Hero/DotSmall"},
		{"Pull" , "Effect/Hero/DotSmall"}};
	public static Dictionary<string,string> ArmCatchEffectDict
	= new Dictionary<string, string> {
		{"SpinCW" , "Effect/Hero/FeatherCW"},
		{"SpinAntiCW" , "Effect/Hero/FeatherAntiCW"},
		{"Pull" , "Effect/Hero/FeatherCW"}};

	public static float EffectDestroyTime = 5f;

	private static int _ID = 0 ;
	public static int getID(){
		Debug.Log("Global ID" + _ID.ToString());
		return _ID++;
	}

	public static string EmptyPrefabPath = "Tool/Prefab/Empty";
	public static string TextPrefabPath = "Tool/Prefab/Text";
	public static string TipsPrefabPath = "Tool/Prefab/Tips";
	public static float TextRelativelyRate = 0.7f;

	public static Dictionary<string,string> LevelScriptDictionary
	= new Dictionary<string, string> {
		{"Level1" , "text/script/level1"}};

	public static int LevelScriptKeyIndex = 0;

	public static float ShowDialogTimeBlock = 3f;

	public static float TextShowYRan = 1f;

}
