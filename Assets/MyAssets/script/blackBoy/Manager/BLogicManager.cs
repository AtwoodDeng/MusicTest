using UnityEngine;
using System.Collections;
using System;

public class BLogicManager : MonoBehaviour {

	public BLogicManager() { s_Instance = this; }
	public static BLogicManager Instance { get { return s_Instance; } }
	private static BLogicManager s_Instance;

	void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnMouseClick ,OnMouseClick );
	}
	
	void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMouseClick, OnMouseClick);
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



}
