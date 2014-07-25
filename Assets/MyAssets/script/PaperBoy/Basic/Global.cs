using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Global {

	public static int SECOND_TO_TICKS = 10000000;

	public static float SwitchTime = 1f ;
	public static float MouseDestroyTime = 1f; 
	public static float ObjDestroyTime = 10f;

	public static Vector3 LevelStartPosition = new Vector3( 0 , 0 , 200f );

	public static Vector3 staticPosition = new Vector3( 0 , 0 , 50f );
	public static Vector3 BstaticPosition = new Vector3( 0 , 0 , 0 );
	public static Vector3 BHeroPosition = new Vector3( 0 , 0 , 0 );
	public static Vector3 BCameraPosition = new Vector3( 0 , 0 , -1f );
	public static Vector3 BCursorPosition = new Vector3( 0 , 0 , -0.70f );
	public static Vector3 BHeroArmOff = new Vector3( 0 , 0 , 0.5f );
	public static Vector3 BEffecPosition = new Vector3( 0 , 0 , -0.5f );
	public static Vector3 BTextPosition = new Vector3( 0 , -2.35f , 0f );
	public static Vector3 BTipsPosition = new Vector3( 0 , 1f , 0f );

	public static Color ArmLeftColor = new Color( 0.1f , 0.1f , 0.1f );
	public static Color ArmRightColor = new Color( 0.6f , 0.6f , 0.6f );

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
		{"Pull" , "Effect/Hero/DotSmall"},
		{"None", ""}};
	public static Dictionary<string,string> HandStayObjCatchEffect
	= new Dictionary<string, string> {
//		{"SpinCW" , "Effect/Hero/FeatherSpinCW"},
//		{"SpinAntiCW" , "Effect/Hero/FeatherSpinAntiCW"},
		{"SpinCW" , ""},
		{"SpinAntiCW" , ""},
		{"Pull" , "Effect/Hero/DotSmall"},
		{"None", "Effect/Hero/DotSmall"}};
	public static Dictionary<string,string> ArmCatchEffectDict
	= new Dictionary<string, string> {
		{"SpinCW" , "Effect/Hero/FeatherCW"},
		{"SpinAntiCW" , "Effect/Hero/FeatherAntiCW"},
		{"Pull" , "Effect/Hero/FeatherCW"},
		{"None", ""}};
	public static Dictionary<string,string> CursorDict
	= new Dictionary<string, string> {
		{"Free" , "Tool/Prefab/CursorFree"},
//		{"PointCW" , "Tool/Prefab/CursorCW"},
//		{"PointACW" , "Tool/Prefab/CursorACW"},
		{"PointCW" , "Tool/Prefab/CursorFree"},
		{"PointACW" , "Tool/Prefab/CursorFree"},

		{"PointCatch" , "Tool/Prefab/CursorCatch"},
		{"None", ""}};

	
	public static Dictionary<string,string> nextLevelDict
	= new Dictionary<string, string> {
		{"level0" , "KafakaLv0_2"}};
	
	public static string FrontMenuPath = "Tool/Prefab/FrontMenuShow";
	public static string FrontMenuName = "FrontMenuMainBody";
	public static string FrontMenuTag = "FRONTMENU";
	public static Vector3 FrontCursorSize = new Vector3( 0.333f , 0.333f , 0.333f );

	public static float EffectDestroyTime = 5f;

	private static int _ID = 0 ;
	public static int getID(){
		Debug.Log("Global ID" + _ID.ToString());
		return _ID++;
	}

	public static string EmptyPrefabPath = "Tool/Prefab/Empty";
	public static string TextPrefabPath = "Tool/Prefab/Text";
	public static string TipsPrefabPath = "Tool/Prefab/Tips";
	public static float TextFontRate = 0.5f;
	public static float TextRelativelyRate = 0.12f;
	public static float TextShowTime = 2f;
	public static float TextDisappearTime = 1f;
	public static float TipsShowTime = 2.2f;
	public static float TipsDisappearTime = 0.1f;

	public static Dictionary<string,string> LevelScriptDictionary
	= new Dictionary<string, string> {
		{"Level1" , "text/script/level1"},
		{"Level0" , "text/script/level0"}};

	public static int LevelScriptKeyIndex = 0;

	public static float ShowDialogTimeBlock = 3f;

	public static float TextShowYRan = 0.33f;

	public static float MAX_FLOAT15 = Mathf.Pow( 2f , 15f );

	public static float A2FRate = 1 / 10f ;

	public static float LargeDrag = 99999f;
	public static float LargeAngleDrag = 99999f;



}
