using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class FrontMenu : MonoBehaviour {

	public bool isNextLevel;
	public string levelName;

	public float duration;

	string nextSence;

	public GameObject nextLevel;
	public GameObject back;
	public GameObject mainMenu;

	public tk2dSprite black;

	public void OnBack()
	{
		Debug.Log( "OnBack" );

		this.gameObject.SetActive( false );
		BEventManager.Instance.PostEvent(EventDefine.OnFrontMenuBack , new MessageEventArgs());
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

	void changeTo()
	{
		Application.LoadLevel( nextSence );

	}

}
