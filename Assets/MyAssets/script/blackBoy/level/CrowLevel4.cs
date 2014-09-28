using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CrowLevel4 : BLevel {

	public List<TwoSideSpin> backVillCitys = new List<TwoSideSpin>();

	protected void OnEnable() {
		base.OnEnable();
		BEventManager.Instance.RegisterEvent (EventDefine.OnAfterCatch ,OnAfterCatch );
		
	}
	
	protected void OnDisable() {
		base.OnEnable();
		BEventManager.Instance.UnregisterEvent (EventDefine.OnAfterCatch, OnAfterCatch);
	}

	public void OnAfterCatch(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( !string.IsNullOrEmpty( msg.GetMessage("BWname" ) ) )
		{
			if ( "village".Equals( msg.GetMessage("BWname") ) )
			{
				foreach( TwoSideSpin obj in backVillCitys )
				{
					obj.Spin( 0 );
				}
			}
			if ( "city".Equals( msg.GetMessage("BWname") ) )
			{
				foreach( TwoSideSpin obj in backVillCitys )
				{
					obj.Spin( 1 );
				}
			}
			if ( "change".Equals( msg.GetMessage("BWname") ) )
			{
				foreach( TwoSideSpin obj in backVillCitys )
				{
					obj.Spin( );
				}
			}
		}
	}


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
