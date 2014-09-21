using UnityEngine;
using System.Collections;
using System;

public class BlackWhiteRock : Catchable {

	public enum SpinType
	{
		Black,
		White,
		Clockwise,
		AntiClockwise,
	}

	public SpinType spinType;
	public bool ifEnableWhenHeroLeave = false;

	void Awake()
	{
		gameObject.tag = "ROCK";
		if ( ifEnableWhenHeroLeave )
			collider.isTrigger = true;
	}

	public override Vector3 getForceMain (Vector3 toBody)
	{
		switch ( spinType )
		{
		case SpinType.Black:
			return forceIntense * Vector3.Cross( toBody.normalized , Vector3.back ) / toBody.magnitude;
			break;
		case SpinType.White:
			return forceIntense * Vector3.Cross( toBody.normalized , Vector3.forward ) / toBody.magnitude;
			break;
		case SpinType.AntiClockwise:
			return forceIntense * Vector3.Cross( toBody.normalized , Vector3.back ) / toBody.magnitude;
			break;
		case SpinType.Clockwise:
			return forceIntense * Vector3.Cross( toBody.normalized , Vector3.forward ) / toBody.magnitude;
			break;
		}
		return Vector3.zero;
	}

	public override HeroHand.ForceType getForceType ()
	{
		switch ( spinType )
		{
		case SpinType.AntiClockwise:
			return HeroHand.ForceType.SpinAntiCW;
			break;
		case SpinType.Black:
			return HeroHand.ForceType.SpinAntiCW;
			break;
		case SpinType.Clockwise:
			return HeroHand.ForceType.SpinCW;
			break;
		case SpinType.White:
			return HeroHand.ForceType.SpinCW;
			break;
		}
		return base.getForceType();
	
	}

	public void OnTriggerExit( Collider col )
	{
		if ( ifEnableWhenHeroLeave && col.tag == Global.HeroTag )
		{
			collider.isTrigger = false;
		}
	}

	public override void DealCatch( MessageEventArgs msg )
	{
		msg.AddMessageReplace("type" , spinType.ToString() );
		BEventManager.Instance.PostEvent( EventDefine.OnAfterCatch , msg );
	}
}
