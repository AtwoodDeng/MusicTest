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
		public string levelName;
		List<ClickEntry> clickList = new List<ClickEntry>();
		List<CatchEntry> catchList = new List<CatchEntry>();
		static char SPLIT_ITEM = '\n';
		static char SPLIT_ENTRY = '|';
		static char SPLIT_LIST = '=';
		public int depth = 0;

		public LevelHistory( int _dep )
		{
			depth = _dep;
		}

		static public string LvlName2Doc( string levelName , string docName ="" )
		{
			if ( docName == "" )
				return Directory.GetCurrentDirectory() + "/" + Global.OPP_HISTORY_DIR + "/" + levelName;
			return Directory.GetCurrentDirectory() + "/" + Global.OPP_HISTORY_DIR + "/" + levelName + "/" + docName;
		}

		static public string[] GetDocNameList( string levelName )
		{
			if ( Directory.Exists( LvlName2Doc( levelName ) ) )
				return Directory.GetFiles( LvlName2Doc( levelName ));
			return null;
		}

		public void LoadFromDir( string lvlName , string docName )
		{

			levelName = lvlName;
			if ( File.Exists( docName ) )
			{
				try{
					StreamReader sr = new StreamReader( docName );
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
				if ( string.IsNullOrEmpty( entries[0] ) || string.IsNullOrEmpty( entries[1] ))
					continue;
				addClick( float.Parse( entries[0]) , Global.Str2V3(entries[1]) );
			}
		}

		public void LoadCatchList(string str)
		{
			string[] items = str.Split(SPLIT_ITEM);
			foreach( string item in items )
			{
				string[] entries = item.Split(SPLIT_ENTRY);
				if ( string.IsNullOrEmpty( entries[0] ) 
				    || string.IsNullOrEmpty( entries[1] )
				    || string.IsNullOrEmpty( entries[2] ) )
					continue;
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
				res += clickList[i].time.ToString() + SPLIT_ENTRY 
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
				res += catchList[i].time.ToString() + SPLIT_ENTRY 
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

		int clickInd = 0;
		int catchInd = 0;

		public ClickEntry getClick( float time )
		{
			if ( clickInd < clickList.Count && time > clickList[clickInd].time )
			{
				return clickList[clickInd++];
			}
			return null;
		}

		public CatchEntry getCatch( float time )
		{
			if ( catchInd < catchList.Count && time > catchList[catchInd].time )
			{
				return catchList[catchInd++];
			}
			return null;
		}

		public void Refresh()
		{
			clickInd = catchInd = 0;
		}

	}


	public LevelHistory currentHis;
	public List<LevelHistory> hisList = new List<LevelHistory>();



	void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnMouseClick , OnMouseClick );
		BEventManager.Instance.RegisterEvent (EventDefine.OnAfterCatch , OnAfterCatch );
		BEventManager.Instance.RegisterEvent (EventDefine.OnBackClick , OnBackClick );
	}
	
	void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMouseClick , OnMouseClick);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnAfterCatch , OnAfterCatch);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnBackClick , OnBackClick);
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
	public void OnBackClick(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		GameObject effectPre = Resources.Load( Global.MouseBackClickEffectPath ) as GameObject;
		GameObject effect = Instantiate( effectPre ) as GameObject;
		effect.transform.parent = BObjManager.Instance.Effect.transform;
		effect.transform.position = Global.Str2V3( msg.GetMessage( "globalPos" ) );

		if ( msg.ContainMessage( "HisDep" ) )
		{
			int depth = int.Parse( msg.GetMessage( "HisDep" ));
			ParticleSystem[] particles = effect.GetComponentsInChildren<ParticleSystem>();
			foreach ( ParticleSystem p in particles )
			{
				Color col = p.startColor;
				col.a *= DepthConceal( depth );
				p.startColor = col;
			}
		}
	}

	string levelName;


	void Awake()
	{
		LoadHistories();
		currentHis = new LevelHistory(-1);
	}
	
	void LoadHistories()
	{
		hisList.Clear();
		levelName = BObjManager.Instance.tempLevel.levelName;
		string[] files = LevelHistory.GetDocNameList( levelName );
		int dep = 0;
		if ( files != null )
			for( int i = files.Length -1 ; i > files.Length - Global.HIS_SHOW_NUMBER - 1 
			    && i >= 0; i-- )
			{
				LevelHistory his = new LevelHistory( dep++ );
				his.LoadFromDir( levelName , files[i] );
				hisList.Add( his );
			}

	}

	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.S ))
		{
			currentHis.SaveToDir();
			Debug.Log( "save to " + Directory.GetCurrentDirectory() );
		}
		float time = BObjManager.Instance.tempLevel.time;
		foreach( LevelHistory his in hisList )
		{
			HisClick( his.getClick( time ) , his.depth );
			HisCatch( his.getCatch( time ) , his.depth );
		}
	}
	void HisClick( ClickEntry click , int dep )
	{
		if ( click == null )
			return;
		MessageEventArgs msg = new MessageEventArgs();
		msg.AddMessage( "HisDep" , dep.ToString() );
		msg.AddMessage( "globalPos" , Global.V32Str( click.position ) );
		BEventManager.Instance.PostEvent( EventDefine.OnBackClick , msg );

	}
	void HisCatch( CatchEntry cat , int dep )
	{
		if ( cat == null )
			return;

		GameObject effectPre = HeroHand.getCatchEffect( BlackWhiteRock.getForceType( cat.type) );
		CreateEffectAt( effectPre , cat.position ,dep );
	}

	void CreateEffectAt( GameObject effectPre, Vector3 pos , int dep)
	{
		GameObject effect = Instantiate( effectPre  ) as GameObject;
		effect.transform.parent = BObjManager.Instance.Effect.transform;
		effect.transform.position = pos;

		ParticleSystem[] particles = effect.GetComponentsInChildren<ParticleSystem>();
		for( int i = 0 ;i < particles.Length ; ++i )
		{
			particles[i].startColor = adjustColor( particles[i].startColor , dep );
		}
	}

	public Color adjustColor( Color col , int dep )
	{
		Color res = col;
		res.a *= DepthConceal( dep );
		res.r = ( res.r + 0.5f ) / 2;
		res.g = ( res.g + 0.5f ) / 2;
		res.b = ( res.b + 0.5f ) / 2;
		return res;
	}

	float DepthConceal( int depth )
	{
		return ( 1 - ( depth + 1 ) / ( Global.HIS_SHOW_NUMBER + 1 ) ) * 0.5f;
	}

}


