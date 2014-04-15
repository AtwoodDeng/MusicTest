using UnityEngine;
using System.Collections;
using System;



public class MouseControl : MonoBehaviour {

	public GameObject mouseEffectPrefab;
	public GameObject mouseTriggerPrefab;
	public GameObject EffectPrefab;

	public GameObject mouseEffectObj;
	private Mouse mouseEffect;
	private MouseTrigger mouseTrigger;
	private GameObject mouse;
	private GameObject mouseTriggerObj;

	public MyLightColor lightColor;


	public float PointIntense;
	public float DragIntense;

	static public MouseControl instance;

	public enum MouseState
	{
		Point,
		Drag,
		Free
	}
	public MouseState state;

	public enum MouseFollowMethod
	{
		Immediately,
		Delay,
		LimitSpeed
	}
	public MouseFollowMethod followMethod;
	public float delayIntense = 0.5f;
	public float limitSpeed = 0.1f;

	DateTime mouseStartTime;
	public float pointSenseTime = 0.2f;

	public Vector3 startPos;
	public Vector3 tempPos;
	public Vector3 pos;



	// Use this for initialization
	void Start () {
		mouseTriggerObj = (GameObject) Instantiate (mouseTriggerPrefab);
		mouseTrigger = mouseTriggerObj.GetComponent<MouseTrigger> ();
		if (instance == null)
						instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 dir = ray.direction;
		 pos = dir / dir.z * AudioManager.staticZ;

		if ( mouseEffectObj)
		{
		if ( followMethod == MouseFollowMethod.Immediately)
		{
			if ( state == MouseState.Drag || state == MouseState.Point)
				mouseEffectObj.transform.localPosition = pos;
		}else if( followMethod == MouseFollowMethod.Delay )
		{
			if ( state == MouseState.Drag || state == MouseState.Point )
			if ( mouseEffectObj.transform.localPosition == Vector3.zero )
			{
				mouseEffectObj.transform.localPosition = pos;
			}else
			{
				mouseEffectObj.transform.localPosition = 
					delayIntense * mouseEffectObj.transform.localPosition
						+ ( 1 - delayIntense ) * pos;
			}

		}else if ( followMethod == MouseFollowMethod.LimitSpeed )
		{
			if ( state == MouseState.Drag || state == MouseState.Point)
			{
				Vector3 toward = - mouseEffect.transform.localPosition + pos;
				if ( toward.magnitude > limitSpeed )
					toward = toward.normalized * limitSpeed;
				mouseEffectObj.transform.localPosition =
					mouseEffectObj.transform.localPosition + toward;
			}
		}
		}

		if (mouseTriggerObj)
			mouseTriggerObj.transform.localPosition = pos;

		state = MouseState.Free;

		if ( Input.GetMouseButtonDown(0) )
		{
			mouseStartTime = System.DateTime.Now;
			startPos = pos;
			state = MouseState.Point;
			if ( mouseEffectObj != null )
			{
//				if ( mouseEffectObj.GetComponent<Mouse>() != null )
//					mouseEffectObj.GetComponent<Mouse>().Destory();
//				else
//					GameObject.Destroy(mouseEffectObj);
			}else{
				mouseEffectObj = (GameObject)Instantiate(mouseEffectPrefab);
				mouseEffect = mouseEffectObj.GetComponent<Mouse>();
				mouseEffectObj.transform.parent = transform;
				mouseEffectObj.transform.localPosition = Vector3.zero;
				ChangeColorTo( lightColor );
			}
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

				}else{
					state = MouseState.Point;
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if ( mouseEffect != null && mouseEffectObj.GetComponent<Mouse>() != null )
			{
				if ( mouseEffectObj.GetComponent<PaperLight>() != null )
					mouseEffectObj.GetComponent<PaperLight>().Destory();
				else 
					mouseEffectObj.GetComponent<Mouse>().Destory();
			}
			mouseEffect = null;
			mouseEffectObj = null;
			if ( state == MouseState.Point )
			{
				mouseTrigger.EachBall( Point );
				GameObject effect = (GameObject)Instantiate( EffectPrefab );

				effect.transform.parent = this.gameObject.transform;
				effect.transform.localPosition = startPos;
			}

		}
		GUIDebug.add (ShowType.label, "MouseState " + state);
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

	public void ChangeColorTo( MyLightColor col )
	{
		//Debug.Log ("Mouse control Change color" + col.ToString ());
		lightColor = col;
		if ( mouseEffectObj != null )
		{
			PaperLight light = mouseEffectObj.GetComponent<PaperLight> ();
			light.ChangeColorTo (col);
		}else
			Debug.Log("Nothing to change ");
	}

	public void  OnLoopEnd()
	{
		ChangeColorTo (MyLightColor.None);
	}

}
