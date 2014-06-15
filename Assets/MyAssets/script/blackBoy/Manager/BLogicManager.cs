using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Holoville.HOTween;

public class BLogicManager : MonoBehaviour {

	public BLogicManager() { s_Instance = this; }
	public static BLogicManager Instance { get { return s_Instance; } }
	private static BLogicManager s_Instance;


	List<TextContent> texts = new List<TextContent>();

	public class TextContent{
		public string text;
		public float showTime;
		public float disappearTime;
		public TextContent( string _text , float _showTime , float _disapperTime )
		{
			text = _text;
			showTime = _showTime;
			disappearTime = _disapperTime;
		}
	}

	void Awake()
	{
//		InvokeRepeating( "test" , 2f , 3f );

//	    string script1 = BDataManager.Instance.getNextDialog("Level1","ENGLISH");
//		MessageEventArgs msg = new MessageEventArgs();
//		msg.AddMessage("text" , script1 );
//		BEventManager.Instance.PostEvent(EventDefine.OnShowText , msg );
	}

	void test()
	{
		ShowTips( "test  test  \n test test hahaha" );
	}

	void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnMouseClick ,OnMouseClick );
		BEventManager.Instance.RegisterEvent (EventDefine.OnShowText ,OnShowText );
		BEventManager.Instance.RegisterEvent (EventDefine.OnShowTips ,OnShowTips );

	}
	
	void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMouseClick, OnMouseClick);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShowText ,OnShowText );
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShowTips ,OnShowTips );
	}

	public void OnShowTips(EventDefine eventName, object sender, EventArgs args)
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
		ShowTips( text , showTime , disappearTime );
		
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
		if ( msg.ContainMessage("buffer") )
			texts.Add( new TextContent( text , showTime , disappearTime ));
		else 
			ShowText( text , showTime , disappearTime );
		
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
		Vector3 initpos = BObjManager.Instance.Hero.transform.position;
		Debug.Log( "NumDrawnChara" + textMesh.NumDrawnCharacters() );
		Debug.Log( "invOrthoSize" + textMesh.font.largestWidth );
		Debug.Log( "textMesh" + textMesh.scale.x );
		initpos.x -= ( textMesh.NumDrawnCharacters() / 2 * textMesh.font.largestWidth * textMesh.scale.x) / ( 1 -  Global.TextRelativelyRate) / 2f;
		initpos.y += UnityEngine.Random.Range( - Global.TextShowYRan , + Global.TextShowYRan );
		textObj.transform.position = initpos;
		BFollowWith follow =  textObj.AddComponent<BFollowWith>();
		follow.followState = BFollowWith.FollowState.HalfRelatively;
		follow.RelativelyRate = Global.TextRelativelyRate;
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

	public void ShowTips( string text , float showTime = 2f , float disappearTime = 3f)
	{
		Debug.Log("ShowTips");
		//create text object
		GameObject tipsPrefab = Resources.Load(Global.TipsPrefabPath) as GameObject;
		GameObject tipsObj = Instantiate( tipsPrefab ) as GameObject;
		tipsObj.transform.parent = BObjManager.Instance.Effect.transform;
		tipsObj.transform.localPosition = Vector3.zero;
		tipsObj.transform.localScale = Vector3.one;
		
		//set the message
		tk2dTextMesh textMesh = tipsObj.GetComponent<tk2dTextMesh>();
		if ( textMesh == null )
		{
			Debug.Log("Cannot find TextMesh");
			return;
		}
		textMesh.text = text;
		
		
		//set the follow 
		Vector3 initpos = BObjManager.Instance.Hero.transform.position;
		Debug.Log( "NumDrawnChara" + textMesh.NumDrawnCharacters() );
		Debug.Log( "invOrthoSize" + textMesh.font.largestWidth );
		Debug.Log( "textMesh" + textMesh.scale.x );
		initpos.x -= textMesh.NumDrawnCharacters() / 2 * textMesh.font.largestWidth * textMesh.scale.x / 2;
		tipsObj.transform.position = initpos + Global.BTipsPosition;
		BFollowWith follow =  tipsObj.AddComponent<BFollowWith>();
		follow.followState = BFollowWith.FollowState.Relatively;
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
		AutoDestory destory = tipsObj.AddComponent<AutoDestory>();
		destory.destroyTime = showTime + disappearTime;
		destory.StartAutoDestory();
	}


	
	void Update()
	{
		DealDialogs(Time.deltaTime);
	}


	float showDialogCounter = 0;
	void DealDialogs( float deltaTime )
	{
		showDialogCounter -= deltaTime;
		if ( texts.Count > 0 && showDialogCounter < 0 )
		{
			TextContent tc = texts[0]; 
			texts.RemoveAt( 0 );
			showDialogCounter = tc.showTime + tc.disappearTime / 3 ;
			ShowText( tc.text , tc.showTime , tc.disappearTime );
		}
	}

}
