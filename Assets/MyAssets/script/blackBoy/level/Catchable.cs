using UnityEngine;
using System.Collections;
using System;

public class Catchable : MonoBehaviour {

	private int ID = -1;
	protected string HandID;

	public enum ForceType
	{
		Out,
		In,
	}
	public ForceType forceType;

	// Use this for initialization

	void Start()
	{
		ID = Global.getID();
	}

	public int getID(){
		if ( ID == -1 )
			ID = Global.getID();
		return ID;
	}

	void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnCatch, OnCatch);
		BEventManager.Instance.RegisterEvent (EventDefine.OnShrink, OnShrink);
	}
	
	void OnDisable() { 
		BEventManager.Instance.UnregisterEvent (EventDefine.OnCatch, OnCatch);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShrink, OnShrink);
	}
	
	
	public void OnCatch(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( !msg.ContainMessage("CatchableID"))
			return;
		int MsgID = Convert.ToInt32( msg.GetMessage("CatchableID"));
		if ( MsgID == getID())
		{
			HandID = msg.GetMessage("HandID");
			DealCatch(msg);
		}
	}
	
	public void OnShrink(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( HandID != null && HandID.Equals( msg.GetMessage("HandID") ))
			DealShrink(msg);
	}

	virtual public void  DealCatch(MessageEventArgs msg){}
	virtual public void  DealShrink(MessageEventArgs msg){}
}
