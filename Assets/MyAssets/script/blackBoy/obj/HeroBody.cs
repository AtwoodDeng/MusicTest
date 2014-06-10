using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class HeroBody : MonoBehaviour {

	public GameObject HeroHandPrefab;
	public GameObject HeroArmPrefab;

	public HandGroup leftHandGroup;
	public HandGroup rightHandGroup;
	public float throwI = 1.0f;
	public MeshCollider meshCollider;

	public float advoidIntense = 1.0f;
	public float MAXAdvoidForce = 5.0f;

	public class HandGroup{
		int num;
		List<HeroHand> hands = new List<HeroHand>();
		HeroBody body;
		
		public enum HandGroupState{
			None,
			Free,
			Fly,
			Catch
		}
		public HandGroupState state
		{
			get {
				if ( HandsStateAllEqual( HeroHand.HandState.Free ))
					return HandGroupState.Free;
				
				if ( HandsStateAllEqual( HeroHand.HandState.Catch ))
					return HandGroupState.Catch;
				
				if ( HandsStateAllEqual( HeroHand.HandState.None ))
					return HandGroupState.None;
				
				return HandGroupState.Fly;
				
			}
			set {
			}
		}
		
		private bool HandsStateAllEqual( HeroHand.HandState state )
		{
			for( int i = 0 ;i < hands.Count ; ++i )
				if ( hands[i].state != state )
					return false;
			return true;
		}
		
		public HandGroup( HeroBody _body )
		{
			body = _body;
			state = HandGroupState.Free;
		}
		
		public void AddHand( HeroHand hand )
		{
			hands.Add(hand);
		}
		
		public void Throw( Vector3 from , Vector3 to , float intense )
		{
			
			for( int i = 0 ; i < hands.Count ; ++i )
			{
				
				hands[i].Throw( to.normalized  * intense );
			}
			
		}
		
		public void Shrink()
		{
			for ( int i = 0 ; i < hands.Count ; ++i )
				hands[i].Shrink();
		}
		
		public Vector3 getForce()
		{
			Vector3 force = Vector3.zero;
			
			for( int i = 0 ; i < hands.Count ; ++i )
			{
				force += hands[i].getForce();
			}
			return force;
		}
	}

	void Start()
	{
		leftHandGroup = new HandGroup(this);
		rightHandGroup = new HandGroup(this);
		AddHand(true);
		AddHand(false);
		meshCollider = GetComponent<MeshCollider>();
	}


	void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnStrenchHand ,OnStrenchHand );
		BEventManager.Instance.RegisterEvent (EventDefine.OnShrinkHand, OnShrinkHand);
		BEventManager.Instance.RegisterEvent (EventDefine.OnMoveHand, OnMoveHand);
	}
	
	void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnStrenchHand, OnStrenchHand);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShrinkHand, OnShrinkHand);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMoveHand, OnMoveHand);
	}
	public void OnMoveHand(EventDefine eventName, object sender, EventArgs args)
	{
		// Debug.Log("on move hand");
		MessageEventArgs msg = (MessageEventArgs)args;
		float posX = transform.localPosition.x;
		float posY = transform.localPosition.y ;
		if ( !msg.ContainMessage("isLeft"))
		{
			Debug.Log("cannot find posX and posY");
			return;
		}
		if ( msg.ContainMessage("posX") || msg.ContainMessage("posY") )
		{
			posX = float.Parse( msg.GetMessage("posX"));
			posY = float.Parse( msg.GetMessage("posY"));
		}
		bool isLeft = bool.Parse(msg.GetMessage("isLeft"));
		if ( isLeft )
		{
			if ( leftHandGroup.state == HandGroup.HandGroupState.Free )
				StrenchHandToward(posX , posY , isLeft );
			else if ( leftHandGroup.state == HandGroup.HandGroupState.Catch )
				ShrinkHand(isLeft);
		}else
		{
			
			if ( rightHandGroup.state == HandGroup.HandGroupState.Free )
				StrenchHandToward(posX , posY , isLeft );
			else if ( rightHandGroup.state == HandGroup.HandGroupState.Catch )
				ShrinkHand(isLeft);
		}
	}
		
	public void OnStrenchHand(EventDefine eventName, object sender, EventArgs args)
	{
		Debug.Log("on strench");
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( !msg.ContainMessage("posX") || !msg.ContainMessage("posY") || !msg.ContainMessage("isLeft"))
		{
			Debug.Log("cannot find posX and posY");
			return;
		}
		float posX = float.Parse( msg.GetMessage("posX"));
		float posY = float.Parse( msg.GetMessage("posY"));
		bool isLeft = bool.Parse(msg.GetMessage("isLeft"));
		StrenchHandToward(posX , posY,isLeft);
	}

	public void OnShrinkHand(EventDefine eventName, object sender, EventArgs args)
	{
		Debug.Log("on shring");
		MessageEventArgs msg = (MessageEventArgs)args;
		bool isLeft = bool.Parse(msg.GetMessage("isLeft"));
		ShrinkHand( isLeft );
	}

	public void ShrinkHand( bool isLeft )
	{
		Debug.Log("shrink hand isLeft: " + isLeft.ToString());
		if ( isLeft )
			leftHandGroup.Shrink();
		else
			rightHandGroup.Shrink();
	}

	public void StrenchHandToward( float x , float y , bool isLeft )
	{
//		Debug.Log("[StrenchHandToward] strech hand pos " + x + " " + y );
//		Debug.Log("[StrenchHandToward] temp position " + transform.position);
		Vector3 toward = new Vector3(x ,y ,0) + Global.BstaticPosition - transform.position;
		Vector3 forceToward = toward.normalized;
		//Debug.Log("[StrenchHandToward] toward " + forceToward );
		if ( isLeft )
		{
			leftHandGroup.Throw( transform.position , forceToward , throwI ); 
		}else
		{
			rightHandGroup.Throw( transform.position, forceToward , throwI );
		}
	}

	public void AddHand( bool isLeft )
	{
		HeroHand hand = ((GameObject)Instantiate(HeroHandPrefab)).GetComponent<HeroHand>();
		HeroArm arm = ((GameObject)Instantiate(HeroArmPrefab)).GetComponent<HeroArm>();
		hand.Init( this , arm );
		arm.Init( this , hand );
		if ( isLeft ) 
		{
			hand.SetForceType( HeroHand.ForceType.SpinAntiCW );
			leftHandGroup.AddHand( hand );
		}
		else
		{
			hand.SetForceType( HeroHand.ForceType.SpinCW );
			rightHandGroup.AddHand( hand );
		}
	}


	// Update is called once per frame
	void Update () {
		///force 
		rigidbody.AddForce( leftHandGroup.getForce() + rightHandGroup.getForce() , ForceMode.Impulse );
		if ( meshCollider )
			meshCollider.convex =true;
	}

	public Vector3 getArmPos()
	{
		return transform.position;
	}



	void AdvoidCollision( Collider collider )
	{
		Vector3 cloestPoint = collider.ClosestPointOnBounds(transform.position);
		Vector3 toward = cloestPoint - transform.position;
		if ( toward.magnitude < 1e-7 )
			toward = collider.gameObject.transform.position - transform.position;

		Vector3 force = - toward.normalized;

		force *= ( 1f / toward.magnitude ) * advoidIntense;

		if ( force.sqrMagnitude > MAXAdvoidForce )
			force = force.normalized * MAXAdvoidForce;
		rigidbody.AddForce( force , ForceMode.Impulse );
	}

	bool WillCollide ( GameObject obj )
	{
		foreach( string tag in Global.CollidableTag )
		{
			if ( tag.Equals( obj.tag ))
				return true;
		}
		return false;
	}

	void OnTriggerStay( Collider collider )
	{
		if ( WillCollide( collider.gameObject ))
			AdvoidCollision( collider );
	}

}
