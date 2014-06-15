using UnityEngine;
using System.Collections;
using System;

public class BLevel : MonoBehaviour {
	public string levelName;
	
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

	virtual public void showTips( string text , float showTime =3f , float disTime = 4f ){
		MessageEventArgs msg = new MessageEventArgs();
		msg.AddMessage( "text" , text );
		msg.AddMessage("showTime" , showTime.ToString());
		msg.AddMessage("disappearTime" , disTime.ToString());
		
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
}
