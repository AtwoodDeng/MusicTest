using UnityEngine;
using System.Collections;

public class CrowLevel4 : BLevel {


	public override void DealTrigger (string msg)
	{
		base.DealTrigger( msg );
		if ( "on_enter_wind_1".Equals( msg ) )
		{
			MessageEventArgs shrinkHandMessage = new MessageEventArgs();
			shrinkHandMessage.AddMessage("both" , "1" );
			BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , shrinkHandMessage );
		}
	}
}
