using UnityEngine;
using System.Collections;
using System;

public class CrowLevel4 : BLevel {





	public override void DealTrigger (string msg)
	{
		base.DealTrigger( msg );
		if ( "shrink".Equals( msg ) )
		{
			MessageEventArgs shrinkHandMessage = new MessageEventArgs();
			shrinkHandMessage.AddMessage("both" , "1" );
			BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , shrinkHandMessage );
		}


	}
}
