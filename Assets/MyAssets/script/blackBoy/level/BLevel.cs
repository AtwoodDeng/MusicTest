using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BLevel : MonoBehaviour {
	public string levelName;
	public GameObject startPoint;
	public GameObject RecoverPoint;

	public float time = 0;

	void Awake()
	{
		GameObject beginPre = Resources.Load( Global.BeginPointEffect ) as GameObject;
		GameObject begin = Instantiate( beginPre ) as GameObject;
		begin.transform.parent = BObjManager.Instance.Effect.transform;
	}

	virtual public void showNextDialogGroup()
	{
		List<string> texts = BDataManager.Instance.getNextDialogGroup( levelName );
		if ( texts == null || texts.Count <= 0 )
			return;
		for ( int i = 0 ; i < texts.Count ; ++i )
		{
			showText( texts[i] , true , Global.TextShowTime , Global.TextDisappearTime );
		}
	}


	virtual public void showNextDialog( int n )
	{
		for ( int i = 0 ;i < n ; ++ i )
		{
			showNextDialog();
		}
	}


	virtual public void showNextDialog(){
		Debug.Log("BLevel showNextDialog");
		MessageEventArgs msg = new MessageEventArgs();
		string text = BDataManager.Instance.getNextDialog( levelName  );
		msg.AddMessage( "text" , text );
		msg.AddMessage( "buffer" , "1" );
		BEventManager.Instance.PostEvent( EventDefine.OnShowText , msg );
	}

	virtual public void showText( string text , bool isBuffer = false , float showTime =2f , float disTime = 3f ){
		MessageEventArgs msg = new MessageEventArgs();
		msg.AddMessage( "text" , text );
		msg.AddMessage("showTime" , showTime.ToString());
		msg.AddMessage("disappearTime" , disTime.ToString());
		if ( isBuffer )
			msg.AddMessage("buffer" , "1");

		BEventManager.Instance.PostEvent( EventDefine.OnShowText , msg );
	}
	
	virtual public void showTipsByKey( string key )
	{
		List<string> tips = BDataManager.Instance.getDialogsWithKey( levelName , key );
		for ( int i = 0 ; i < tips.Count ; ++ i )
			showTips( tips[i] );

	}
	virtual public void showTips( string text , float showTime =3f , float disTime = 4f ){
		MessageEventArgs msg = new MessageEventArgs();
		msg.AddMessage( "text" , text );
		msg.AddMessage("showTime" , Global.TipsShowTime.ToString() );
		msg.AddMessage("disappearTime" , Global.TipsDisappearTime.ToString());
		
		BEventManager.Instance.PostEvent( EventDefine.OnShowTips , msg );
	}

	protected void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnTriggerable ,OnTriggerable );
		BEventManager.Instance.RegisterEvent (EventDefine.OnRestart ,OnRestart );
		BEventManager.Instance.RegisterEvent (EventDefine.OnDead ,OnDead );
		
	}

	protected void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnTriggerable, OnTriggerable);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnRestart ,OnRestart );
		BEventManager.Instance.UnregisterEvent (EventDefine.OnDead ,OnDead );
	}



	public void OnTriggerable(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		DealTrigger( msg.GetMessage(Global.TriggableMessage));
	}

	virtual public void DealTrigger( string msg )
	{
		if ( Global.EndPointMessage.Equals( msg ))
		{
			OnEnd(Global.EndLevelTime);
		}
		if ( Global.RECOVER_MSG.Equals( msg.Substring(0, Global.RECOVER_MSG.Length )) )
		{
			if ( RecoverPoint == null || RecoverPoint.name != msg )
			{
				foreach( GameObject rp in BObjManager.Instance.RecoverPoints )
				{
					if ( rp.name == msg )
					{
						RecoverPoint = rp;
					}
				}
			}
		}
	}

	virtual public void DealWith( float deltaTime )
	{
		time += deltaTime;
	}

	public void OnRestart(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		Restart( msg.GetMessage("msg"));
		time = 0;
	}

	virtual public void Restart( string msg )
	{
		if ( startPoint != null )
		{
			HeroBody body = BObjManager.Instance.BHeroBody;
			body.transform.position = startPoint.transform.position;
			Vector3 velocity = body.rigidbody.velocity;
			velocity.x = 0;
			velocity.y = 0;
			body.rigidbody.velocity = velocity;
			body.Restart();
		}
	}

	public void OnEnd( float endTime )
	{
		GameObject endPre = Resources.Load( Global.EndPointEffect ) as GameObject;
		GameObject end = Instantiate( endPre ) as GameObject;
		end.transform.parent = BObjManager.Instance.Effect.transform;

		Invoke( "OnEndFinal" , endTime );
	}

	public void OnEndFinal()
	{
		Application.LoadLevel( Global.nextLevelDict[levelName] );
	}

	void Update()
	{
		DealWith( Time.deltaTime );
	}

	public void OnDead(EventDefine eventName, object sender, EventArgs args )
	{
		//effect
		GameObject deadTurnBlackPre = Resources.Load( Global.DeadTurnBlackEffect ) as GameObject;
		GameObject deadTurnBlack = Instantiate( deadTurnBlackPre ) as GameObject;
		deadTurnBlack.transform.parent = BObjManager.Instance.Effect.transform;

		//invoke reset position
		SpriteColorChange[] scChanges = deadTurnBlack.GetComponentsInChildren<SpriteColorChange>();
		float maxFadeTime = 0f;
		foreach( SpriteColorChange scc in scChanges )
		{
			if ( maxFadeTime < scc.fadeTime + scc.delay )
				maxFadeTime = scc.fadeTime + scc.delay;
		}

		Invoke( "OnDeadResetPosition" , maxFadeTime );

		//shrink
		MessageEventArgs msg = new MessageEventArgs();
		msg.AddMessage( "both" , "1" );
		BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , msg );
	}
	
	public void OnDeadResetPosition()
	{
		//effect
		GameObject deadAppearPre = Resources.Load( Global.DeadAppearEffect ) as GameObject;
		GameObject deadAppear = Instantiate( deadAppearPre ) as GameObject;
		deadAppear.transform.parent = BObjManager.Instance.Effect.transform;

		//reset position
		if ( RecoverPoint != null )
		{
			BObjManager.Instance.BHeroBody.transform.position = RecoverPoint.transform.position;
			BObjManager.Instance.BHeroBody.Heal();
		}else
		{
			BObjManager.Instance.BHeroBody.transform.position = startPoint.transform.position;
			BObjManager.Instance.BHeroBody.Heal();
		}
	}
}
