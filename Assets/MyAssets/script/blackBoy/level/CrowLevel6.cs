using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowLevel6 : BLevel {

	public Shine NoClick;
	public Shine NoClick2;
	public Shine NoSmoke;
	public Shine NoSmoke2;
	public Shine Woman_Man;
	public Shine Exit;
	public Shine NoRightButton;


	public override void DealTrigger (string msg)
	{
		base.DealTrigger( msg );
		if ( "out_of_range".Equals(msg))
		{
			Restart(""); 
		}
		if ( "enter_no_click".Equals( msg ))
		{
			NoClick.DoShine();
			NoClick2.DoShine();
		}
		if ( "enter_no_smoke".Equals( msg ))
		{
			NoSmoke.DoShine();
			NoSmoke2.DoShine();
		}
		if ( "enter_woman_man".Equals( msg ))
		{
			Woman_Man.DoShine();
		}
		if ( "enter_no_right".Equals( msg ))
		{
			NoRightButton.DoShine();
		}
		if ( "enter_exit".Equals( msg ))
		{
			Exit.DoShine();
		}

		
	}

	int banana1Index = 0;

	public override void DealTrigger ( MessageEventArgs msg )
	{
		if ( "get_coin".Equals( msg.GetMessage( Global.TriggableMessage )) )
		{
			showWord( "A coin!" , msg.GetMessage("pos") );
		}

	}
}
