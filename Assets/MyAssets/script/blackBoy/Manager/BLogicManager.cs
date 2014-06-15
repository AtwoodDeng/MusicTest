using UnityEngine;
using System.Collections;
using System;
using Holoville.HOTween;

public class BLogicManager : MonoBehaviour {

	public BLogicManager() { s_Instance = this; }
	public static BLogicManager Instance { get { return s_Instance; } }
	private static BLogicManager s_Instance;

	void Awake()
	{
		//InvokeRepeating( "test" , 2f , 3f );

	    string script1 = BDataManager.Instance.getScript("Level1");
		MessageEventArgs msg = new MessageEventArgs();
		msg.AddMessage("text" , script1 );
		BEventManager.Instance.PostEvent(EventDefine.OnShowText , msg );
	}

	void test()
	{
		ShowText( "test  test  \n test test hahaha" );
	}

	void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnMouseClick ,OnMouseClick );
		BEventManager.Instance.RegisterEvent (EventDefine.OnShowText ,OnShowText );

	}
	
	void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMouseClick, OnMouseClick);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShowText ,OnShowText );
	}

	
	public void OnShowText(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		string text = "";
		float showTime = 2f;
		float disappearTime = 3f;
		if ( msg.ContainMessage("text") )
			text = msg.GetMessage("text");
		if ( msg.ContainMessage("showTime"))
			showTime = float.Parse( msg.GetMessage("showTime"));
		if ( msg.ContainMessage("disappearTime"))
			disappearTime = float.Parse( msg.GetMessage("disappearTime"));

		ShowText( text );
	}
	
	
	public void OnMouseClick(EventDefine eventName, object sender, EventArgs args)
	{
		// Debug.Log("on mouse click");
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( Global.MouseLeft.Equals( msg.GetMessage("type")))
		{
			msg.AddMessage("isLeft", Boolean.TrueString );
			BEventManager.Instance.PostEvent( EventDefine.OnMoveHand , args );
		}
		
		if ( Global.MouseRight.Equals( msg.GetMessage("type")))
		{
			msg.AddMessage("isLeft", Boolean.FalseString );
			BEventManager.Instance.PostEvent( EventDefine.OnMoveHand , args );
		}
		
	}

	public void ShowText( string text , float showTime = 2f , float disappearTime = 3f)
	{
		//create text object
		GameObject textPrefab = Resources.Load(Global.TextPrefabPath) as GameObject;
		GameObject textObj = Instantiate( textPrefab ) as GameObject;
		textObj.transform.parent = BObjManager.Instance.Effect.transform;
		textObj.transform.localPosition = Vector3.zero;
		textObj.transform.localScale = Vector3.one;

		//set the message
		tk2dTextMesh textMesh = textObj.GetComponent<tk2dTextMesh>();
		if ( textMesh == null )
		{
			Debug.Log("Cannot find TextMesh");
			return;
		}
		textMesh.text = text;

		//set the follow 
		textObj.transform.position = BObjManager.Instance.Hero.transform.position;
		BFollowWith follow =  textObj.AddComponent<BFollowWith>();
		follow.followState = BFollowWith.FollowState.HalfRelatively;
		follow.RelativelyRate = 0.7f;
		follow.UpdateTarget( BObjManager.Instance.Hero );

		//set the effect
		Color toCol = textMesh.color;
		toCol.a = 0;
		HOTween.To( textMesh 
		           , disappearTime
		           , "color"
		           , toCol
		           , false
		           , EaseType.Linear
		           , showTime );

		//auto destory
		AutoDestory destory = textObj.AddComponent<AutoDestory>();
		destory.destroyTime = showTime + disappearTime;
		destory.StartAutoDestory();
	}



}
