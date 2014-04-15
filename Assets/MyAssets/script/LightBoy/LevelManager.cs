using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour {

	static public LevelManager instance;

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

	//level
	public int tempLevel;
	public GameObject levelObj;
	BoyLevel levelCom;


	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		StartLevel (tempLevel);
	}

	void RestartLevel()
	{
		startTime = System.DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {
		tempTime = System.DateTime.Now;
//		if ((tempTime - startTime).TotalMilliseconds > 50000)
//		{
//			RestartLevel ();
//			SendMessage("OnLoopEnd" , SendMessageOptions.DontRequireReceiver );
//		}
	}

	public int getIndex(){
		tempTime = System.DateTime.Now;
		//Debug.Log ("Time " + (tempTime - startTime).TotalMilliseconds);
		return Convert.ToInt32( (tempTime - startTime).TotalMilliseconds);
	}

	public void StartLevel( int i )
	{
		RestartLevel ();

		GameObject levelPrefab = (GameObject)Resources.Load ("Level/Level" + i.ToString ());
		if ( levelPrefab == null )
		{
			Debug.Log("Cannot find Level " + i.ToString());
			return;
		}
		levelObj = (GameObject)Instantiate (levelPrefab);
		levelObj.transform.parent = transform;
		levelObj.transform.localPosition = Vector3.zero;
		levelCom = levelObj.GetComponent<BoyLevel> ();
	}

	public void EndLevel()
	{
		if (levelObj != null) {
			levelObj.GetComponent<BoyLevel>().FadeOut();
			levelObj = null;
		}
		BoyManager.instance.Clear ();
	}

	public void EnterCastle()
	{
		//Debug.Log ("Enter Castle");
		SendMessage("OnLoopEnd" , SendMessageOptions.DontRequireReceiver );
		tempLevel++;
		EndLevel ();
		StartLevel (tempLevel);
	}

	public void EndPaperLevel()
	{
		if (levelObj != null) {
			levelObj.GetComponent<BoyLevel>().FadeOut();
			levelObj = null;
		}
	}

	public void EnterFinnal()
	{
		SendMessage("OnLoopEnd" , SendMessageOptions.DontRequireReceiver );
		tempLevel++;
		EndPaperLevel ();
		StartLevel (tempLevel);
	}

	public Vector3 startPosition()
	{
		Vector3 res = levelCom.start.transform.position;
		res.z = AudioManager.staticZ;
		return res;
	}
}
