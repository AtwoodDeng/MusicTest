using UnityEngine;
using System.Collections;

public class CrowLevel2 : BLevel {
	
	public enum State{
		Init,
		Play,
		End,
	}
	public State state = State.Init;
	
	//===== start point =====
//	public GameObject startPoint;
	public int maxHalo;
	
	//records
	//level 0_1
	private bool isOutOfRange = false;
	private bool isClick = false;
	private bool isWhite = false;
	private bool isBlack = false;
	private bool isMind = false;
	
	//all
	private int num_pea = 0 ;
	
	
	void OnEnable() {
		base.OnEnable();
	}
	
	void OnDisable() {
		base.OnDisable();
	}

	
	public override void DealTrigger (string msg)
	{
		base.DealTrigger(msg);
		if ( "out_of_range".Equals( msg ) )
		{
			Restart( "" );
		}
//		if ( "on_end_point".Equals( msg ))
//		{
//			BEventManager.Instance.PostEvent( EventDefine.OnFrontMenu , new MessageEventArgs() );
//		}
	}
	
	public override void Restart (string msg)
	{
		HeroBody body = BObjManager.Instance.BHeroBody;
		body.transform.position = startPoint.transform.position;
		Vector3 velocity = body.rigidbody.velocity;
		velocity.x = 0;
		velocity.y = 0;
		body.rigidbody.velocity = velocity;
		body.Restart();
	}
}
