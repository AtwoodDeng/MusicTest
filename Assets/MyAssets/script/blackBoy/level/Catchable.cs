using UnityEngine;
using System.Collections;
using System;

public class Catchable : MonoBehaviour {

	private int ID = -1;
	protected string HandID;
	public float forceIntense = 1f;
	public bool isAttract = false;

	public enum ForceType
	{
		Out,
		In,
	}
	public ForceType forceType = ForceType.In;

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

	virtual public Vector3 getForceMain( Vector3 toBody )
	{

		return forceIntense * Vector3.Cross( toBody.normalized , Vector3.back ); // / toBody.magnitude;
	}

	virtual public void  DealCatch(MessageEventArgs msg){}
	virtual public void  DealShrink(MessageEventArgs msg){}

	virtual public HeroHand.ForceType getForceType()
	{
		return HeroHand.ForceType.None;
	}
}
