using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PHistoryType
{
	Normal,
}


public class PDrawHistory : MonoBehaviour {
	
	public List<GameObject> mouseEffectPrefabs;
	public GameObject crossEffectPrefabs;
	
	//record 
	public List<List<MouseRecordEntry>> records;
	
	int tempRecord = -1;
	
	//show
	public List<int> showIndexs;
	public List<GameObject> mouseEffects;
	
	//cross
	public int checkStep = 5;
	
	//type to record history
	public PHistoryType type = PHistoryType.Normal;
	
	// Use this for initialization
	void Start () {
		records = new List<List<MouseRecordEntry>> ();
		mouseEffects = new List<GameObject> ();
		OnLoopEnd ();
	}
	
	void Update(){
		Record ();
		Show ();
	}
	
	public void OnLoopEnd()
	{
		
		tempRecord ++;
		showIndexs = new List<int> ();
		for (int i = 0; i < tempRecord + 1 ; ++i)
			showIndexs.Add(0);
		foreach ( GameObject effect in mouseEffects )
		{
			if ( effect !=null && effect.GetComponent<Mouse>()!=null )
				effect.GetComponent<Mouse>().Destory();
		}
		for ( int i = 0 ; i < tempRecord ; ++ i )
		{
			GameObject effect = (GameObject)Instantiate(mouseEffectPrefabs[i]);
			effect.transform.parent = this.gameObject.transform;
			effect.transform.localPosition = Vector3.zero;
			mouseEffects.Add( effect );
			
		}
		records.Add (new List<MouseRecordEntry> ());
		
		Debug.Log ("History loop end ");
	}
	
	public void Record(){

		//mouse
		Vector3 pos = MouseControl.instance.pos;
		MouseControl.MouseState state = MouseControl.instance.state;
		int index = PLevelManager.instance.getIndex ();

//		if ( type == PHistoryType.Music )
//			index = AudioManager.instance.index;
//		else if ( type == PHistoryType.Normal )
//			index = LevelManager.instance.getIndex();
//		
//		if (tempRecord < 0) {
//			
//		} else if (tempRecord >= records.Count) {
//			//Debug.LogError ("temp record > records  count " + tempRecord + " " + records.Count);
//			
//		} else {
//			if (type == PHistoryType.Music) {
//				if ((index - tempGestureIndex) > gestureRecordTime * AudioManager.instance.clip_freq) {
//					tempGestureName = null;
//				}
//			}
//			//Debug.Log( "record " + tempRecord + " " + pos + " " + state + " " + index );
//			records [tempRecord].Add (new MouseRecordEntry (pos, state, index, tempGestureName));
//		}

		//
	}

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
		int j = showIndexs [i];
		//check the world's index( the index of the music)
		int tempIndex = 0;
//		if ( type == HistoryType.Music )
//			tempIndex = AudioManager.instance.index;
//		else if ( type == HistoryType.Level )
//			tempIndex = LevelManager.instance.getIndex();
		
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
			if ( mouseEffects[i] != null && mouseEffects[i].GetComponent<Mouse>() != null )
				mouseEffects[i].GetComponent<Mouse>().Destory();
			mouseEffects[i] = null;
			return;
		}
		
		//Debug.Log ("show " + i + "index " + j + "state " + records [i] [j].state + " pos " + records [i] [j].pos);
		
		//check if there are mouseEffect to show this record
		switch ( records[i][j].state )
		{
		case MouseControl.MouseState.Drag:
			if ( mouseEffects[i] == null )
			{	
				mouseEffects[i] = (GameObject)Instantiate(mouseEffectPrefabs[i]);
				mouseEffects[i].transform.parent = this.gameObject.transform;
				mouseEffects[i].transform.localPosition = Vector3.zero;
			}
			break;
		case MouseControl.MouseState.Point:
			if ( mouseEffects[i] == null )
			{	
				mouseEffects[i] = (GameObject)Instantiate(mouseEffectPrefabs[i]);
				mouseEffects[i].transform.parent = this.gameObject.transform;
				mouseEffects[i].transform.localPosition = Vector3.zero;
				
			}
			break;
		case MouseControl.MouseState.Free:
			if ( mouseEffects[i] != null )
			{
				if ( mouseEffects[i].GetComponent<Mouse>() )
					mouseEffects[i].GetComponent<Mouse>().Destory();
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
		if (i >= mouseEffects.Count)
			Debug.Log (" error 1 ");
		if (i >= records.Count)
			Debug.Log ("error 2 ");
		if (j >= records [i].Count)
			Debug.Log ("error 3 ");
		/////
		
		//set the mouse effect to the recorded position
		
		if ( MouseControl.instance.followMethod == MouseControl.MouseFollowMethod.Immediately)
		{
			if (mouseEffects [i])
				mouseEffects [i].transform.localPosition = records [i] [j].pos;
		}else if( MouseControl.instance.followMethod == MouseControl.MouseFollowMethod.Delay )
		{
			if ( mouseEffects [i].transform.localPosition == Vector3.zero )
			{
				mouseEffects [i].transform.localPosition = records [i] [j].pos;
			}else
			{
				mouseEffects [i].transform.localPosition = 
					MouseControl.instance.delayIntense * mouseEffects [i].transform.localPosition
						+ ( 1 - MouseControl.instance.delayIntense ) * records [i] [j].pos;
			}
			
		}else if ( MouseControl.instance.followMethod == MouseControl.MouseFollowMethod.LimitSpeed )
		{
			Vector3 toward = - mouseEffects[i].transform.localPosition + records [i] [j].pos;
			if ( toward.magnitude > MouseControl.instance.limitSpeed )
				toward = toward.normalized * MouseControl.instance.limitSpeed ;
			mouseEffects[i].transform.localPosition =
				mouseEffects[i].transform.localPosition + toward;
		}
		
	}
	
	public class MouseRecordEntry{
		public Vector3 pos;
		public MouseControl.MouseState state;
		public int index;
		public string gestureName ;
		
		public MouseRecordEntry( Vector3 _pos , MouseControl.MouseState _state , int _index , string _gesName )
		{
			pos = _pos;
			state = _state;
			index = _index;
			gestureName = _gesName;
		}
	}

	public class MouseRecordGroup{
		List<MouseRecordEntry> data;
		int level;

	}
}
