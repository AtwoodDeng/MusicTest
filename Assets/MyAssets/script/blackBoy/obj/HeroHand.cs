using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class HeroHand : MonoBehaviour {
	

	public int ID;

	static int _ID = 0 ;
	static int getID(){
		return _ID++;
	}


	public HeroBody heroBody;
	public HeroArm heroArm;
	public float forceIntense = 0.025f;
	public float minForceLength = 0.001f;
	public float maxForceLength = 7f;
	public float forceIntenseN = 0.33f;
	public enum SecondForceType
	{
		Always,
		N,
		Spring,
		SpringAndN,

	}
	public SecondForceType secondForceType;
	public float toBodyForceRate = 0.6f;
	public float MinChangeForceTypeTime = 2f;
	private float ForceTypeTime = 0f;

	//public float MinChangeDistance = 0.1f;
	public GameObject stayObj
	{
		get {
			return _stayObj;
		}
		set {
			_stayObj = value;
			stayObjCatchable = _stayObj.GetComponent<Catchable>();
		}
	}
	private GameObject _stayObj;
	public Catchable stayObjCatchable;
	public Vector3 stayPos
	{
		get {
			if ( stayAdhereObj != null )
				return stayAdhereObj.transform.position;
			return transform.position;
		}
	}
	private GameObject stayAdhereObj;
	public GameObject stayAdhereEffect;

	public float minFlyTime = 0.2f;
	private float MaxLength
	{
		get{
			return pMaxLength * Mathf.Pow ( fHealth , 1.5f ) ;
		}
	}

	public float pMaxLength = 8f;


	[HideInInspector]public GameObject catchEffectPrefab;
	public float catchEffectTime = 1.0f;

	public float maxVelocity = 1f;

	public enum HandState{
		None,
		Free,
		Fly,
		Shrink,
		Catch,
	}
	public HandState state = HandState.Free;
	public enum ForceType{
		SpinCW,
		SpinAntiCW,
		Pull,
		None,
	}
	public ForceType forceType;

	public DateTime throwTime = System.DateTime.Now;
	public double ShrinkTime = 1.0;
	public float ShrinkClose = 0.98f;

	private float fHealth{
		get{
			if ( heroBody != null )
				return heroBody.fHealth;
			return 1f;
		}
	}


	// Use this for initialization
	void Start () {
		ID = getID();
		collider.isTrigger = true;
		GameObject emptyPrefab = Resources.Load(Global.EmptyPrefabPath) as GameObject;
		stayAdhereObj = Instantiate( emptyPrefab ) as GameObject;
		stayAdhereObj.transform.parent = this.transform;
		stayAdhereObj.transform.localPosition = Vector3.zero;
//		stayAdhereEffectPrefabs = new GameObject[Global.HandStayObjCatchEffect.Count];
//		foreach( string key in Global.HandStayObjCatchEffect.Keys )
//		{
//			int index = (int)Enum.Parse( typeof(ForceType) , key );
//			stayAdhereEffectPrefabs[index] = 
//				Resources.Load(Global.HandStayObjCatchEffect[key]) as GameObject;
//		}
	}
	
	// Update is called once per frame
	void Update () {
//		GUIDebug.add(ShowType.label , state.ToString() );
		if ( state == HandState.Fly )
		{
			DateTime now = System.DateTime.Now;
			if ( ( now - throwTime ).TotalSeconds > ShrinkTime )
				Shrink();
		}else if ( state == HandState.Shrink )
		{
			DoShrink();
		}else if ( state == HandState.Free )
		{
			DoFree();
		}else if ( state == HandState.Catch )
		{
			DoCatch();
		}

		if ( CheckLength())
			Shrink();
	}

	public void setRotationToward( Vector3 toward )
	{
		toward.z = 0;
		float angle =  Vector3.Angle( new Vector3( 1f , 0, 0 ) , toward );
		if ( toward.y < 0 )
			angle = -angle;
		transform.rotation = Quaternion.Euler( 0 , 0 , angle );
	}
	
	public bool CheckLength()
    {
		return transform.localPosition.sqrMagnitude > MaxLength;
	}

	public void Init( HeroBody body , HeroArm arm) {
		heroBody = body;
		heroArm = arm;

		transform.parent = body.transform;
		transform.localPosition = Vector3.zero;
		transform.localScale  = Vector3.one;
		state = HandState.Free;

	}

	public void Throw( Vector3 force )
	{
		Debug.Log("throw " + state + " force " + force);
		if ( state == HandState.Free )
		{
			state = HandState.Fly;
			rigidbody.velocity = force;
//			float angle = (float)(Math.Atan( 1.0 * force.y/ force.x) * 180 / Math.PI);
//			if ( force.x < 0 )
//				angle += 180f;
			setRotationToward(force);
			throwTime = System.DateTime.Now;
		}
	}
	public void Shrink()
	{
		//Debug.Log("shrink " + state );
		bool isShrink = false;
		if ( state == HandState.Fly )
		{
			state = HandState.Shrink;
			isShrink = true;

		}else if ( state == HandState.Catch )
		{
			state = HandState.Shrink;
			isShrink = true;
		}

		if ( isShrink )
		{
			SetForceType( ForceType.None );
			MessageEventArgs msg = new MessageEventArgs();
			msg.AddMessage( "HandID" , ID.ToString());
			BEventManager.Instance.PostEvent( EventDefine.OnShrink , msg );
			stayAdhereObj.transform.parent = this.transform;
			stayAdhereObj.transform.localPosition = Vector3.zero;
		}
	}

	public void DoShrink()
	{
		if ( state == HandState.Shrink )
		{
			rigidbody.velocity = Vector3.zero;
			Vector3 toward = Vector3.zero - transform.localPosition;
			transform.localPosition = toward * ( 1f - ShrinkClose ) + transform.localPosition;
		}
	}

	public void Free()
	{
		//Debug.Log("free " + state );
		state = HandState.Free;
		transform.localPosition = heroBody.transform.localPosition;
	}

	public void DoFree()
	{
		if ( state == HandState.Free )
			transform.localPosition = Vector3.zero;
	}

	public void Catch( GameObject _stayObj )
	{
		
		Debug.Log("catch out " + state );
		if ( state == HandState.Fly )
		{
			Debug.Log("Catch in");
			stayObj = _stayObj;

			if ( stayAdhereObj != null )
			{
				stayAdhereObj.transform.parent = _stayObj.transform;
				stayAdhereObj.transform.position = transform.position;
			}
			rigidbody.velocity = Vector3.zero;
			state = HandState.Catch;

			SetForceType( stayObjCatchable.getForceType() );
			CreateCatchEffect( stayPos );


			MessageEventArgs msg = new MessageEventArgs();
			msg.AddMessage( "HandID" , ID.ToString());
			msg.AddMessage( "ForceType", forceType.ToString());
			if ( _stayObj.GetComponent<Catchable>() != null )
			{
				msg.AddMessage( "CatchableID" , _stayObj.GetComponent<Catchable>().getID ().ToString());
				Debug.Log("HeroHand addID " + _stayObj.GetComponent<Catchable>().getID ().ToString() );
				Vector3 pos = transform.position;
				msg.AddMessage( "CatchPosition" , pos.x + " " + pos.y + " " + pos.z );
				msg.AddMessage( "CatchPositionGlobal" , Global.V32Str( pos ) );
			}
			BEventManager.Instance.PostEvent( EventDefine.OnCatch , msg );
		}
	}

	public void CreateCatchEffect( Vector3 position )
	{
		if ( catchEffectPrefab != null )
		{
			GameObject catchEffectTemp = (GameObject)Instantiate( catchEffectPrefab );
			if ( catchEffectTemp != null )
			{
				catchEffectTemp.transform.parent = BObjManager.Instance.Effect.transform;
				Vector3 startPos = position;
				startPos.z = Global.BEffecPosition.z;
				catchEffectTemp.transform.position = startPos;
			}else{
				Debug.Log("Cannot instantiate Catch Effect" );
			}
		}
	}

	public void DoCatch()
	{
		if ( state == HandState.Catch )
		{
			transform.position = stayPos;
			UpdateForceType(true,false);
		}
	}

	public Vector3 getForce()
	{
		UpdateForceType(false,false);

		if ( state == HandState.Catch )
		{
//			if ( forceType == ForceType.Pull )
//				return getForceMain();
//			else
//			{
//				return ( getForceMain() + getForceSecond()) * Mathf.Pow( fHealth , 0.5f );
//
//			}
//			GUIDebug.add(ShowType.label , "forec M " + (getForceMain()*1000f) + getForceMain().sqrMagnitude + " S " + (getForceSecond()*1000f) + getForceSecond().sqrMagnitude );
//			GUIDebug.add (ShowType.label , "VN " + (getVN()*100f) +getVN().sqrMagnitude + " VT " + (getVT() * 100f ) + getVT().sqrMagnitude);
//			GUIDebug.add( ShowType.label , "velocity " + ( heroBody.rigidbody.velocity*100f ));
			return ( getForceMain() + getForceSecond()) * Mathf.Pow( fHealth , 0.5f ) ;
		}
		return Vector3.zero;
	}

	

	public Vector3 getForceMain()
	{
		Vector3 toBody = heroBody.transform.position - transform.position;

		if ( heroBody.forceType == HeroBody.ForceType.ByHand )
		{
			if ( stayObjCatchable != null )
			{
				float length = transform.localPosition.sqrMagnitude;
				Vector3 force = Vector3.zero;
				if ( length > minForceLength )
					force = stayObjCatchable.getForceMain( toBody );

				//adjust by VT
				float vt = getVT().sqrMagnitude;
				if ( vt > maxVelocity )
					vt = maxVelocity;
				return forceIntense * force * ( maxVelocity - vt );
			}else
			{
//				if ( forceType == ForceType.Pull )
//					return forceIntense * ( - toBody );
//				else if ( forceType == ForceType.SpinAntiCW )
//				{
//					float handLength = transform.localPosition.sqrMagnitude;
//					
//					if ( handLength < Global.MIN_HAND_FORCE_LENGTH )
//						return Vector3.zero;
//
//					Vector3 force = forceIntense * Vector3.Cross( toBody.normalized , Vector3.back ) / transform.localPosition.magnitude;
//					//				GUIDebug.add( ShowType.label , "ForceT" + force + " " + force.sqrMagnitude);
//					//				GUIDebug.add( ShowType.label , "ForceN" + forceIntense * toBodyForceRate * toBody + " " + (forceIntense * toBodyForceRate * toBody).sqrMagnitude);
//					return force;
//				}else if ( forceType == ForceType.SpinCW )
//				{
//					float handLength = transform.localPosition.sqrMagnitude;
//					
//					if ( handLength < Global.MIN_HAND_FORCE_LENGTH )
//						return Vector3.zero;
//
//					Vector3 force = forceIntense * Vector3.Cross( toBody.normalized , Vector3.forward ) / transform.localPosition.magnitude;
//					//				GUIDebug.add( ShowType.label , "ForceT" + force + " " + force.sqrMagnitude );
//					//				GUIDebug.add( ShowType.label , "ForceN" + forceIntense * toBodyForceRate * toBody + " " + (forceIntense * toBodyForceRate * toBody).sqrMagnitude );
//					return force;
//				}
			}
		}
		else if ( heroBody.forceType == HeroBody.ForceType.ToMouse )
		{
//			Vector3 force = BInputManager.Instance.MousePosition - heroBody.transform.position;
//			force = force.normalized * heroBody.forceIntense;
//			return force;
		}

		return Vector3.zero;
	}

	public float adjustNRate = 0.1f;
	public Vector3 getForceSecond()
	{
		if ( state == HandState.Catch )
		{
			if ( secondForceType == SecondForceType.Always )
			{
				float direct = -1f;
				if ( stayObjCatchable != null )
				{
					switch( stayObjCatchable.forceType)
					{
					case Catchable.ForceType.In :
						direct = -1f;
						break;
					case Catchable.ForceType.Out:
						direct = 1f;
						break;
					}
				}
				Vector3 toBody = heroBody.transform.position - transform.position;
				return direct * forceIntense * toBodyForceRate * toBody ;
			}else if ( secondForceType == SecondForceType.N )
			{
				//force N
				Vector3 toBody = heroBody.transform.position - transform.position;
				Vector3 VT = getVT();
				Vector3 VN = getVN();
				Vector3 force =  - VT.magnitude / toBody.sqrMagnitude 
					* heroBody.rigidbody.mass
						* toBody.normalized
						* Time.deltaTime 
						* Global.FORCE_NORMAL
						* forceIntenseN;
				
				return force;
			}else if ( secondForceType == SecondForceType.Spring )
			{
				Vector3 toBody = heroBody.transform.position - transform.position;
				float length = toBody.sqrMagnitude;
				if ( minForceLength > length )
				{
					//force out 
					return forceIntense * toBodyForceRate * toBody.normalized * ( minForceLength - length );
				}else if ( maxForceLength < length )
				{
					//force in
					return - forceIntense * toBodyForceRate * toBody.normalized * ( length - maxForceLength );	
				}
			}else if ( secondForceType == SecondForceType.SpringAndN )
			{
				Vector3 force = Vector3.zero;
				Vector3 toBody = heroBody.transform.position - transform.position;
				float length = toBody.sqrMagnitude;
				Vector3 VN = getVN();
				if ( minForceLength > length )
				{
					//force out 
					force +=  forceIntense * toBodyForceRate * toBody.normalized * ( minForceLength - length );
					return force;
				}else if ( maxForceLength < length )
				{
					//force in
					if ( Vector3.Dot( VN , toBody ) > 0 )
						force += - forceIntense * toBodyForceRate * toBody.normalized * Mathf.Pow ( ( length - maxForceLength ) , 3f ) * VN.sqrMagnitude;
				
				}
//				if ( minForceLength > length )
//				{
//					if ( Vector3.Dot( VN , toBody ) < 0 )
//						force += VN.sqrMagnitude * adjustNRate * heroBody.rigidbody.mass * Global.FORCE_NORMAL * toBody.normalized;
//
//				}else if ( maxForceLength < length )
//				{
//					if ( Vector3.Dot( VN , toBody ) > 0 )
//						force += - VN.sqrMagnitude * adjustNRate * heroBody.rigidbody.mass * Global.FORCE_NORMAL * toBody.normalized;
//				}
				
				//force N
				Vector3 VT = getVT();
				force += - VT.magnitude / toBody.sqrMagnitude 
					* heroBody.rigidbody.mass
						* toBody.normalized
						* Time.deltaTime 
						* Global.FORCE_NORMAL
						* forceIntenseN;
				return force;
			}
			
		}
		return Vector3.zero;
	}

	public void UpdateForceType( bool ifUpdate = true , bool ifForceChange = false)
	{
//		GUIDebug.add( ShowType.label , "forceType " + forceType.ToString() );

//		if ( state == HandState.Catch )
//		{
//			
//			if ( ifUpdate )
//				ForceTypeTime += Time.deltaTime;
//
//			if ( stayObj != null )
//			{
//				Vector3 mousePos = BInputManager.Instance.MousePosition;
//				Vector3 bodyPos = heroBody.transform.position;
//				Vector3 handPos = stayObj.transform.position;
//
//				if ( Mathf.Abs( handPos.y - bodyPos.y ) < MinChangeDistance )
//				{
//					ForceTypeTime = 0f;
//				}
//				if ( ForceTypeTime > MinChangeForceTypeTime )
//				{
//					ChangeForceType();
//				}
//				if ( ifForceChange )
//				{
//					SetForceType( ForceType.None );
//					ChangeForceType();
//				}
//			}
//		}
	}

	public void ChangeForceType()
	{
//		if ( state == HandState.Catch )
//		{
//			if ( stayObj != null )
//			{
//				
//				Vector3 mousePos = BInputManager.Instance.MousePosition;
//				Vector3 bodyPos = heroBody.transform.position;
//				Vector3 handPos = stayObj.transform.position;
//
//				ForceType toForceType = ForceType.Pull;
//				//hand over body
//				if ( handPos.y > bodyPos.y )
//				{
//					if ( mousePos.x > bodyPos.x )
//					{
//						toForceType = ForceType.SpinAntiCW;
//					}else
//					{
//						toForceType = ForceType.SpinCW;
//					}
//				}
//				else
//					//body over hand 
//				{
//					if ( mousePos.x  > bodyPos.x )
//					{
//						toForceType = ForceType.SpinCW;
//					}else
//					{
//						toForceType = ForceType.SpinAntiCW;
//					}
//					
//				}
//
//				if ( toForceType != forceType )
//				{
//					SetForceType( toForceType );
//				}
//
//			}
//		}
	}

	public void SetForceType( ForceType toForceType )
	{
//		if ( forceType == toForceType )
//			return ;
//		Debug.Log("SetForceType" + " from " + forceType + " to " + toForceType );

		catchEffectPrefab =  Resources.Load( Global.HandCatchEffectDict [ toForceType.ToString() ] ) as GameObject;

		if ( stayAdhereEffect != null )
		{
			AutoDestory destory = stayAdhereEffect.AddComponent<AutoDestory>();
			destory.isStopParticle = true;
			destory.destroyTime = 3f;
			destory.StartAutoDestory();
		}
//		Debug.Log( "stayAdhereObj" + stayAdhereObj + "stat " + state );
		if ( stayAdhereObj != null && state == HandState.Catch )
		{

			switch( toForceType )
			{
				case ForceType.SpinAntiCW:
				{
					GameObject prefab = Resources.Load( Global.HandStayObjCatchEffect[ toForceType.ToString()] ) as GameObject;
					if ( prefab == null )
						break;
					stayAdhereEffect = Instantiate( prefab ) as GameObject;
					if ( stayAdhereEffect == null )
						break;
					stayAdhereEffect.transform.parent = stayAdhereObj.transform.parent;
					stayAdhereEffect.transform.localPosition = stayAdhereObj.transform.localPosition;
					stayAdhereEffect.transform.localScale = Vector3.one;
				}
					break;
				case ForceType.SpinCW:
				{
					GameObject prefab = Resources.Load( Global.HandStayObjCatchEffect[ toForceType.ToString()] ) as GameObject;
					if ( prefab == null )
						break;
					stayAdhereEffect = Instantiate( prefab ) as GameObject;
					if ( stayAdhereEffect == null )
						break;
					stayAdhereEffect.transform.parent = stayAdhereObj.transform.parent;
					stayAdhereEffect.transform.localPosition = stayAdhereObj.transform.localPosition;
					stayAdhereEffect.transform.localScale = Vector3.one;
				}
					break;
				default:
					break;
				}
		}

//		if (!( stayAdhereEffectPrefabs == null || stayAdhereEffectPrefabs.Length < Global.HandStayObjCatchEffect.Count ) )
//		{
//
//			if ( toForceType == ForceType.SpinCW )
//			{
//				
//				Debug.Log("CW Index is " + ((int)ForceType.SpinCW).ToString());
//				stayAdhereEffect = Instantiate( stayAdhereEffectPrefabs[(int)ForceType.SpinCW]) as GameObject;
//				stayAdhereEffect.transform.parent = stayAdhereObj.transform;
//				stayAdhereEffect.transform.localPosition = Vector3.zero;
//				stayAdhereEffect.transform.localScale = Vector3.one;
//
//			}else if ( toForceType == ForceType.SpinAntiCW )
//			{
//				Debug.Log("AntiCW Index is " + ((int)ForceType.SpinAntiCW).ToString());
//				Debug.Log("prefabs size is " + stayAdhereEffectPrefabs.Length );
//
//				stayAdhereEffect = Instantiate( stayAdhereEffectPrefabs[(int)ForceType.SpinAntiCW]) as GameObject;
//				stayAdhereEffect.transform.parent = stayAdhereObj.transform;
//				stayAdhereEffect.transform.localPosition = Vector3.zero;
//				stayAdhereEffect.transform.localScale = Vector3.one;
//			}
//		}

		forceType = toForceType;
	}



	public Vector3 getForceN()
	{
		//get force V
		Vector3 radius = heroBody.transform.position - stayObj.transform.position;
		Vector3 velocity = heroBody.rigidbody.velocity;
		Vector3 velocityN = radius.normalized * Vector3.Dot( radius.normalized , velocity );
		Vector3 VelocityT = velocity - velocityN;

		Vector3 forceV = - radius.normalized * VelocityT.magnitude / radius.sqrMagnitude;

		//get force main
		Vector3 forceM = getForceMain();
		forceM = - radius.normalized * Vector3.Dot( forceM , radius.normalized );

		return forceV + getForceSecond() + forceM;
	}

	void OnTriggerEnter( Collider collider )
	{
		if ( state == HandState.Fly )
		{
			Debug.Log("trigger enter " + collider.gameObject.name + " " + collider.gameObject.tag );
			if ( ( System.DateTime.Now - throwTime ).TotalSeconds > minFlyTime )
			{
			
				if ( Global.HandStayTag.Equals( collider.gameObject.tag ))
				{
					Catch( collider.gameObject );
				}
			}

		}else if ( state == HandState.Shrink )
		{
			if ( Global.HeroTag.Equals( collider.gameObject.tag ))
				Free ();
		}
	}

//	public void SetForceType( ForceType type )
//	{
//		if ( true )
//		{
//			forceType = type;
//			catchEffectPrefab =  Resources.Load( Global.HandCatchEffectDict [ type.ToString() ] ) as GameObject;
//			Debug.Log( "[SetForceType] " + Global.HandCatchEffectDict[ type.ToString() ] );
//			if ( catchEffectPrefab == null )
//			{
//				Debug.Log("Cannot find prefab for " + type.ToString());
//			}
//
//		}
//
//	}

	public Vector3 getArmPos()
	{
		return transform.position;
	}
	public Vector3 getVN()
	{
		if ( heroBody != null )
		{
			Vector3 v = heroBody.rigidbody.velocity;
			Vector3 toBody = heroBody.transform.position - transform.position;
			Vector3 VN = Vector3.Dot( v , toBody.normalized ) * toBody.normalized;
			return VN;
		}
		return Vector3.zero;
	}
	public Vector3 getVT()
	{
		if ( heroBody != null )
		{
			Vector3 VT = heroBody.rigidbody.velocity - getVN();
			return VT;
		}
		return Vector3.zero;
	}


}
