using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CrowLevel6 : BLevel {

//	public Shine NoClick;
//	public Shine NoClick2;
//	public Shine NoSmoke;
//	public Shine NoSmoke2;
//	public Shine Woman_Man;
//	public Shine Exit;
//	public Shine NoRightButton;

	public tk2dSpriteAnimator ArrowEnd;
	public tk2dSpriteAnimator leftMouse;
	public tk2dSpriteAnimator rightMouse;
	public Triggerable endBarrier;


	public override void DealTrigger (string msg)
	{
		base.DealTrigger( msg );
		if ( "out_of_range".Equals(msg))
		{
			Restart(""); 
		}
		if ( "enter2".Equals( msg ))
		{
			rightMouse.Play();
		}
		if ( "enter_end".Equals( msg ))
		{
			OnEnd( );
		}

		//		if ( "enter_no_click".Equals( msg ))
//		{
//			NoClick.DoShine();
//			NoClick2.DoShine();
//		}
//		if ( "enter_no_smoke".Equals( msg ))
//		{
//			NoSmoke.DoShine();
//			NoSmoke2.DoShine();
//		}
//		if ( "enter_woman_man".Equals( msg ))
//		{
//			Woman_Man.DoShine();
//		}
//		if ( "enter_no_right".Equals( msg ))
//		{
//			NoRightButton.DoShine();
//		}
//		if ( "enter_exit".Equals( msg ))
//		{
//			Exit.DoShine();
//		}
	}

	protected void OnEnable() {
		base.OnEnable();
		BEventManager.Instance.RegisterEvent (EventDefine.OnMouseClick ,OnMouseClick );
		BEventManager.Instance.RegisterEvent (EventDefine.OnCatch , OnCatch );
	}

	protected void OnDisable() {
		base.OnDisable();
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMouseClick ,OnMouseClick );
		BEventManager.Instance.UnregisterEvent (EventDefine.OnCatch , OnCatch );
	}

	public void OnMouseClick(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs) args;
		if ( msg.ContainMessage( "type" ))
		{
			if ( msg.GetMessage( "type" ).Equals( Global.MouseLeft ))
			{

			}else if ( msg.GetMessage( "type" ).Equals( Global.MouseRight ))
			{
				if ( rightMouse.Playing )
				{
					showWord( "" , Global.V32Str( rightMouse.transform.position ) );
					rightMouse.StopAndResetFrame();
				}
			}
		}
	}

	public void OnCatch(EventDefine eventName, object sender, EventArgs args)
	{
		if ( leftMouse.Playing )
		{
			showWord( "" , Global.V32Str( leftMouse.transform.position ) );
			leftMouse.StopAndResetFrame();

		}
	}

	int banana1Index = 0;

	public override void DealTrigger ( MessageEventArgs msg )
	{
		if ( "get_coin".Equals( msg.GetMessage( Global.TriggableMessage )) )
		{
			showWord( "" , msg.GetMessage("pos") );
			ArrowEnd.StopAndResetFrame();
			endBarrier.isBarrier = false;
		}

	}


}
