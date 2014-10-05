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
	public static string HeroHandTag = "HERO_HAND";

	public static string MouseLeft = "MOUSE_LEFT";
	public static int MouseLeftInt = 0;
	
	public static string MouseRight = "MOUSE_RIGHT";
	public static int MouseRightInt = 1;
	
	public static string MouseMid = "MOUSE_MID";
	public static int MouseMidInt = 2;


	public static string TriggableMessage = "msg";
	public static string PeanutMessage = "get_peanut";
	public static string EndPointMessage = "on_end_point";


	public static string[] CollidableTag = {"ROCK","OUTER"};

	public static Dictionary<string,string> HandCatchEffectDict
		= new Dictionary<string, string> {
		{"SpinCW" , "Effect/Hero/DropSmallCW"},
		{"SpinAntiCW" , "Effect/Hero/DropSmallAntiCW"},
		{"Pull" , "Effect/Hero/DotSmall"},
		{"None", "Effect/Hero/DotSmall"}};
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
		{"SpinCW" , ""},
		{"SpinAntiCW" , ""},
		{"Pull" , "Effect/Hero/FeatherStable"},
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
		{"CrowLevel0", "CrowLevel1"},
		{"CrowLevel1" , "CrowLevel2"},
		{"CrowLevel2" , "CrowLevel3"},
		{"CrowLevel3" , "CrowLevel4"},
		{"CrowLevel4" , "CrowLevel0"},
		{"level0" , "KafakaLv0_2"}};

	public static string ScreenCloudPath = "Tool/Prefab/ScreenCloud";

	public static string FrontMenuPath = "Tool/Prefab/FrontMenuShow";
	public static string FrontMenuName = "FrontMenuMainBody";
	public static string FrontMenuTag = "FRONTMENU";
	public static Vector3 FrontCursorSize = new Vector3( 0.333f , 0.333f , 0.333f );

	public static float EffectDestroyTime = 5f;

	private static int _ID = 0 ;
	public static int getID(){
		return _ID++;
	}

	public static string EmptyPrefabPath = "Tool/Prefab/Empty";
	public static string TextPrefabPath = "Tool/Prefab/Text";
	public static string TipsPrefabPath = "Tool/Prefab/Tips";
	public static string WordPrefabPath = "Tool/Prefab/Word";
	public static float TextFontRate = 0.5f;
	public static float TextRelativelyRate = 0.12f;
	public static float TextShowTime = 2f;
	public static float TextDisappearTime = 1f;
	public static float TipsShowTime = 2.2f;
	public static float TipsDisappearTime = 0.1f;
	public static float WordRotateAngle = 30f;

	
	public static string BeginPointEffect = "Effect/level/BeginLevel";
	public static string EndPointEffect = "Effect/level/EndLevel";

	public static Dictionary<string,string> LevelScriptDictionary
	= new Dictionary<string, string> {
		{"CrowLevel4" , "text/script/CrowLevel4"},
		{"CrowLevel3" , "text/script/CrowLevel3"},
		{"CrowLevel2" , "text/script/CrowLevel2"},
		{"CrowLevel1" , "text/script/CrowLevel1"},
		{"CrowLevel0" , "text/script/CrowLevel0"},
		{"Level1" , "text/script/level1"},
		{"Level0" , "text/script/level0"}
	};

	public static float MIN_HAND_FORCE_LENGTH = 0.25f;
	public static float MAX_HAND_FORCE = 0.03f;

	public static int LevelScriptKeyIndex = 0;

	public static float ShowDialogTimeBlock = 3f;

	public static float TextShowYRan = 0.33f;

	public static float MAX_FLOAT15 = Mathf.Pow( 2f , 15f );

	public static float A2FRate = 1 / 10f ;

	public static float LargeDrag = 99999f;
	public static float LargeAngleDrag = 99999f;

	public static float MAX_HEALTH = 1.0f;
	public static float HURT_HEALTH1 = 0.45f;
	public static float HURT_HEALTH2 = 0.45f;
	public static float HURT_HEALTH3 = 0.45f;
	public static float MIN_HEALTH = 0.03f;

	public static float MIN_SCREEN_VIEW = 0.4f;

	public static float EndLevelTime = 3f;
	
	public static float FORCE_NORMAL = 1f;

	public static string RECOVER_POINT_TAG = "RECOVER_POINT";
	public static string RECOVER_MSG = "RecoverPoint";

	public static float DEAD_START_TIME = 0.33f;
	
	public static string DeadTurnBlackEffect = "Effect/level/DeadTurnBlack";
	public static string DeadAppearEffect = "Effect/level/DeadAppear";

	public static string V32Str( Vector3 v3)
	{
		return v3.x + "$" + v3.y + "$" + v3.z;
	}
	public static Vector3 Str2V3( string str)
	{
		string[] v3s = str.Split('$');
		return new Vector3( float.Parse(v3s[0]) , float.Parse(v3s[1]) , float.Parse(v3s[2]));
	}

	public static float BACK_FLOW_DESTORY_TIME = 1f;

	public static string OPP_HISTORY_DIR = "crow.app/Contents/Resources/His";
	public static string OPP_HISTORY_SUFFIX = ".his";

	public static int HIS_SHOW_NUMBER = 5;

	public static string MouseBackClickEffectPath = "Effect/Hero/DotSmall";

	public static float adjustAngle( float _angle)
	{
		while ( _angle > 180f )
			_angle -= 360f;
		while ( _angle < -180f )
			_angle += 360f;
		return _angle;
	}

}
