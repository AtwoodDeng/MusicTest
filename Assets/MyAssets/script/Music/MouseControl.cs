using UnityEngine;
using System.Collections;
using System;

public class MouseControl : MonoBehaviour {

	public GameObject mouseEffectPrefab;
	public GameObject mouseTriggerPrefab;
	public GameObject EffectPrefab;
	private GameObject mouseEffectObj;
	private Mouse mouseEffect;
	private MouseTrigger mouseTrigger;
	private GameObject mouse;
	private GameObject mouseTriggerObj;


	public float PointIntense;
	public float DragIntense;


	public enum MouseState
	{
		Point,
		Drag,
		Free
	}

	MouseState state;

	DateTime mouseStartTime;
	public float pointSenseTime = 0.2f;

	Vector3 startPos;
	Vector3 tempPos;



	// Use this for initialization
	void Start () {
		mouseTriggerObj = (GameObject) Instantiate (mouseTriggerPrefab);
		mouseTrigger = mouseTriggerObj.GetComponent<MouseTrigger> ();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 dir = ray.direction;
		Vector3 pos = dir / dir.z * AudioManager.staticZ;

		if (mouseEffectObj)
			mouseEffectObj.transform.localPosition = pos;
		if (mouseTriggerObj)
			mouseTriggerObj.transform.localPosition = pos;

		if ( Input.GetMouseButtonDown(0) )
		{
			mouseStartTime = System.DateTime.Now;
			startPos = pos;
			state = MouseState.Point;
			mouseEffectObj = (GameObject)Instantiate(mouseEffectPrefab);
			mouseEffect = mouseEffectObj.GetComponent<Mouse>();

		}
		if (Input.GetMouseButton(0))
		{
			tempPos = pos;
			if ( mouseEffect != null )
			{
				DateTime tempTime = System.DateTime.Now;
				double mouseDownTime = (tempTime-mouseStartTime).TotalSeconds;
				if ( mouseDownTime > pointSenseTime )
				{
					state = MouseState.Drag;
					mouseEffect.DragOn();
					mouseTrigger.EachBall( Drag );
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if ( mouseEffect != null )
			{
				mouseEffect.Destory();
			}
			mouseEffect = null;
			if ( state == MouseState.Point )
			{
				mouseTrigger.EachBall( Point );
				GameObject effect = (GameObject)Instantiate( EffectPrefab );
				effect.transform.parent = this.gameObject.transform;
				effect.transform.localPosition = startPos;
			}

			state = MouseState.Free;

		}
	}

	public void Point( GameObject obj )
	{
		Vector3 pointToBall = obj.transform.position - tempPos;
		Rigidbody rig = obj.GetComponent<Rigidbody> ();
		if ( rig == null) return;
		//Debug.Log ("point");
		rig.AddForce( pointToBall / pointToBall.magnitude 
		             // * AudioManager.instance.getFadeValue() 
		             * PointIntense , ForceMode.Impulse );
		BallAI ballAI = obj.GetComponent<BallAI> ();
		if ( ballAI == null ) return;
		ballAI.stopParticle ();
	}

	public void Drag( GameObject obj )
	{
		
		Vector3 ballToPoint = - obj.transform.position + tempPos;
		Rigidbody rig = obj.GetComponent<Rigidbody> ();
		if ( rig == null ) 
			return;
		//Debug.Log ("drag");
		rig.AddForce( ballToPoint / ballToPoint.magnitude 
		             //* AudioManager.instance.getFadeValue() 
		             * DragIntense , ForceMode.Impulse );
		BallAI ballAI = obj.GetComponent<BallAI> ();
		if ( ballAI == null ) return;
		ballAI.stopParticle ();
	}
}
