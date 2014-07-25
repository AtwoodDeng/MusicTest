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
	List<TextContent> tips = new List<TextContent>();

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

	public bool heroEnableMove = true;

	private FrontMenu frontMenu;

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
		BEventManager.Instance.RegisterEvent (EventDefine.OnFreezen ,OnFreezen );
		BEventManager.Instance.RegisterEvent (EventDefine.OnUnfreezen ,OnUnfreezen );
		BEventManager.Instance.RegisterEvent (EventDefine.OnFrontMenu ,OnFrontMenu );
		BEventManager.Instance.RegisterEvent (EventDefine.OnStop ,OnStop );
		BEventManager.Instance.RegisterEvent (EventDefine.OnBack ,OnBack );

	}
	
	void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMouseClick, OnMouseClick);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShowText ,OnShowText );
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShowTips ,OnShowTips );
		BEventManager.Instance.UnregisterEvent (EventDefine.OnFreezen ,OnFreezen );
		BEventManager.Instance.UnregisterEvent (EventDefine.OnUnfreezen ,OnUnfreezen );
		BEventManager.Instance.UnregisterEvent (EventDefine.OnFrontMenu ,OnFrontMenu );
		BEventManager.Instance.UnregisterEvent (EventDefine.OnStop ,OnStop );
		BEventManager.Instance.UnregisterEvent (EventDefine.OnBack ,OnBack );
	}

	public void OnStop(EventDefine eventName, object sender, EventArgs args)
	{
		heroEnableMove = false;
	}

	public void OnBack(EventDefine eventName, object sender, EventArgs args)
	{
		heroEnableMove = true;
	}

	public void OnFrontMenu(EventDefine eventName, object sender, EventArgs args)
	{
		if ( frontMenu != null )
		{
			frontMenu.gameObject.SetActive( true );
		}else{
			GameObject frontMenuPrefab = Resources.Load( Global.FrontMenuPath ) as GameObject;
			GameObject frontMenuObj = Instantiate( frontMenuPrefab ) as GameObject;
			frontMenuObj.name = Global.FrontMenuName;
			frontMenuObj.tag = Global.FrontMenuTag;
			frontMenu = frontMenuObj.GetComponent<FrontMenu>();
		}

		BEventManager.Instance.PostEvent( EventDefine.OnStop , new MessageEventArgs() );
	}

	public void OnFreezen(EventDefine eventName, object sender, EventArgs args)
	{
		//shrink the body
		MessageEventArgs msg = (MessageEventArgs)args;
		if (  msg.ContainMessage( "ifShrink" ) && msg.GetMessage( "ifShrink" ).Equals( Boolean.TrueString ))
		{
			MessageEventArgs msg1 = new MessageEventArgs();
			msg1.AddMessage("isLeft" , Boolean.TrueString );
			BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , msg1 );
			MessageEventArgs msg2 = new MessageEventArgs();
			msg2.AddMessage("isLeft" , Boolean.FalseString );
			BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , msg2 );
		}

		if ( msg.ContainMessage( "time" ))
		{
			float time = float.Parse( msg.GetMessage( "time" ));
			Invoke( "Unfreezen" , time );
		}
		//unable to move 
		heroEnableMove = false;
	}

	public void Unfreezen()
	{
		BEventManager.Instance.PostEvent( EventDefine.OnUnfreezen , new EventArgs()) ;

	}
	
	public void OnUnfreezen(EventDefine eventName, object sender, EventArgs args)
	{
		heroEnableMove = true;
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
		tips.Add( new TextContent( text , showTime , disappearTime ));
		
	}

	public void OnShowText(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		string text = "";
		float showTime = Global.TextShowTime;
		float disappearTime = Global.TextDisappearTime;
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

		if ( heroEnableMove )
		{
			if ( Global.MouseLeft.Equals( msg.GetMessage("type")))
			{
				msg.AddMessage("isLeft", Boolean.TrueString );
				BEventManager.Instance.PostEvent( EventDefine.OnMoveHand , args );
			}
			
			if ( Global.MouseRight.Equals( msg.GetMessage("type")))
			{
	//			msg.AddMessage("isLeft", Boolean.FalseString );
	//			BEventManager.Instance.PostEvent( EventDefine.OnMoveHand , args );
				msg.AddMessage("isLeft" , Boolean.FalseString );
				BEventManager.Instance.PostEvent( EventDefine.OnChangeForce , args );
			}
		}
	}

	public void ShowText( string text , float showTime = 3f , float disappearTime = 2f,  bool freezenByText = true )
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
		Vector3 initpos = BObjManager.Instance.Hero.transform.position ;
