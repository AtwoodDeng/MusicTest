using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BLevel : MonoBehaviour {
	public string levelName;

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
		
	}

	protected void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnTriggerable, OnTriggerable);
	}

	public void OnTriggerable(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		DealTrigger( msg.GetMessage("msg"));
	}

	virtual public void DealTrigger( string msg )
	{
	}

	virtual public void DealWith( float deltaTime )
	{
	}

	void Update()
	{
		DealWith( Time.deltaTime );
	}
}
