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
	public float toBodyForceRate = 0.33f;
	public GameObject stayObj;
	public Vector3 stayPos;
	public float MaxLength = 100f;

	[HideInInspector]public GameObject catchEffectPrefab;
	public float catchEffectTime = 1.0f;

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
	}
	public ForceType forceType;

	public DateTime throwTime = System.DateTime.Now;
	public double ShrinkTime = 1.0;
	public float ShrinkClose = 0.98f;

	// Use this for initialization
	void Start () {
		ID = getID();
		collider.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		GUIDebug.add(ShowType.label , state.ToString() );
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
		//Debug.Log("throw " + state + " force " + force);
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
		if ( state == HandState.Fly )
		{
			state = HandState.Shrink;

			
			MessageEventArgs msg = new MessageEventArgs();
			msg.AddMessage( "HandID" , ID.ToString());
			BEventManager.Instance.PostEvent( EventDefine.OnShrink , msg );

		}else if ( state == HandState.Catch )
		{
			state = HandState.Shrink;

			
			MessageEventArgs msg = new MessageEventArgs();
			msg.AddMessage( "HandID" , ID.ToString());
			BEventManager.Instance.PostEvent( EventDefine.OnShrink , msg );

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
		
		//Debug.Log("catch " + state );
		if ( state == HandState.Fly )
		{
			stayObj = _stayObj;
			stayPos = transform.position;
			rigidbody.velocity = Vector3.zero;
			state = HandState.Catch;

			CreateCatchEffect( stayPos );

			
			MessageEventArgs msg = new MessageEventArgs();
			msg.AddMessage( "HandID" , ID.ToString());
			msg.AddMessage( "ForceType", forceType.ToString());
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
			transform.position = stayPos;
	}

	public Vector3 getForce()
	{
		if ( state == HandState.Catch )
		{
			Vector3 toBody = heroBody.transform.position - transform.position;
			if ( forceType == ForceType.Pull )
				return forceIntense * ( - toBody );
			else if ( forceType == ForceType.SpinAntiCW )
			{
				Vector3 force = forceIntense * Vector3.Cross( toBody.normalized , Vector3.forward ) / transform.localPosition.magnitude;
				force -= forceIntense * toBodyForceRate * toBody ;
				return force;
			}else if ( forceType == ForceType.SpinCW )
			{
				Vector3 force = forceIntense * Vector3.Cross( toBody.normalized , Vector3.back ) / transform.localPosition.magnitude;
				force -= forceIntense * toBodyForceRate * toBody ;
				return force;
			}
		}
		return Vector3.zero;
	}
	void OnTriggerEnter( Collider collider )
	{
//		Debug.Log("trigger enter " + collider.gameObject.name + " " + collider.gameObject.tag );
		if ( state == HandState.Fly )
		{
			if ( Global.HandStayTag.Equals( collider.gameObject.tag ))
			{
				Catch( collider.gameObject );
			}

		}else if ( state == HandState.Shrink )
		{
			if ( Global.HeroTag.Equals( collider.gameObject.tag ))
				Free ();
		}
	}

	public void SetForceType( ForceType type )
	{
		if ( true )
		{
			forceType = type;
			catchEffectPrefab =  Resources.Load( Global.HandCatchEffectDict [ type.ToString() ] ) as GameObject;
			Debug.Log( "[SetForceType] " + Global.HandCatchEffectDict[ type.ToString() ] );
			if ( catchEffectPrefab == null )
			{
				Debug.Log("Cannot find prefab for " + type.ToString());
			}

		}

	}

	public Vector3 getArmPos()
	{
		return transform.position;
	}
}
