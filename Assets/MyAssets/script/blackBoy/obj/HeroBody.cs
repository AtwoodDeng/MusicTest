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
	[HideInInspector]public ParticleSystem catchEffect;

	public float advoidIntense = 1.0f;
	public float MAXAdvoidForce = 5.0f;

	public float MAXFreezenSpeed = 0.05f;
	public float freezenDragMutiply = 200f;
	private float dragOrignal = 0f;
	private float angularDragOrignal = 0f;

	public HeroHand.ForceType tempForceType = HeroHand.ForceType.SpinCW;

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

		public void setForceType( HeroHand.ForceType type )
		{
			
			for ( int i = 0 ; i < hands.Count ; ++i )
				hands[i].SetForceType( type );
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

		public HeroHand GetHandByID( int ID )
		{
			foreach( HeroHand hand in hands )
			{
				if ( hand.ID == ID )
					return hand;
			}
			return null;
		}
	}

	void Start()
	{
		leftHandGroup = new HandGroup(this);
		rightHandGroup = new HandGroup(this);
		AddHand(true);
		AddHand(false);
		meshCollider = GetComponent<MeshCollider>();
		if ( rigidbody )
		{
			dragOrignal = rigidbody.drag;
			angularDragOrignal = rigidbody.angularDrag;
		}
	}


	void OnEnable() {
//		BEventManager.Instance.RegisterEvent (EventDefine.OnStrenchHand ,OnStrenchHand );
		BEventManager.Instance.RegisterEvent (EventDefine.OnShrinkHand, OnShrinkHand);
		BEventManager.Instance.RegisterEvent (EventDefine.OnMoveHand, OnMoveHand);
		BEventManager.Instance.RegisterEvent (EventDefine.OnChangeForce, OnChangeForce);
		BEventManager.Instance.RegisterEvent (EventDefine.OnFreezen, OnFreezen);
		BEventManager.Instance.RegisterEvent (EventDefine.OnUnfreezen, OnUnfreezen);
//		BEventManager.Instance.RegisterEvent (EventDefine.OnCatch, OnCatch);

	}
	
	void OnDisable() { 
//		BEventManager.Instance.UnregisterEvent (EventDefine.OnStrenchHand, OnStrenchHand);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShrinkHand, OnShrinkHand);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMoveHand, OnMoveHand);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnChangeForce, OnChangeForce);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnFreezen, OnFreezen);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnUnfreezen, OnUnfreezen);
//		BEventManager.Instance.UnregisterEvent (EventDefine.OnCatch, OnCatch);
	}
//
//	public void OnCatch(EventDefine eventName, object sender, EventArgs args)
//	{
//		MessageEventArgs msg = (MessageEventArgs)args;
//		HeroHand.ForceType type = HeroHand.ForceType.Pull;
//		if ( msg.ContainMessage("ForceType")) 
//		{
//			type = (HeroHand.ForceType) Enum.Parse( typeof(HeroHand.ForceType) , msg.GetMessage("ForceType"));
//		}else
//		{
//			if ( msg.ContainMessage("HandID"))
//			{
//				HeroHand hand = GetHandByID( Convert.ToInt32( msg.GetMessage("HandID")));
//				type = hand.forceType;
//			}
//		}
//		CreateCatchEffect( type );
//
//	}
//
//	public void CreateCatchEffect( HeroHand.ForceType type )
//	{
//
//		GameObject catchEffectPrefab = Resources.Load( Global.HeroCatchEffectDict[type.ToString()]) as GameObject;
//		if ( catchEffectPrefab == null )
//		{
//			Debug.Log("Cannot find catch effect prefab " + Global.HeroCatchEffectDict[type.ToString()] );
//			return;
//		}
//		GameObject catchEffectObj = (GameObject)Instantiate( catchEffectPrefab );
//		if ( catchEffectObj == null )
//		{
//			Debug.Log("Cannot instantiate catch effect " + catchEffectPrefab.name);
//			return;
//		}
//		catchEffect.transform.parent = BObjManager.Instance.Effect.transform;
//		catchEffect.transform.localPosition = Vector3.zero;
//		BFollowWith followWith = catchEffect.GetComponent<BFollowWith>();
//		if ( followWith != null )
//			followWith.UpdateTarget( this.gameObject );
//
//
//
//	}

	public void OnFreezen(EventDefine eventName, object sender, EventArgs args)
	{
		//stay
		if ( rigidbody.velocity.sqrMagnitude > MAXFreezenSpeed )
			rigidbody.velocity = rigidbody.velocity.normalized * MAXFreezenSpeed;
		rigidbody.drag = dragOrignal * freezenDragMutiply;
		rigidbody.angularDrag = angularDragOrignal * freezenDragMutiply;
	}

	public void OnUnfreezen(EventDefine eventName, object sender, EventArgs args)
	{
		rigidbody.drag = dragOrignal;
		rigidbody.angularDrag = angularDragOrignal;
	}

	public void OnChangeForce(EventDefine eventName, object sender, EventArgs args)
	{
		if ( tempForceType == HeroHand.ForceType.SpinAntiCW )
			tempForceType = HeroHand.ForceType.SpinCW;
		else if ( tempForceType == HeroHand.ForceType.SpinCW )
			tempForceType = HeroHand.ForceType.SpinAntiCW;

		leftHandGroup.setForceType( tempForceType );
		rightHandGroup.setForceType( tempForceType );
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
		//strench left and shrink right if left is free
		//else strench right and shrink left

		bool isLeft =  (leftHandGroup.state == HandGroup.HandGroupState.Free);
			
		leftHandGroup.setForceType( tempForceType );
		rightHandGroup.setForceType( tempForceType );

		StrenchHandToward( posX , posY , isLeft );
		ShrinkHand( !isLeft );


//		bool isLeft = bool.Parse(msg.GetMessage("isLeft"));
//		if ( isLeft )
//		{
//			if ( leftHandGroup.state == HandGroup.HandGroupState.Free )
//			{
//				StrenchHandToward(posX , posY , isLeft );
//				ShrinkHand( !isLeft );
//			}
//			else if ( leftHandGroup.state == HandGroup.HandGroupState.Catch )
//			{
//				ShrinkHand(isLeft);
//			}
//		}else 
//		{
//			
//			if ( rightHandGroup.state == HandGroup.HandGroupState.Free )
//			{
//				StrenchHandToward(posX , posY , isLeft );
//				ShrinkHand( !isLeft );
//			}
//			else if ( rightHandGroup.state == HandGroup.HandGroupState.Catch )
//			{
//				ShrinkHand(isLeft);
//			}
//		}
	}
		
