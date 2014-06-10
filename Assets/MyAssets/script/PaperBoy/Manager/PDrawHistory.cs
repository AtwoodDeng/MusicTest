using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum PHistoryType
{
	Normal,
}

public enum PMouseHeroState
{
	MouseInHero,
	MouseOutHero,
}


public class PDrawHistory : MonoBehaviour {

	static int RECORD_INDEX_NONE = -1;
	static int RECORD_INDEX_OUT = -2;
	static Vector3 hisPosDiff = new Vector3( 0 , 0 , 10f );

	public List<UnityEngine.Object> mousePrefabs;
	public GameObject heroPrefab;

	
	//record 
	[HideInInspector]public List<RecordSet> records = new List<RecordSet>();
	RecordSet tempRecordSet;

	//temp state
	public PMouseHeroState mouseHeroState = PMouseHeroState.MouseOutHero;
//	int tempRecord = 0;

	
	//show
//	[HideInInspector]public List<int> showIndexs;
//	public List<PMouse> mouses;
//	public List<Hero> heros;

	//type to record history
	static public PHistoryType type = PHistoryType.Normal;

	
	// Use this for initialization
	void Start () {
		MessageEventArgs msg = new MessageEventArgs();
		msg.AddMessage("levelID" , "21" );
		PEventManager.Instance.PostEvent( EventDefine.OnLevelStart , msg );
//		records = new List<RecordSet> ();
//		UpdateTempRecord();
//		records.Add ( tempRecordSet );
//		mouses = new List<PMouse> ();
//		heros = new List<Hero>();
//		init ();
	}
	
	void Update(){
		Record ();
		UpdateRecord ();
	}

	void OnEnable() {
		PEventManager.Instance.RegisterEvent (EventDefine.OnSwitchLevel, OnSwitchLevel);
		PEventManager.Instance.RegisterEvent ( EventDefine.OnMouseInHero , OnMouseInHero );
		PEventManager.Instance.RegisterEvent ( EventDefine.OnMouseOutHero , OnMouseOutHero );
		PEventManager.Instance.RegisterEvent ( EventDefine.OnLevelStart , OnLevelStart );
	}
	
	void OnDisable() {
		PEventManager.Instance.UnregisterEvent (EventDefine.OnSwitchLevel, OnSwitchLevel);
		PEventManager.Instance.UnregisterEvent ( EventDefine.OnMouseInHero , OnMouseInHero );
		PEventManager.Instance.UnregisterEvent ( EventDefine.OnMouseOutHero , OnMouseOutHero );
		PEventManager.Instance.UnregisterEvent ( EventDefine.OnLevelStart , OnLevelStart );
	}

	public void OnLevelStart(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( msg.ContainMessage( "levelID" ) )
			ResetTempRecordSet( Convert.ToInt32(msg.GetMessage("levelID" )) );
		else
			ResetTempRecordSet();
		records.Add ( tempRecordSet );

	}

	public void OnMouseInHero(EventDefine eventName, object sender, EventArgs args)
	{
		mouseHeroState = PMouseHeroState.MouseInHero;
	}

	public void OnMouseOutHero(EventDefine eventName, object sender, EventArgs args)
	{
		mouseHeroState = PMouseHeroState.MouseOutHero;
	}
	
	
	public void OnSwitchLevel(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = ( MessageEventArgs ) args;
		if ( !msg.ContainMessage("levelID") || !msg.ContainMessage("toward") || !msg.ContainMessage("index"))
		{
			Debug.Log("cannot find enough message");
			return;
		}
		int tempLevelID = Convert.ToInt32( msg.ContainMessage("levelID"));
		int nextLevelID = 0;
		mouseHeroState = PMouseHeroState.MouseOutHero;

		if ( "last".Equals( msg.GetMessage("toward")) )
			nextLevelID = PLevelManager.LastLevelID( tempLevelID );
		else
		    nextLevelID = PLevelManager.NextLevelID( tempLevelID );
		int lastIndex = Convert.ToInt32( msg.GetMessage("index"));
		SwitchLevel( tempLevelID , nextLevelID , lastIndex);
	}

	public void SwitchLevel( int fromLevelID , int toLevelID , int lastIndex  )
	{
		for( int i = 0 ; i < records.Count ; ++ i )
		{
			if ( records[i].levelID == fromLevelID )
			{
//				if ( heros[i] != null )
//				heros[i].Create( Global.SwitchTime);
//				if ( mouses[i] != null )
//				mouses[i].Create(Global.SwitchTime);
				records[i].SwitchOut( lastIndex );
			}
			if ( records[i].levelID == toLevelID )
			{
//				if ( heros[i] != null )
//				heros[i].Destory( Global.SwitchTime );
//				if ( mouses[i] != null )
//				mouses[i].Destroy( Global.SwitchTime );
//				records[i].SwitchIn();
				tempRecordSet = records[i];
				tempRecordSet.CreateHero( transform , heroPrefab , HeroState.Control );
			}
		}
	}


//	public void OnLoopEnd()
//	{
//		tempRecordSet = new RecordSet();
//		records.Add( tempRecordSet);

//		init();

//		//add the index of tempRdcord
//		tempRecord ++;
//		records.Add ( new MouseRecordSet() );
//
//		//initilize the class
//		init ();
//
//		for ( int i = 0 ; i < tempRecord ; ++ i )
//		{
//			PMouse mouse = ((GameObject)Instantiate(getObjByList(mousePrefabs,i))).GetComponent<PMouse>();
//			mouse.transform.parent = this.gameObject.transform;
//			mouse.transform.localPosition = Vector3.zero;
//			mouses.Add( mouse );
//			
//		}
//		
//		Debug.Log ("History loop end ");
//	}

