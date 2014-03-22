using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawHistory : MonoBehaviour {

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
	public 

	// Use this for initialization
	void Start () {
		records = new List<List<MouseRecordEntry>> ();
		mouseEffects = new List<GameObject> ();
	}

	void Update(){
		Record ();
		Show ();
		Check();
	}

	public void OnLoopEnd()
	{

		tempRecord ++;
		showIndexs = new List<int> ();
		for (int i = 0; i < tempRecord + 1 ; ++i)
						showIndexs.Add(0);
		mouseEffects.Clear ();
		for ( int i = 0 ; i < tempRecord ; ++ i )
		{
			GameObject effect = (GameObject)Instantiate(mouseEffectPrefabs[i]);
			effect.transform.parent = this.gameObject.transform;
			mouseEffects.Add( effect );

		}
		records.Add (new List<MouseRecordEntry> ());

		Debug.Log ("History loop end ");
	}

	public void Record(){

		Vector3 pos = MouseControl.instance.pos;
		MouseControl.MouseState state = MouseControl.instance.state;
		int index = AudioManager.instance.index;
	
		records [tempRecord].Add (new MouseRecordEntry (pos, state, index));
	}
	public void Check()
	{
		if (records [tempRecord].Count <= checkStep+1 ) 
						return;

		Vector3 p1 = records [tempRecord][records [tempRecord].Count-1].pos;
		Vector3 p2 = records [tempRecord][records [tempRecord].Count-1-checkStep].pos;
		for ( int i = 0 ; i < tempRecord ; ++ i )
		{
			if ( showIndexs[i]-checkStep < 0 || records[i].Count <= showIndexs[i] )
				continue;
			Vector3 p3 = records[i][showIndexs[i]].pos;
			Vector3 p4 = records[i][showIndexs[i]-checkStep].pos;
			Vector3 cross = CheckPointCross( p1 , p2 , p3 , p4 );
			if ( cross == Vector3.zero )
				continue;
			Debug.Log("cross!!");
			GameObject effect = (GameObject)Instantiate(crossEffectPrefabs);
			effect.transform.parent = this.transform;
			effect.transform.localPosition = cross;
		}

	}
	public Vector3 CheckPointCross( Vector3 v1 , Vector3 v2 , Vector3 v3 , Vector3 v4 )
	{
		if (v1 == v2 || v3 == v4) // can not be line
						return Vector3.zero;

		if ( Larger( v1 , v2 ))
		{
			Vector3 tem = v2;
			v2 = v1;
			v1 = tem;
		}
		if ( Larger (v3, v4))
		{
			Vector3 tem = v4;
			v4 = v3;
			v3 = tem;
		}
		if (v1 == v3 || v1 == v4 || v2 == v3 || v2 == v4) //overlap
						return Vector3.zero;
		if (Vector3.Cross (v2 - v1, v4 - v3).magnitude < 1e-7f)
						return Vector3.zero;

		float d = (v2.x - v1.x) * (v4.y - v3.y) - (v4.x - v3.x) * (v2.y - v1.y);
		float b1 = (v2.y - v1.y) * v1.x + (v1.x - v2.x) / v1.y;
		float b2 = (v4.y - v3.y) * v3.x + (v3.x - v4.x) * v3.y;
		float d1 = b2 * (v2.x - v1.x) - b1 * (v4.x - v3.x);
		float d2 = b2 * (v2.y - v1.y) - b1 * (v4.y - v3.y);
		Vector3 cross = new Vector3 (d1 / d, d2 / d, AudioManager.staticZ);	

		//Debug.Log ("cross: " + cross);

		if (Vector3.Dot (v2 - v1, cross - v1) < 1e-7f
		    || Vector3.Dot (v1 - v2, cross - v2) < 1e-7f
		    || Vector3.Dot (v3 - v4, cross - v4) < 1e-7f
		    || Vector3.Dot (v4 - v3, cross - v3) < 1e-7f )
						return Vector3.zero;
		return cross;

	}
	public bool Larger( Vector3 v1 , Vector3 v2 )
	{
		return (v1.x > v2.x || ((v1.x - v2.x) < 1e-7f && v1.y > v2.y));
	}
	public void Show()
	{
		for (int i = 0 ; i < tempRecord ; ++ i) 
		{
			Show (i);
		}
	}

	public void Show( int i )
	{
		Debug.Log ("show " + i);
		int j = showIndexs [i];
		int tempAudioIndex = AudioManager.instance.index;
		if (j >= records [i].Count - 1 )
						j = records [i].Count - 1;

		GUIDebug.add (ShowType.label, "show " + i + " index " + records [i] [j].index);
		while ( j < records[i].Count && tempAudioIndex > records[i][j].index )
		{
			j++;
		}
		showIndexs [i] = j;

		if ( mouseEffects [i] == null )
		{
			return ;
		}
		mouseEffects [i].SetActive (true);
		//Debug.Log ("record " + i + j + " " + records [i] [j].pos);
		if (i >= mouseEffects.Count)
						Debug.Log (" error 1 ");
		if (i >= records.Count)
						Debug.Log ("error 2 ");
		if (j >= records [i].Count)
						Debug.Log ("error 3 ");
		mouseEffects [i].transform.localPosition = records [i] [j].pos;

		switch ( records[i][j].state )
		{
		case MouseControl.MouseState.Drag:
			mouseEffects[i].SetActive(true);
			break;
		case MouseControl.MouseState.Point:
			mouseEffects[i].SetActive(true);
			break;
		case MouseControl.MouseState.Free:
			mouseEffects[i].SetActive(false);
			break;
		}
	}

	public class MouseRecordEntry{
		public Vector3 pos;
		public MouseControl.MouseState state;
		public int index;
		public MouseRecordEntry( Vector3 _pos , MouseControl.MouseState _state , int _index  )
		{
			pos = _pos;
			state = _state;
			index = _index;
		}
	}
}
