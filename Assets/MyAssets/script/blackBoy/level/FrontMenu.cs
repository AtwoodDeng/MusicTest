using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class FrontMenu : MonoBehaviour {

	public bool isNextLevel;
	public string levelName;

	public float duration;

	string nextSence;
	public tk2dSprite black;
	public GameObject cursor;

	public void Awake()
	{
		GameObject cursorPrefab = Resources.Load( Global.CursorDict["Free"]) as GameObject;
		cursor = Instantiate( cursorPrefab ) as GameObject;
		cursor.transform.parent = this.transform;
		cursor.layer = 8;
		cursor.transform.localScale = Global.FrontCursorSize;
	}

	public void OnBack()
	{
		Debug.Log( "OnBack" );

		this.gameObject.SetActive( false );
		BEventManager.Instance.PostEvent(EventDefine.OnBack , new MessageEventArgs());

	}

	public void OnNextLevel()
	{
		ChangeSence ( Global.nextLevelDict[levelName] );

	}

	public void OnMainMenu()
	{
		ChangeSence( "main" );
	}

	public void ChangeSence(string ns )
	{
		nextSence = ns;

		Color toColor = black.color;
		toColor.a = 1.0f;

		HOTween.To( black  ,
		           duration ,
		           "color" ,
		           toColor ,
		           false ,
		           EaseType.Linear ,
		           0f );

		Invoke( "changeTo" , duration );
	}

	public void OnRestart( )
	{
		BEventManager.Instance.PostEvent( EventDefine.OnRestart , new MessageEventArgs() ) ;

		OnBack();
	}

	void changeTo()
	{
		Application.LoadLevel( nextSence );

	}

	void Update()
	{
		DealCursor();
	}

	void DealCursor()
	{
		Camera viewCamera = GetComponent<Camera>();

		Vector3 mousePos;
		mousePos = Input.mousePosition;
		mousePos.x -= Screen.width / 2;
		mousePos.y -= Screen.height / 2 ;
		mousePos.x *= viewCamera.orthographicSize / ( Screen.height / 2 ) ;
		mousePos.y *= viewCamera.orthographicSize / ( Screen.height / 2 ) ;
		mousePos += viewCamera.transform.position;
		mousePos.z = Global.BstaticPosition.z;

		cursor.transform.position = mousePos;
	}

}
