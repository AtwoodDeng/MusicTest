using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PLevelManager : MonoBehaviour {
	
	static public PLevelManager instance;
	
	public int index
	{
		get{
			return getIndex();
		}
		set{
			
		}
	}
	
	public DateTime startTime;
	public DateTime tempTime;
	public DateTime stopTime;

	//init
	public int initLevelID;

	//level
	public PLevel tempLevel;
	List<PLevel> levelList = new List<PLevel>();
	
	
	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		RestartLevel();
	}

	void OnEnable() {
		PEventManager.Instance.RegisterEvent (EventDefine.OnSwitchLevel, OnSwitchLevel);
		PEventManager.Instance.RegisterEvent ( EventDefine.OnLevelStart , OnLevelStart );
		
	}
	
	void OnDisable() {
		PEventManager.Instance.UnregisterEvent (EventDefine.OnSwitchLevel, OnSwitchLevel);
		PEventManager.Instance.UnregisterEvent ( EventDefine.OnLevelStart , OnLevelStart );
	}

	public void OnLevelStart(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( msg.ContainMessage( "levelID" ) )
		{
			SwitchLevel( Convert.ToInt32(msg.GetMessage("levelID" )) );
		}

	}


	public void OnSwitchLevel(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( msg.ContainMessage("toward"))
		{
			if ( "last".Equals( msg.GetMessage("toward")))
				StartLastLevel();
			else if ( "next".Equals( msg.GetMessage("toward")))
				StartNextLevel();
		}
	}

	
	void RestartLevel()
	{
		startTime = System.DateTime.Now;
	}
	
	// Update is called once per frame
//	void Update () {
//
//	}
	
	public int getIndex(){
		tempTime = System.DateTime.Now;
		return Convert.ToInt32( (tempTime - startTime).TotalMilliseconds);
	}
	
	public PLevel CreateLevel( int levelID )
	{
		foreach( PLevel level in levelList )
		{
			if ( level.GetLevelID() == levelID )
				return level;
		}

		GameObject levelPrefab = (GameObject)Resources.Load ("Level/Level" + levelID.ToString ());

		if ( levelPrefab == null )
		{
			return null;
		}

		GameObject levelObj = (GameObject)Instantiate (levelPrefab);
		levelObj.transform.parent = transform;
		levelObj.transform.localPosition = Vector3.zero;
		PLevel res =levelObj.GetComponent<PLevel> ();

		levelList.Add(levelObj.GetComponent<PLevel> ());

		return res;
	}


	public void StartLastLevel()
	{
		if ( ! SwitchLevel ( LastLevelID(tempLevel.GetLevelID()) ))
			Debug.Log( "start last level fail");
		else
			Invoke( "RestartLevel" , Global.SwitchTime );
	}

	public void StartNextLevel()
	{
		
		if ( ! SwitchLevel ( NextLevelID( tempLevel.GetLevelID()) ))
			Debug.Log( "start next level fail");
		else
			Invoke( "RestartLevel" , Global.SwitchTime );
	}

	public bool SwitchLevel( int levelID )
	{
		PLevel newLevel = null;
		if ( ( newLevel = CreateLevel( levelID )) == null )
			return false;
		if ( tempLevel != null )
			tempLevel.SwitchOut();
		newLevel.SwitchIn();
		tempLevel = newLevel;
		//RestartLevel();
		return true;
	}

	public int tempLevelID()
	{
		if ( tempLevel != null )
			return tempLevel.GetLevelID();

		return 0;
	}

	public static int LastLevelID( int tempLevelID)
	{
		return tempLevelID -1 ;
	}
	public static int NextLevelID( int tempLevelID )
	{
		return tempLevelID + 1 ;
	}
	
//	public void EndLevel()
//	{
//		if (levelObj != null) {
//			levelObj.GetComponent<BoyLevel>().FadeOut();
//			levelObj = null;
//		}
//		BoyManager.instance.Clear ();
//	}
//	
//	public void EnterCastle()
//	{
//		//Debug.Log ("Enter Castle");
//		SendMessage("OnLoopEnd" , SendMessageOptions.DontRequireReceiver );
//		tempLevel++;
//		EndLevel ();
//		StartLevel (tempLevel);
//	}
//	
//	public void EndPaperLevel()
//	{
//		if (levelObj != null) {
//			levelObj.GetComponent<BoyLevel>().FadeOut();
//			levelObj = null;
//		}
//	}
//	
//	public void EnterFinnal()
//	{
//		SendMessage("OnLoopEnd" , SendMessageOptions.DontRequireReceiver );
//		tempLevel++;
//		EndPaperLevel ();
//		StartLevel (tempLevel);
//	}
	
//	public Vector3 startPosition()
//	{
//		Vector3 res = levelCom.start.transform.position;
//		res.z = AudioManager.staticZ;
//		return res;
//	}
}
