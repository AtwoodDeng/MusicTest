using UnityEngine;
using System.Collections;
using System;
using Holoville.HOTween;

public class Pullable : BlackWhiteRock {

	public enum PullType
	{
		Continuous,
	}
	public PullType pullType;

	public enum TestAccumulateType{
		Time,
		Energy,
	}
	public TestAccumulateType testAccumulateType;

	public Vector3 pullToward;
	public float pullLimit = 0.01f;
	public float testTime = 1f;
	private float nowTestTime = 0f;
	public float testEnergy = 0.05f;
	private float nowTestEnergy = 0f;
	public bool isShrink = true;
	public bool isShrinkOnFinish = true;
	public float pullTime = 1f;

	public int pullCount = 0;

	private bool isReadyPull = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( isReadyPull )
			DealPull( Time.deltaTime );
	}

	void DealPull( float deltaTime )
	{
		//get force 
		Vector3 force = - BObjManager.Instance.BHeroBody.GetHandByID( Convert.ToInt32( HandID )).getForceN();

		switch( forceType )
		{
		case ForceType.In:
			break;
		case ForceType.Out:
			force *= -1f;
			break;
		}
		float forceIntense = Vector3.Dot (force , pullToward.normalized );
		
		GUIDebug.add(ShowType.label , "[DealPull]forceIntense " + forceIntense.ToString() );

		//set test time
		if ( testForce( forceIntense ) )
		{
			nowTestTime += deltaTime;
			nowTestEnergy += deltaTime * forceIntense;
		}else{
			nowTestTime = 0f;
			nowTestEnergy = 0f;
		}

		//set Pull
		if ( TestPull() )
		{
			Pull();
			nowTestTime = 0f;
			nowTestEnergy = 0f;
		}
	}

	bool TestPull()
	{
		switch( testAccumulateType )
		{
		case TestAccumulateType.Energy:
			return nowTestEnergy > testEnergy;
		case TestAccumulateType.Time:
			return nowTestTime > testTime;
		}
		return false;
	}

	bool testForce( float force )
	{
			return force > pullLimit;
	}

	public override void DealCatch (MessageEventArgs msg)
	{
		Debug.Log("Pull Dealcatch " + getID() + gameObject.name);
			isReadyPull = true;
	}
	public override void DealShrink (MessageEventArgs msg)
	{
		Debug.Log("Pull DealShrink " + getID() + gameObject.name);
			isReadyPull = false;
	}

	public void Pull()
	{
		
		//set the spin animation
		HOTween.To( transform , pullTime , new TweenParms().Prop("position" , pullToward , true ).Ease(EaseType.EaseInOutBack));
		
		//send the message to shrink the hands
		if ( isShrink )
		{
			MessageEventArgs msg = new MessageEventArgs();
			msg.AddMessage("HandID" , HandID );
			BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , msg );
		}
		
		//set spin count 
		if ( pullCount > 0 )
			pullCount--;
		if ( pullCount == 0 )
		{
			enabled = false;
			isReadyPull = false;
			forceType = ForceType.In;
			if ( isShrinkOnFinish )
			{
				MessageEventArgs msg = new MessageEventArgs();
				msg.AddMessage("HandID" , HandID );
				BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , msg );
			}
			{
				MessageEventArgs msg = new MessageEventArgs();
				msg.AddMessage("CatchableID" , getID().ToString() );
				msg.AddMessage("Name" , gameObject.name );
				BEventManager.Instance.PostEvent( EventDefine.OnPullFinish , msg );
			}
			
		}
	}

}
