using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(LineRenderer))]
public class HeroArm : MonoBehaviour {

	public HeroBody body;
	public HeroHand hand;
	public LineRenderer lineRenderer;
	public GameObject catchEffectBody;

	public bool enableCatchEffect = false;

	// Update is called once per frame
	void Update () {

		if ( lineRenderer == null )
			lineRenderer = GetComponent<LineRenderer>();
		if ( body != null && hand != null )
		{
			lineRenderer.SetPosition( 0 , body.getArmPos() + Global.BHeroArmOff);
			lineRenderer.SetPosition( 1 , hand.getArmPos() + Global.BHeroArmOff);
		}
		if ( catchEffectBody != null )
		{
			Vector3 bodyToHand = hand.transform.position - body.transform.position;
			bodyToHand.z = 0 ;
			Vector3 direction = Vector3.Cross( Vector3.back , bodyToHand );

			direction = - body.getForce();

			if ( direction.magnitude > 0.001f )
			{

	//			GUIDebug.add(ShowType.label , "bth:" + bodyToHand.ToString() +  " direction:" + direction.ToString());
				float angle =  Vector3.Angle( new Vector3( 1f , 0, 0 ) , direction );
	//			GUIDebug.add(ShowType.label , "Angle " + angle );
				if ( direction.y < 0 )
					angle = -angle;
				catchEffectBody.transform.rotation = Quaternion.Euler( 0 , 0 , angle );
			}

		}
	}

	public void Init( HeroBody _body , HeroHand _hand )
	{
		body = _body;
		hand = _hand;
		transform.parent = _body.transform;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		Update();
	}

	void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnCatch, OnCatch);
		BEventManager.Instance.RegisterEvent (EventDefine.OnShrink, OnShrink);
		
	}
	
	void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnCatch, OnCatch);
		BEventManager.Instance.UnregisterEvent (EventDefine.OnShrink, OnShrink);
	}

	public void OnShrink(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		if( !msg.ContainMessage( "HandID" ) || hand == null )
		{
			Debug.Log("Cannot deal OnCatch for missing message" );
			return;
		}
		
		if ( hand.ID.ToString() != msg.GetMessage("HandID"))
		{
			//should not deal this catch
			return;
		}
		DestoryCatchEffectBody();

	}
	
	public void OnCatch(EventDefine eventName, object sender, EventArgs args)
	{

		MessageEventArgs msg = (MessageEventArgs)args;
		if( !msg.ContainMessage( "HandID" ) || hand == null )
		{
			Debug.Log("Cannot deal OnCatch for missing message" );
			return;
		}

		if ( hand.ID.ToString() != msg.GetMessage("HandID"))
		{
			//should not deal this catch
			return;
		}
		CreateCatchEffectBody( hand.forceType );
		
	}
	
	public void CreateCatchEffectBody( HeroHand.ForceType type )
	{

		if ( catchEffectBody != null )
		{
			DestoryCatchEffectBody();
		}

		//Catch Effect
		GameObject catchEffectPrefab = Resources.Load( Global.ArmCatchEffectDict [ type.ToString() ] ) as GameObject;
		if ( catchEffectPrefab == null )
		{
			Debug.Log("Cannot find catch effect prefab " + Global.ArmCatchEffectDict[type.ToString()] );
			return;
		}
		catchEffectBody = (GameObject)Instantiate( catchEffectPrefab );
		if ( catchEffectBody == null )
		{
			Debug.Log("Cannot instantiate catch effect " + catchEffectPrefab.name);
			return;
		}

		catchEffectBody.transform.parent = BObjManager.Instance.Effect.transform;
		catchEffectBody.transform.localPosition = Vector3.zero;
		BFollowWith followWith = catchEffectBody.GetComponent<BFollowWith>();

		if ( followWith != null )
			followWith.UpdateTarget( body.gameObject );
		
		AutoDestory destroy = catchEffectBody.GetComponent<AutoDestory>();
		if (destroy != null )
			destroy.isDestroyOnAwake = false;
		catchEffectBody.SetActive( enableCatchEffect );

		Update();
		
	}

	public void DestoryCatchEffectBody()
	{
		if ( catchEffectBody == null )
			return;
		ParticleEmitter[] emitters = catchEffectBody.GetComponentsInChildren<ParticleEmitter>();
		foreach( ParticleEmitter emitter in emitters )
		{
			emitter.emit = false;
		}
		ParticleSystem[] particles = catchEffectBody.GetComponentsInChildren<ParticleSystem>();
		foreach( ParticleSystem particle in particles )
		{
			particle.enableEmission = false;
		}

		AutoDestory destroy = catchEffectBody.GetComponent<AutoDestory>();
		if (destroy != null )
			destroy.StartAutoDestory();
		catchEffectBody = null;
	}
}
