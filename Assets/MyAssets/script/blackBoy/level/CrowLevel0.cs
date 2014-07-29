using UnityEngine;
using System.Collections;
using System;

public class CrowLevel0 : BLevel {

	public enum State{
		Init,
		Play,
		End,
	}
	public State state = State.Init;

	//===== start point =====
//	public GameObject[] startPoint;
	public int maxHalo;

	//records
	//level 0_1
	private bool isOutOfRange = false;
	private bool isClick = false;
	private bool isWhite = false;
	private bool isBlack = false;
	private bool isMind = false;

	//all
	private int halo_count = 0 ;


	void OnEnable() {
		base.OnEnable();
		BEventManager.Instance.RegisterEvent (EventDefine.OnMouseClick ,OnMouseClick );
		BEventManager.Instance.RegisterEvent (EventDefine.OnCatch ,OnCatch );
	}
	
	void OnDisable() {
		base.OnDisable();
		BEventManager.Instance.RegisterEvent (EventDefine.OnMouseClick ,OnMouseClick );
		BEventManager.Instance.RegisterEvent (EventDefine.OnCatch ,OnCatch );
	}

	
	public void OnMouseClick(EventDefine eventName, object sender, EventArgs args)
	{
//		MessageEventArgs msg = (MessageEventArgs)args;
//		if (  Global.MouseLeft.Equals( msg.GetMessage("type") ) )
//		{
//			if ( ! isClick )
//			{
//				isClick = true;
//				showTipsByKey( "CLICK" );
//			}
//		}

	}
	public void OnCatch(EventDefine eventName, object sender, EventArgs args)
	{
//		Catchable catchable;
//		if ( ( catchable = BObjManager.Instance.BHeroBody.leftHandGroup.getCatchable() ) == null )
//		{
//			if  ( ( catchable = BObjManager.Instance.BHeroBody.rightHandGroup.getCatchable() ) == null )
//			{
//				return;
//			}
//		}
//
//		BlackWhiteRock rock = (BlackWhiteRock)catchable;
//		if ( rock != null )
//		{
//
//			if ( rock.spinType == BlackWhiteRock.SpinType.White )
//			{
//				if ( ! isWhite )
//				{
//					isWhite = true;
//					showTipsByKey( "WHITE" );
//				}
//			} else if ( rock.spinType == BlackWhiteRock.SpinType.Black )
//			{
//				if ( ! isBlack )
//				{
//					isBlack = true;
//					showTipsByKey( "BLACK" );
//				}
//			}
//		}

	}
	public override void DealWith (float deltaTime)
	{
//		switch( state )
//		{
//		case State.Init:
//			showNextDialogGroup();
//			state =  State.Play;
//			break;
//		case State.Play:
//			if ( halo_count >= maxHalo )
//			{
//				state = State.End;
//			}
//			break;
//		case State.End:
//			Application.LoadLevel("begin");
//			break;
//		}
	}

	public override void DealTrigger (string msg)
	{
		if ( "out_of_range".Equals( msg ) )
		{
//			if ( ! isOutOfRange )
//			{
//				isOutOfRange = true;
//				
//				showTipsByKey( "OUT" );
//			}
			Restart( "" );
		}
		if ( "on_end_point".Equals( msg ))
		{
			BEventManager.Instance.PostEvent( EventDefine.OnFrontMenu , new MessageEventArgs() );
		}
//		if ( "get_mind".Equals( msg ) )
//		{	
//			halo_count++;
//
//			if ( !isMind )
//			{
//				isMind = true;
//				showTipsByKey( "HALO" );
//			}else {
//				
//				string tips = BDataManager.Instance.getDialogWithKey( levelName , "MIND" );
//				tips += "(" + halo_count.ToString() + "/" + maxHalo.ToString() + ")";
//				showTips( tips , Global.TipsShowTime , Global.TipsDisappearTime );
//			}
//		}
	}

//	public override void Restart (string msg)
//	{
//		HeroBody body = BObjManager.Instance.BHeroBody;
//		body.transform.position = startPoint[1].transform.position;
//		Vector3 velocity = body.rigidbody.velocity;
//		velocity.x = 0;
//		velocity.y = 0;
//		body.rigidbody.velocity = velocity;
//		body.Restart();
//	}
}
