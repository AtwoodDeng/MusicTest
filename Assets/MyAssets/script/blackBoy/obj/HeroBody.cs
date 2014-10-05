using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class HeroBody : MonoBehaviour {



	public GameObject HeroHandPrefab;
	public GameObject HeroArmPrefab;

	//force
	public enum ForceType
	{
		ByHand,
		ToMouse
	};
	public ForceType forceType;
	public float ForceBlockTime = 1f;
	public float forceIntense = 50.0f;

	public HandGroup leftHandGroup;
	public HandGroup rightHandGroup;

	//throw hands
	private float throwI{
		get{
			return pThrowI * fHealth;
		}

	}
	public float pThrowI = 5f;
	public MeshCollider meshCollider;
	[HideInInspector]public ParticleSystem catchEffect;

	//advoid touch wall force
	public float advoidIntense = 1.0f;
	public float MAXAdvoidForce = 5.0f;

	//freezen
	public float MAXFreezenSpeed = 0.05f;
	public float freezenDragMutiply = 200f;
	private float dragOrignal = 0f;
	private float angularDragOrignal = 0f;

	//heal
	
	public enum HealthType
	{
		Dead,
		NotDead,
	};
	public HealthType healthType = HealthType.NotDead;
	public float fHealth
	{
		get {
			//return 1f / ( 0.5f + 1f / health );
			if ( healthType == HealthType.NotDead )
				return Mathf.Pow( health , 1.33f );
			else if ( healthType == HealthType.Dead )
				return 0.33f + 0.66f * health;
			return health;
		}
	}
	public float health = 1.0f;
	public float harmRate = 0.05f;
	public float recoverRate = 1.002f;
	private bool isDead = false;
	public float deadDragIncrease = 30f;


	public HeroHand.ForceType tempForceType = HeroHand.ForceType.SpinCW;

	public GameObject ScreenCloud;

	public class HandGroup{
		int num;
		List<HeroHand> hands = new List<HeroHand>();
		HeroBody body;
		
		public enum HandGroupState{
			None,
			Free,
			Fly,
			Shrink,
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
				if ( HandsStateAllEqual( HeroHand.HandState.Shrink ))
					return HandGroupState.Shrink;

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

		public Catchable getCatchable()
		{
			foreach( HeroHand hand in hands )
			{
				if ( hand.stayObjCatchable != null )
					return hand.stayObjCatchable;
			}
			return null;
		}
	}

	public tk2dSprite sprite;

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
		//creat screen Cloud
		{
			GameObject scPre = Resources.Load( Global.ScreenCloudPath ) as GameObject;
			ScreenCloud = Instantiate( scPre ) as GameObject;
			ScreenCloud.transform.parent = BObjManager.Instance.Effect.transform;
		}
		sprite = gameObject.GetComponent<tk2dSprite>();
	}


	void OnEnable() {
//		BEventManager.Instance.RegisterEvent (EventDefine.OnStrenchHand ,OnStrenchHand );
		BEventManager.Instance.RegisterEvent (EventDefine.OnShrinkHand, OnShrinkHand);
		BEventManager.Instance.RegisterEvent (EventDefine.OnMoveHand, OnMoveHand);
		BEventManager.Instance.RegisterEvent (EventDefine.OnChangeForce, OnChangeForce);
		BEventManager.Instance.RegisterEvent (EventDefine.OnFreezen, OnFreezen);
		BEventManager.Instance.RegisterEvent (EventDefine.OnUnfreezen, OnUnfreezen);
		BEventManager.Instance.RegisterEvent (EventDefine.OnStop, OnStop);
		BEventManager.Instance.RegisterEvent (EventDefine.OnBack, OnBack);
		BEventManager.Instance.RegisterEvent (EventDefine.OnTriggerable, OnTriggerable);
		BEventManager.Instance.RegisterEvent (EventDefine.OnCatch, OnCatch);

	}
	
	void OnDisable() { 
//		BEventManager.Instance.UnregisterEvent (EventDefine.OnStrenchHand, OnStrenchHand);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShrinkHand, OnShrinkHand);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnMoveHand, OnMoveHand);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnChangeForce, OnChangeForce);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnFreezen, OnFreezen);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnUnfreezen, OnUnfreezen);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnStop, OnStop);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnBack, OnBack);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnTriggerable, OnTriggerable);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnCatch, OnCatch);
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

	public void OnCatch(EventDefine eventName, object sender, EventArgs args)
	{
		//judge which hand
		MessageEventArgs msg = (MessageEventArgs)args;
		if ( msg.ContainMessage("HandID") )
		{
			int ID = int.Parse( msg.GetMessage("HandID") );
			if ( leftHandGroup.GetHandByID(ID) != null )
				_lastIsLeft = true;
			else
				_lastIsLeft = false;
		}
		ShrinkHand( !_lastIsLeft);
	}

	public void OnTriggerable(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs) args;
		if ( Global.PeanutMessage.Equals( msg.GetMessage(Global.TriggableMessage)) )
		{
			Heal();
		}
	}

	private Vector3 tempVelocity;
	private Vector3 tempAngleVelocity;

	public void OnStop(EventDefine eventName, object sender, EventArgs args)
	{
		tempVelocity = rigidbody.velocity;
		tempAngleVelocity = rigidbody.angularVelocity;
		rigidbody.velocity = Vector3.zero;
		rigidbody.drag = Global.LargeDrag;
		rigidbody.angularDrag = Global.LargeAngleDrag;
	}
	
	public void OnBack(EventDefine eventName, object sender, EventArgs args)
	{
		rigidbody.velocity = tempVelocity;
		rigidbody.angularVelocity = tempAngleVelocity;
		rigidbody.drag = dragOrignal;
		rigidbody.angularDrag = angularDragOrignal;
	}
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
		
	bool _lastIsLeft;
	public void OnMoveHand(EventDefine eventName, object sender, EventArgs args)
	{
		if ( healthType == HealthType.Dead && isDead )
			return;
		// Debug.Log("on move hand");
		MessageEventArgs msg = (MessageEventArgs)args;
		float posX = transform.localPosition.x;
		float posY = transform.localPosition.y ;
		Vector3 pos = transform.position;
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
		if ( msg.ContainMessage( "mousePos" ) )
		{
			pos = Global.Str2V3( msg.GetMessage( "mousePos" ));
		}
		//strench left and shrink right if left is free
		//else strench right and shrink left


		bool isLeft =  (leftHandGroup.state == HandGroup.HandGroupState.Free);
			
		leftHandGroup.setForceType( tempForceType );
		rightHandGroup.setForceType( tempForceType );

		//cannot strentch if a hand group is flying
		if ( leftHandGroup.state != HandGroup.HandGroupState.Fly 
		    && rightHandGroup.state != HandGroup.HandGroupState.Fly )
		{
			StrenchHandToward( pos.x , pos.y , isLeft );
			_lastIsLeft = isLeft;
			//ShrinkHand( !isLeft );
		}


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
		}else if (msg.ContainMessage("both"))
		{
			ShrinkHand(true);
			ShrinkHand(false);
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
		Vector3 force = leftHandGroup.getForce() + rightHandGroup.getForce();
		GUIDebug.add( ShowType.label , "Hand's force " + force + force.sqrMagnitude );
		if ( force.sqrMagnitude > Global.MAX_HAND_FORCE )
			force = force.normalized * Global.MAX_HAND_FORCE;
		//restrict to force 
		if ( force != Vector3.zero )
			force = force * forceIntense - Physics.gravity * rigidbody.mass * Time.deltaTime * Global.FORCE_NORMAL; 
		return force;

	}


	// Update is called once per frame
	void Update () {
		///force 

		if ( ifForce (Time.deltaTime) )
		{
			rigidbody.AddForce( getForce() , ForceMode.Impulse );
		}

		if ( meshCollider )
			meshCollider.convex =true;

		//recover
		Recover();
		GUIDebug.add( ShowType.label , "Health " + health );

		//screen cloud 
		UpdateScreenCloud();
		//adjust
//		Vector3 pos = transform.position;
//		pos.z = Global.BHeroPosition.z;
//		transform.position  = pos ;

		//set Rotation
		SetRotation();

	}


	public float bodyAngleAdjustRate = 0.5f;
	public float maxTolerateAngle = 45f;

	
	
	public GameObject smoke;

	public void SetRotation()
	{
		if ( leftHandGroup.state != HandGroup.HandGroupState.Catch
		    && rightHandGroup.state != HandGroup.HandGroupState.Catch )
		{
			GUIDebug.add( ShowType.label , "Body set rotation " );
			if ( Math.Abs( Global.adjustAngle( transform.eulerAngles.z ) ) > maxTolerateAngle )
			{
				float toAngle = 0;
				transform.rotation = Quaternion.Euler( 0 , 0 , Global.adjustAngle( toAngle ) * bodyAngleAdjustRate 
				                                               + Global.adjustAngle( transform.eulerAngles.z ) * ( 1- bodyAngleAdjustRate)  );
			}
		}
		smoke.transform.rotation = Quaternion.Euler( 0 , 0 , 0 );
	}
	public void DoFlipX( bool isLeft )
	{
		sprite.FlipX = isLeft;
		if ( isLeft )
		{
			Vector3 location = smoke.transform.localPosition;
			location.x = - Math.Abs( location.x );
			smoke.transform.localPosition = location;
		}
		else
		{
			Vector3 location = smoke.transform.localPosition;
			location.x = Math.Abs( location.x );
			smoke.transform.localPosition = location;
		}

	}

	private float forceTimer = 0f;
	bool ifForce( float deltaTime , bool ifUpdate = true , bool ifChange = true )
	{
		if ( ifUpdate )
			forceTimer += deltaTime;
		if ( forceTimer > ForceBlockTime )
		{
			if ( ifChange )
				forceTimer = 0;
			return true;
		}
		return false;
	}


	void UpdateScreenCloud()
	{
		tk2dSprite[] sprites = ScreenCloud.GetComponentsInChildren<tk2dSprite>();
		for( int i = 0 ; i < sprites.Length ; ++i )
		{
			Color col = sprites[i].color;
			col.a = ( 1f - Mathf.Pow( health , 0.9f ) ) * ( 1 - Global.MIN_SCREEN_VIEW ) ;
			sprites[i].color = col;
		}
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

	public void Restart()
	{
		tempAngleVelocity = tempVelocity = Vector3.zero;
	}

	
	public void Harm( Vector3 relativeVelocity )
	{
		if ( healthType == HealthType.NotDead )
			health *= 1 / ( 1 + Mathf.Pow( relativeVelocity.sqrMagnitude , 3f ) * harmRate );
		else if ( healthType == HealthType.Dead )
			health *= 1 / ( 1 + Mathf.Pow( relativeVelocity.sqrMagnitude , 1f ) * harmRate );
	}
	public void Recover ()
	{

		if ( healthType == HealthType.NotDead )
		{
			if ( health < Global.MIN_HEALTH )
				health = Global.MIN_HEALTH;
			if ( health <= Global.HURT_HEALTH3 )
			{
				health = Mathf.Min( health * recoverRate , Global.HURT_HEALTH3 );
			}else if ( health <= Global.HURT_HEALTH2 )
			{
				health = Mathf.Min( health * recoverRate , Global.HURT_HEALTH2 );
			}else if ( health <= Global.HURT_HEALTH1 )
			{
				health = Mathf.Min( health * recoverRate , Global.HURT_HEALTH1 );
			}else
			{
				health = Mathf.Min( health * recoverRate , Global.MAX_HEALTH );
			}
		}else if ( healthType == HealthType.Dead && !isDead )
		{
			if ( health < Global.MIN_HEALTH )
			{
				BEventManager.Instance.PostEvent( EventDefine.OnDead );
				isDead = true;
				rigidbody.drag *= deadDragIncrease;
				rigidbody.angularDrag *= deadDragIncrease;
			}
		}
	}
	public void Heal ()
	{
		if ( healthType == HealthType.NotDead )
		{
			if ( health <= Global.HURT_HEALTH3 )
			{
				health = Global.HURT_HEALTH3 * recoverRate;
			}else if ( health <= Global.HURT_HEALTH2 )
			{
				health = Global.HURT_HEALTH2 * recoverRate;
			}else if ( health <= Global.HURT_HEALTH1 )
			{
				health = Global.HURT_HEALTH1 * recoverRate;
			}else
			{
				health = Global.MAX_HEALTH;
			}
		}else if ( healthType == HealthType.Dead )
		{
			transform.eulerAngles = new Vector3(0,0,0);
			health = Global.MAX_HEALTH;
			isDead = false;
			rigidbody.drag /= deadDragIncrease;
			rigidbody.angularDrag /=deadDragIncrease;
		}
	}
}