//	public void OnStrenchHand(EventDefine eventName, object sender, EventArgs args)
//	{
//		Debug.Log("on strench");
//		MessageEventArgs msg = (MessageEventArgs)args;
//		if ( !msg.ContainMessage("posX") || !msg.ContainMessage("posY") || !msg.ContainMessage("isLeft"))
//		{
//			Debug.Log("cannot find posX and posY");
//			return;
//		}
//		float posX = float.Parse( msg.GetMessage("posX"));
//		float posY = float.Parse( msg.GetMessage("posY"));
//		bool isLeft = bool.Parse(msg.GetMessage("isLeft"));
//		StrenchHandToward(posX , posY,isLeft);
//	}
//
	public void OnShrinkHand(EventDefine eventName, object sender, EventArgs args)
	{
		Debug.Log("on shring");
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( msg.ContainMessage("isLeft"))
		{
			bool isLeft = bool.Parse(msg.GetMessage("isLeft"));
			ShrinkHand( isLeft );
		}else if ( msg.ContainMessage("HandID"))
		{
			int handID = Convert.ToInt32( msg.GetMessage("HandID"));
			ShrinkHand( handID );
		}
	}

	public void ShrinkHand( bool isLeft )
	{
		Debug.Log("shrink hand isLeft: " + isLeft.ToString());
		if ( isLeft )
			leftHandGroup.Shrink();
		else
			rightHandGroup.Shrink();
	}

	public void ShrinkHand(int handID )
	{
		GetHandByID( handID ).Shrink();
	}

	public void StrenchHandToward( float x , float y , bool isLeft )
	{
		Vector3 toward = new Vector3(x ,y ,0) + Global.BstaticPosition - transform.position;
		Vector3 forceToward = toward.normalized;

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
		if ( arm.renderer != null )
		{
			if ( isLeft )
				arm.renderer.material.color = Global.ArmLeftColor;
			else
				arm.renderer.material.color = Global.ArmRightColor;
		}
		if ( isLeft ) 
		{
//			hand.SetForceType( HeroHand.ForceType.SpinAntiCW );
			hand.SetForceType( tempForceType );
			leftHandGroup.AddHand( hand );
		}
		else
		{
//			hand.SetForceType( HeroHand.ForceType.SpinCW );
			hand.SetForceType( tempForceType );
			rightHandGroup.AddHand( hand );
		}
	}

	public HeroHand GetHandByID( int ID )
	{
		HeroHand hand = leftHandGroup.GetHandByID( ID );
		if ( hand == null )
			hand = rightHandGroup.GetHandByID( ID );
		return hand;

	}

	public Vector3 getForce()
	{
		return leftHandGroup.getForce() + rightHandGroup.getForce();
	}


	// Update is called once per frame
	void Update () {
		///force 
		rigidbody.AddForce( getForce() , ForceMode.Impulse );

		if ( meshCollider )
			meshCollider.convex =true;

		//adjust
//		Vector3 pos = transform.position;
//		pos.z = Global.BHeroPosition.z;
//		transform.position  = pos ;
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
		if ( toward.magnitude < 1e-7 )
			return;
		Vector3 force = - toward.normalized;


		force *= ( 1f / toward.magnitude ) * advoidIntense;

		if ( force.sqrMagnitude > MAXAdvoidForce )
			force = force.normalized * MAXAdvoidForce;

		force.z = 0 ;

		if ( force.magnitude > 1e20 )
			return ;

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