//		Debug.Log( "NumDrawnChara" + textMesh.NumDrawnCharacters() );
//		Debug.Log( "invOrthoSize" + textMesh.font.largestWidth );
//		Debug.Log( "textMesh" + textMesh.scale.x );
		//initpos.x -= ( textMesh.NumDrawnCharacters() / 2 * textMesh.font.largestWidth * textMesh.scale.x) / ( 1 -  Global.TextFontRate) / 2f;
		initpos.y += UnityEngine.Random.Range( - Global.TextShowYRan , + Global.TextShowYRan );
		textObj.transform.position = initpos + Global.BTextPosition ;
		BFollowWith follow =  textObj.AddComponent<BFollowWith>();
		follow.followState = BFollowWith.FollowState.HalfRelatively;
		follow.RelativelyRate = Global.TextRelativelyRate;
		follow.UpdateTarget( BObjManager.Instance.Hero );

		//set the effect
		if ( isTheTailText() )
			disappearTime *= 3f;
		Color toCol = textMesh.color;
		toCol.a = 0;
		HOTween.To( textMesh 
		           , disappearTime
		           , "color"
		           , toCol
		           , false
		           , EaseType.Linear
		           , showTime );

		//background
		tk2dSprite[] backs = textObj.GetComponentsInChildren<tk2dSprite>();
		Debug.Log("In logic manager" + " get " + backs.Length + "sprites " );
		foreach( tk2dSprite back in backs )
		{
			Color backCol = back.color;
			backCol.a = 0;
			HOTween.To( back
			           , disappearTime
			           , "color"
			           , backCol
			           , false
			           , EaseType.Linear
			           , showTime );
		}


		//auto destory
		AutoDestory destory = textObj.AddComponent<AutoDestory>();
		destory.destroyTime = showTime + disappearTime;
		destory.StartAutoDestory();

		//freezen the hero
		MessageEventArgs msg = new MessageEventArgs();
		msg.AddMessage( "isShrink", Boolean.TrueString );
		BEventManager.Instance.PostEvent( EventDefine.OnFreezen , new MessageEventArgs());
		Debug.Log("Freezen");
		//call unfreezen
		if ( !freezenByText )
		{
			float unfreezenTime = showTime + disappearTime + Time.deltaTime;
			Invoke( "ShowTextEnd" , unfreezenTime);
		}else if ( isTheTailText() )
		{
			float unfreezenTime = showTime + Time.deltaTime;
			Invoke( "ShowTextEnd" , unfreezenTime);

		}
	}

	public bool isTheTailText()
	{
		return texts.Count <= 0 ;
	}

	public void ShowTextEnd(  )
	{
		BEventManager.Instance.PostEvent( EventDefine.OnUnfreezen , new EventArgs()) ;
		Debug.Log("Unfreezen");
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

		//freezen
		BEventManager.Instance.PostEvent( EventDefine.OnFreezen , new MessageEventArgs() );

		if ( tips.Count <= 0 )
			BEventManager.Instance.PostEvent( EventDefine.OnUnfreezen , new MessageEventArgs () );
	}

	void Update()
	{
		DealDialogs(Time.deltaTime);
		DealTips(Time.deltaTime);
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

	float showTipsCounter = 0;
	void DealTips( float deltaTime )
	{
		showTipsCounter -= deltaTime;
		if ( tips.Count > 0 && showTipsCounter < 0 )
		{
			TextContent tc = tips[0]; 
			tips.RemoveAt( 0 );
			showTipsCounter = tc.showTime + tc.disappearTime / 3 ;
			ShowTips( tc.text , tc.showTime , tc.disappearTime );
		}
	}

}
