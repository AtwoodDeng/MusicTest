using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class BHistoryManager : MonoBehaviour {

	public class ClickEntry
	{
		public float time;
		public Vector3 position;
		public ClickEntry( float _time, Vector3 _pos )
		{
			time = _time;
			position = _pos;
		}
	}
	public class CatchEntry
	{
		public float time;
		public Vector3 position;
		public BlackWhiteRock.SpinType type;
		public CatchEntry( float _time, Vector3 _pos, BlackWhiteRock.SpinType _type )
		{
			time = _time;
			position = _pos;
			type = _type;
		}
	}

	public class LevelHistory
	{
		string levelName;
		List<ClickEntry> clickList = new List<ClickEntry>();
		List<CatchEntry> catchList = new List<CatchEntry>();
		static char SPLIT_ITEM = '\n';
		static char SPLIT_ENTRY = '|';
		static char SPLIT_LIST = '=';

		public string LvlName2Doc( string levelName , string docName ="" )
		{
			if ( docName == "" )
				return System.Environment.CurrentDirectory + "/" + Global.OPP_HISTORY_DIR + "/" + levelName;
			return System.Environment.CurrentDirectory + "/" + Global.OPP_HISTORY_DIR + "/" + levelName + "/" + docName;
		}

		public void LoadFromDir( string lvlName , string docName )
		{

			levelName = lvlName;
			if ( File.Exists( LvlName2Doc( lvlName , docName ) ) )
			{
				try{
					StreamReader sr = new StreamReader( LvlName2Doc( lvlName , docName ) );
					string text = sr.ReadToEnd();
					string[] lists = text.Split(SPLIT_LIST);
					LoadClickList( lists[0] );
					LoadCatchList( lists[1] );
				}catch( Exception e )
				{
					Debug.LogError( "file error : " + e );
				}

			}
		}

		public void LoadClickList(string str)
		{
			string[] items = str.Split(SPLIT_ITEM);
			foreach( string item in items )
			{
				string[] entries = item.Split(SPLIT_ENTRY);
				addClick( float.Parse( entries[0]) , Global.Str2V3(entries[1]) );
			}
		}

		public void LoadCatchList(string str)
		{
			string[] items = str.Split(SPLIT_ITEM);
			foreach( string item in items )
			{
				string[] entries = item.Split(SPLIT_ENTRY);
				addCatch( float.Parse( entries[0] ) 
				         , Global.Str2V3( entries[1] )
				         , (BlackWhiteRock.SpinType)Enum.Parse(typeof(BlackWhiteRock.SpinType) , entries[2]) );
			}
		}

		public void SaveToDir( string lvlName = "" )
		{
			levelName = BObjManager.Instance.tempLevel.levelName;

			try{
				if ( !Directory.Exists( LvlName2Doc( levelName )) )
				{
					Directory.CreateDirectory(LvlName2Doc( levelName ) );
				}
				string fileName = Directory.GetFiles( LvlName2Doc( levelName ) ).Length + Global.OPP_HISTORY_SUFFIX;

				StreamWriter sw = new StreamWriter(LvlName2Doc(levelName , fileName) );
				sw.Write( Click2Str() );
				sw.Write( Catch2Str() ); 
				sw.Close();
			}catch(Exception e )
			{
				Debug.LogError( "[file error]" + e );
			}
		}

		public string Click2Str()
		{
			string res = "";
			for( int i = 0 ; i < clickList.Count ; ++ i )
			{
				res += clickList[i].time + SPLIT_ENTRY 
					+ Global.V32Str( clickList[i].position ) + SPLIT_ITEM;
			}
			res += SPLIT_LIST;
			return res;
		}
		public string Catch2Str()
		{
			string res = "";
			for( int i = 0 ; i < catchList.Count ; ++ i )
			{
				res += catchList[i].time + SPLIT_ENTRY 
					+ Global.V32Str( catchList[i].position ) + SPLIT_ENTRY
					+ catchList[i].type.ToString() + SPLIT_ITEM;
			}
			res += SPLIT_LIST;
			return res;
		}
		public void addClick( float time , Vector3 pos )
		{
			clickList.Add( new ClickEntry( time , pos ));
		}

		public void addCatch( float time, Vector3 pos , BlackWhiteRock.SpinType type )
		{
			catchList.Add( new CatchEntry( time , pos , type ));
		}
	}


	public LevelHistory currentHis;
	public List<LevelHistory> hisList = new List<LevelHistory>();

	void Awake()
	{
		LoadHistories();
		currentHis = new LevelHistory();
	}

	void LoadHistories()
	{
		hisList.Clear();
	}

	void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnMouseClick , OnMouseClick );
		BEventManager.Instance.RegisterEvent (EventDefine.OnAfterCatch , OnAfterCatch );
	}
	
	void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMouseClick , OnMouseClick);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnAfterCatch , OnAfterCatch);
	}

	public void OnMouseClick(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		float time = BObjManager.Instance.tempLevel.time;
		Vector3 pos = Global.Str2V3( msg.GetMessage( "globalPos" ));
		currentHis.addClick( time ,pos );

	}
	public void OnAfterCatch(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		float time = BObjManager.Instance.tempLevel.time;
		Vector3 pos = Global.Str2V3( msg.GetMessage( "CatchPositionGlobal" ));
		BlackWhiteRock.SpinType type = (BlackWhiteRock.SpinType)Enum.Parse( typeof(BlackWhiteRock.SpinType) , msg.GetMessage("type"));
		currentHis.addCatch( time ,pos , type);
	}

	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.S ))
		{
			currentHis.SaveToDir();
			Debug.Log( "save to " + Directory.GetCurrentDirectory() );
		}
	}



}