	public UnityEngine.Object getObjByList( List<UnityEngine.Object> list , int i )
	{
		return (list.Count > i) ? list [i] : list [list.Count - 1];
	}

//	public void init()
//	{
//		if ( showIndexs == null )
//			showIndexs = new List<int> ();
//		showIndexs.Clear ();
//		for (int i = 0; i < records.Count ; ++i)
//			showIndexs.Add(0);

//		if ( mouses != null )
//		{
//			foreach ( PMouse mouse in mouses )
//			{
//				if ( mouse != null )
//					mouse.Destroy( Global.MouseDestroyTime );
//			}
//			mouses.Clear ();
//		}else
//			mouses = new List<PMouse>();
//
//		for ( int i = 0 ; i < records.Count ; ++i )
//			mouses.Add( null );
//
//		if ( heros != null )
//		{
//			foreach( Hero hero in heros )
//			{
//				Destroy( hero.gameObject );
//			}
//			heros.Clear();
//		}else heros = new List<Hero>();
//
//		for ( int i = 0 ; i < records.Count ; ++i )
//			heros.Add( null );
//	}

	public void ResetTempRecordSet()
	{
		ResetTempRecordSet( PLevelManager.instance.tempLevelID());
	}

	public void  ResetTempRecordSet( int levelID )
	{
		if ( tempRecordSet == null )
		{
			foreach ( RecordSet rs in records )
			{
				if ( rs.levelID == levelID )
				{
					tempRecordSet = rs;
					if ( tempRecordSet.Hero != null )
						tempRecordSet.Hero.SetState( HeroState.Control );
					return;
				}
			}
			tempRecordSet = new RecordSet();
			tempRecordSet.levelID = levelID;
			tempRecordSet.CreateHero( transform , heroPrefab , HeroState.Control );
			records.Add( tempRecordSet );
		}else{
		}
	}
	
	public void Record(){

		if ( tempRecordSet == null )
			return;
		//mouse
		Vector3 pos = PMouseManager.getMousePos ();
		PMouseManager.MouseState state = PMouseManager.instance.state;
		int index = PLevelManager.instance.getIndex ();
		//Vector3 heroPos = PObjManager.instance.getHeroPosition();
		Vector3 heroPos = tempRecordSet.Hero.transform.position;

		if ( tempRecordSet == null )
			Debug.LogError("cannot find temp record in record()");
		tempRecordSet.addRecord( pos , state , index , heroPos , mouseHeroState );

//		if (tempRecord < 0 ) {
//			Debug.LogError("[PDrawHistory]temp record < 0 ");
//		} else if (tempRecord >= records.Count) {
//			Debug.LogError ("[PDrawHistory]temp record > records  count " + tempRecord + " " + records.Count);
//		} else {
//			records [tempRecord].addRecord(new RecordEntry (pos, state, index));
//		}

	}

	public void UpdateRecord()
	{
		for (int i = 0 ; i < records.Count ; ++ i) 
		{
			if ( records[i] != tempRecordSet )
				UpdateRecord (i);
			else
				UpdateTempRecord();
		}
	}
	
