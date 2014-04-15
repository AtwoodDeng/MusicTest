using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DrawHistoryPaper : MonoBehaviour {
	
	public List<GameObject> mouseEffectPrefabs;
	//public GameObject crossEffectPrefabs;
	
	//record 
	public List<List<MouseRecordEntry>> records;
	
	int tempRecord = -1;
	
	//show
	public List<int> showIndexs;
	public List<GameObject> mouseEffects;
	
	//cross
	//public int checkStep = 5;
	
	//gesture
	//string tempGestureName;
	//int tempGestureIndex;
	//public float gestureRecordTime = 0.2f;
	
	//type to record history
	public HistoryType type = HistoryType.Music;
	
	// Use this for initialization
	void Start () {
		records = new List<List<MouseRecordEntry>> ();
		mouseEffects = new List<GameObject> ();
		OnLoopEnd ();
	}
	
	void Update(){
		Record ();
		Show ();
		//Check();
		//CheckGesture ();
	}
	
	public void OnLoopEnd()
	{
		
		tempRecord ++;
		Debug.Log ("tempRecord " + tempRecord);
		showIndexs = new List<int> ();
		for (int i = 0; i < tempRecord + 1 ; ++i)
			showIndexs.Add(0);
		foreach ( GameObject effect in mouseEffects )
		{
			if ( effect !=null && effect.GetComponent<PaperLight>()!=null )
				effect.GetComponent<PaperLight>().Destory();
		}
		for ( int i = 0 ; i < tempRecord ; ++ i )
		{
			GameObject effect = (GameObject)Instantiate(mouseEffectPrefabs[i]);
			effect.transform.parent = this.gameObject.transform;
			effect.transform.localPosition = new Vector3( 9999f , 9999f , 9999f ) ;
			mouseEffects.Add( effect );
			
		}
		records.Add (new List<MouseRecordEntry> ());
//		tempGestureName = null;
//		tempGestureIndex = 0;
		
		Debug.Log ("History loop end ");
	}
	
	public void Record(){
		
		Vector3 pos = MouseControl.instance.pos;
		MouseControl.MouseState state = MouseControl.instance.state;
		int index = 0;
		if ( type == HistoryType.Music )
			index = AudioManager.instance.index;
		else if ( type == HistoryType.Level )
			index = LevelManager.instance.getIndex();
		
		if (tempRecord < 0) {
			
		} else if (tempRecord >= records.Count) {
			//Debug.LogError ("temp record > records  count " + tempRecord + " " + records.Count);
			
		} else {
			MyLightColor lightColor = MyLightColor.None;
			if ( MouseControl.instance.mouseEffectObj != null )
				lightColor = MouseControl.instance.mouseEffectObj.GetComponent<PaperLight>().lightColor;

			records [tempRecord].Add (new MouseRecordEntry (pos, state, index, "" , lightColor));
		}
	}
//	public void Check()
//	{
//		if (records [tempRecord].Count <= checkStep+1 ) 
//			return;
//		//check the record step
//		MouseControl.MouseState s1 = records [tempRecord] [records [tempRecord].Count - 1].state;
//		MouseControl.MouseState s2 = records [tempRecord] [records [tempRecord].Count - 1-checkStep].state;
//		if (s1 == MouseControl.MouseState.Free || s2 == MouseControl.MouseState.Free)
//			return;
//		//two recorded position
//		Vector3 p1 = records [tempRecord][records [tempRecord].Count-1].pos;
//		Vector3 p2 = records [tempRecord][records [tempRecord].Count-1-checkStep].pos;
//		
//		for ( int i = 0 ; i < tempRecord ; ++ i )
//		{
//			//check the effectiveness of the index
//			if ( showIndexs[i]-checkStep < 0 || records[i].Count <= showIndexs[i] )
//				continue;
//			
//			//check if the record is available
//			MouseControl.MouseState s3 = records[i][showIndexs[i]].state;
//			MouseControl.MouseState s4 = records[i][showIndexs[i]-checkStep].state;
//			if ( s3 == MouseControl.MouseState.Free || s4 == MouseControl.MouseState.Free )
//				continue;
//			
//			Vector3 p3 = records[i][showIndexs[i]].pos;
//			Vector3 p4 = records[i][showIndexs[i]-checkStep].pos;
//			Vector3 cross = CheckPointCross( p1 , p2 , p3 , p4 );
//			if ( cross == Vector3.zero )
//				continue;
//			
//			Debug.Log("cross!!");
//			
//			if ( crossEffectPrefabs != null )
//			{
//				GameObject effect = (GameObject)Instantiate(crossEffectPrefabs);
//				effect.transform.parent = this.transform;
//				effect.transform.localPosition = cross;
//			}
//		}
//		
//	}
//	public void CheckGesture()
//	{
//		for ( int i = 0 ; i < tempRecord ; ++ i )
//		{
//			if ( i >= records.Count )
//				break;
//			if ( records[i].Count > showIndexs[i] && records[i][showIndexs[i]].gestureName != null )
//			{
//				Debug.Log("identify a guesture in history " + records[i][showIndexs[i]].gestureName );
//				if ( this.gameObject.GetComponent<PointCloudGestureSample>() )
//					this.gameObject.GetComponent<PointCloudGestureSample>().OnCustomGesture(records[i][showIndexs[i]].gestureName);                                                            
//			}else
//			{
//			}
//		}
//	}
	
	
//	public Vector3 CheckPointCross( Vector3 v1 , Vector3 v2 , Vector3 v3 , Vector3 v4 )
//	{
//		if (v1 == v2 || v3 == v4) // can not be line
//			return Vector3.zero;
//		
//		if ( Larger( v1 , v2 ))
//		{
//			Vector3 tem = v2;
//			v2 = v1;
//			v1 = tem;
//		}
//		if ( Larger (v3, v4))
//		{
//			Vector3 tem = v4;
//			v4 = v3;
//			v3 = tem;
//		}
//		if (v1 == v3 || v1 == v4 || v2 == v3 || v2 == v4) //overlap
//			return Vector3.zero;
//		if (Vector3.Cross (v2 - v1, v4 - v3).magnitude < 1e-7f)
//			return Vector3.zero;
//		
//		float d = (v2.x - v1.x) * (v4.y - v3.y) - (v4.x - v3.x) * (v2.y - v1.y);
//		float b1 = (v2.y - v1.y) * v1.x + (v1.x - v2.x) / v1.y;
//		float b2 = (v4.y - v3.y) * v3.x + (v3.x - v4.x) * v3.y;
//		float d1 = b2 * (v2.x - v1.x) - b1 * (v4.x - v3.x);
//		float d2 = b2 * (v2.y - v1.y) - b1 * (v4.y - v3.y);
//		Vector3 cross = new Vector3 (d1 / d, d2 / d, AudioManager.staticZ);	
//		
//		//Debug.Log ("cross: " + cross);
//		
//		if (Vector3.Dot (v2 - v1, cross - v1) < 1e-7f
//		    || Vector3.Dot (v1 - v2, cross - v2) < 1e-7f
//		    || Vector3.Dot (v3 - v4, cross - v4) < 1e-7f
//		    || Vector3.Dot (v4 - v3, cross - v3) < 1e-7f )
//			return Vector3.zero;
//		return cross;
//		
//	}
//	public bool Larger( Vector3 v1 , Vector3 v2 )
//	{
//		return (v1.x > v2.x || ((v1.x - v2.x) < 1e-7f && v1.y > v2.y));
//	}
	public void Show()
	{
		for (int i = 0 ; i < tempRecord && i < records.Count ; ++ i) 
		{
			Show (i);
		}
	}
	
	/// <summary>
	/// show the i th record of mouse move
	/// </summary>
	/// <param name="i">The index.</param>
	public void Show( int i )
	{
		//Debug.Log ("Show " + i + " " + records.Count + " " + records[i].Count );
		int j = showIndexs [i];
		//check the world's index( the index of the music)
		int tempIndex = 0;
		if ( type == HistoryType.Music )
			tempIndex = AudioManager.instance.index;
		else if ( type == HistoryType.Level )
			tempIndex = LevelManager.instance.getIndex();
		
		//GUIDebug.add (ShowType.label, "show " + i + " index " + records [i] [j].index);
		if (records [i].Count <= 0)
			return;
		if (j >= records [i].Count - 1 )
			j = records [i].Count - 1;
		//set j to be the index of the record to show
		while ( j < records[i].Count && tempIndex > records[i][j].index )
		{
			j++;
		}
		
		showIndexs [i] = j;
		if ( j >= records[i].Count )
		{
			if ( mouseEffects[i] != null && mouseEffects[i].GetComponent<PaperLight>() != null )
				mouseEffects[i].GetComponent<PaperLight>().Destory();
			mouseEffects[i] = null;
			return;
		}
		
		//Debug.Log ("show " + i + "index " + j + "state " + records [i] [j].state + " pos " + records [i] [j].pos);

		//Debug.Log ("Check state " + i);
		//check if there are mouseEffect to show this record
		switch ( records[i][j].state )
		{
		case MouseControl.MouseState.Drag:
			if ( mouseEffects[i] == null )
			{	
				mouseEffects[i] = (GameObject)Instantiate(mouseEffectPrefabs[i]);
				mouseEffects[i].transform.parent = this.gameObject.transform;
				mouseEffects[i].transform.localPosition = new Vector3( 9999f , 9999f , 9999f );
			}
			break;
		case MouseControl.MouseState.Point:
			if ( mouseEffects[i] == null )
			{	
				mouseEffects[i] = (GameObject)Instantiate(mouseEffectPrefabs[i]);
				mouseEffects[i].transform.parent = this.gameObject.transform;
				mouseEffects[i].transform.localPosition = new Vector3( 9999f , 9999f , 9999f );
			}
			break;
		case MouseControl.MouseState.Free:
			if ( mouseEffects[i] != null )
			{
				if ( mouseEffects[i].GetComponent<PaperLight>() )
					mouseEffects[i].GetComponent<PaperLight>().Destory();
				mouseEffects[i] = null;
				return;
			}else
				return;
			break;
		default:
			return ;
			break;
		}
		
		mouseEffects [i].SetActive (true);
		
		//TODO for debug
		//Debug.Log ("record " + i + j + " " + records [i] [j].pos);
//		if (i >= mouseEffects.Count)
//			Debug.Log (" error 1 ");
//		if (i >= records.Count)
//			Debug.Log ("error 2 ");
//		if (j >= records [i].Count)
//			Debug.Log ("error 3 ");
//		/////
		
		//set the mouse effect to the recorded position

		switch( MouseControl.instance.followMethod )
		{
		case MouseControl.MouseFollowMethod.Immediately:
			if (mouseEffects [i])
			{
				//Debug.Log( "pos " + records[i][j].pos );
				mouseEffects [i].transform.localPosition = records [i] [j].pos;
			}
			break;
		case MouseControl.MouseFollowMethod.Delay :
			if ( mouseEffects [i].transform.localPosition == Vector3.zero )
			{
				mouseEffects [i].transform.localPosition = records [i] [j].pos;
			}else
			{
				mouseEffects [i].transform.localPosition = 
					MouseControl.instance.delayIntense * mouseEffects [i].transform.localPosition
						+ ( 1 - MouseControl.instance.delayIntense ) * records [i] [j].pos;
			}
			break;
		case MouseControl.MouseFollowMethod.LimitSpeed:
			Vector3 toward = - mouseEffects[i].transform.localPosition + records [i] [j].pos;
			if ( toward.magnitude > MouseControl.instance.limitSpeed )
				toward = toward.normalized * MouseControl.instance.limitSpeed ;
			mouseEffects[i].transform.localPosition =
				mouseEffects[i].transform.localPosition + toward;
			break;
		default:
			break;
		};

		//set the light 
		mouseEffects [i].GetComponent<PaperLight> ().ChangeColorTo (records [i] [j].color);
	}
	
//	void OnCustomGesture( PointCloudGesture gesture )
//	{
//		if ( type == HistoryType.Music )
//		{
//			tempGestureName = gesture.RecognizedTemplate.name;
//			tempGestureIndex = AudioManager.instance.index;
//			Debug.Log ("History OnCustomGesture " + gesture.RecognizedTemplate.name );
//		}
//	}

	public class MouseRecordEntry{
		public Vector3 pos;
		public MouseControl.MouseState state;
		public int index;
		public string gestureName ;
		public MyLightColor color;
		
		public MouseRecordEntry( Vector3 _pos , MouseControl.MouseState _state , int _index , string _gesName , MyLightColor _lightColor )
		{
			pos = _pos;
			state = _state;
			index = _index;
			gestureName = _gesName;
			color = _lightColor;
		}
	}
}