	/// <summary>
	/// show the i th record of mouse move
	/// </summary>
	/// <param name="i">The index.</param>
	public void UpdateRecord( int i )
	{

		////////////// Judge ////////////////
//		if (mouses == null || mouses.Count <= i)
////			return;
//		int j = showIndexs [i] ;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
		int tempIndex = PLevelManager.instance.index + tempRecordSet.LastIndex;

		int recordIndex = records [i].getRecordIndexByIndex ( tempIndex , true );
		// no such record
		if (recordIndex == RECORD_INDEX_NONE)
			return;
		else // if the record index is out of range
		if ( recordIndex == RECORD_INDEX_OUT )
		{
//			if ( mouses[i] != null )
//				mouses[i].Destroy( Global.MouseDestroyTime );
//			mouses[i] = null;
			if ( records[i].Mouse != null )
				records[i].Mouse.Destroy( Global.MouseDestroyTime );
			return;
		}
		RecordEntry record = records [i].data [recordIndex];
//		showIndexs [i] = recordIndex;


		///////////////// Update Mouse ////////////////
		//get the PMouse
		PMouse hisMouse = records[i].Mouse;

		//Debug.Log ("show " + i + "index " + j + "state " + records [i] [j].state + " pos " + records [i] [j].pos);
		
		//check if there are mouseEffect to show this record
		switch ( record.state )
		{
		case PMouseManager.MouseState.Drag:
			if ( hisMouse == null )
			{	
				records[i].CreateMouse( transform , (GameObject)getObjByList( mousePrefabs , i ));
			}
			break;
		case PMouseManager.MouseState.Point:
			if ( hisMouse == null )
			{	
				records[i].CreateMouse( transform , (GameObject)getObjByList( mousePrefabs , i ));
			}
			break;
		case PMouseManager.MouseState.Free:
			if ( hisMouse != null )
			{
				hisMouse.Destroy( Global.MouseDestroyTime );
				records[i].Mouse = null;
				return;
			}else
				return;
			break;
		default:
			return ;
			break;
		}

		//set the mouse effect to the recorded position


		if ( records[i].Mouse != null )
		{
			Debug.Log( "set mouse " + i.ToString() );
			records[i].Mouse.transform.localPosition = record.mousePos + hisPosDiff;
			records[i].Mouse.gameObject.SetActive (true);
		}


		////////////// Update Hero /////////////
		//set the hero
		if ( records[i].Hero == null )
		{	
//			heros[i] = ((GameObject)Instantiate(heroPrefab)).GetComponent<Hero>();
//			heros[i].transform.parent = this.gameObject.transform;
//			heros[i].transform.localScale = Vector3.one;
//			heros[i].transform.localPosition = Vector3.zero;
//			heros[i].gameObject.SetActive(false);
//			heros[i].SetState( HeroState.Histroy );
			records[i].CreateHero( transform , heroPrefab , HeroState.Histroy);
			records[i].Hero.transform.localScale = Vector3.one * ( 1 + i );
		}
		records[i].Hero.transform.localPosition = record.heroPos + hisPosDiff;
		records[i].Hero.gameObject.SetActive( true );

	}

	public void UpdateTempRecord()
	{
		tempRecordSet.Hero.Force();
	}

	public class RecordSet{
		public List<RecordEntry> data;
		public int levelID;
		public int totalIndex;
		public Hero Hero;
		public PMouse Mouse;
		public int LastIndex = 0;
		public int showIndex = 0 ;
		// public PMouse linkMouse;

		public void CreateMouse ( Transform parent , GameObject prefab ){	
			Mouse = ((GameObject)Instantiate(prefab)).GetComponent<PMouse>();
			Mouse.transform.parent = parent;
			Mouse.transform.localPosition = Vector3.zero;
			Mouse.gameObject.SetActive(false);
		}
		public void CreateHero( Transform parent , GameObject prefab , HeroState state){	
			Hero = ((GameObject)Instantiate(prefab)).GetComponent<Hero>();
			Hero.transform.parent = parent;
			Hero.transform.localScale = Vector3.one;
			Hero.transform.localPosition = Vector3.zero;
			Hero.gameObject.SetActive(false);
			Hero.SetState( state );
		}
		
		public RecordSet()
		{
			data = new List<RecordEntry>();
		}

		public RecordEntry addRecord( Vector3 _pos , PMouseManager.MouseState _state , int _index , Vector3 _heroPos , PMouseHeroState _MHState)
		{
			RecordEntry res = new RecordEntry (_pos, _state, _index + LastIndex , _heroPos , _MHState);
			addRecord (res);
			Hero = null;
			Mouse = null;
			return res;
		}
		public void addRecord( RecordEntry entry)
		{
			if (entry != null)
				data.Add (entry);
		}

		public int getRecordIndexByIndex( int index , bool ifupdate = false )
		{
			if (data.Count <= 0)
				return RECORD_INDEX_NONE;
			for (int i = showIndex ; i < data.Count; ++i)
				if (data[i].index > index)
				{
					if ( ifupdate )
						showIndex = i;
					return i;
				}
			//if nothing to return
			return RECORD_INDEX_OUT;
		}

		public RecordEntry getRecordByIndex ( int index , bool ifupdate = false )
		{
			int ind = 0;
			if ( ( ind = getRecordIndexByIndex( index , ifupdate )) >= 0 )
				return data[ind];
			return null;
		}

		public void SwitchOut( int _lastIndex )
		{
			if ( Hero != null )
				Hero.Create( Global.SwitchTime );
			if ( Mouse != null )
				Mouse.Create( Global.SwitchTime );
			LastIndex = _lastIndex;
		}

		public void SwitchIn( )
		{
			if ( Hero != null )
				Hero.Destory( Global.SwitchTime );
			if ( Mouse != null )
				Mouse.Destroy( Global.MouseDestroyTime );
		}

	}

	public class RecordEntry{
		public PMouseManager.MouseState state;
		public Vector3 mousePos;
		public int index;
		public PMouseHeroState MHState;
		public Vector3 heroPos;
		
		public RecordEntry( Vector3 _mousePos , PMouseManager.MouseState _state , int _index , Vector3 _heroPos , PMouseHeroState _MHState )
		{
			mousePos = _mousePos;
			state = _state;
			index = _index;
			heroPos = _heroPos;
			MHState = _MHState;
		}
	}
}
