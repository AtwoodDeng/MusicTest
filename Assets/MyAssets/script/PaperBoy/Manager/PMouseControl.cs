using UnityEngine;
using System.Collections;
using System;

/**
 * PMouseControl
 * Function:
 * 1. getMousePos() 
 *        return the position of the mouse
 * 2. Update()
 * 		  update the visual effect of the mouse
 */

public class PMouseControl : MonoBehaviour {
	
	static public PMouseControl instance;

	public GameObject mousePrefab;
	
	public GameObject mouseObj;
	
	public enum MouseState
	{
		Point,
		Drag,
		Free
	}
	public MouseState state;
	
	DateTime mouseStartTime;
	public float pointSenseTime = 0.2f;
	
	public Vector3 startPos;
	public Vector3 tempPos;
	public Vector3 pos;


	static public Vector3 getMousePos()
	{
		if (instance != null)
			return instance.pos;
		return Vector3.one * 99999f;
	}

	static public MouseState getMouseState()
	{
		if (instance != null)
			return instance.state;
		return MouseState.Free;
	}
	
	
	// Use this for initialization
	void Start () {
		//set the instance of Mouse Control
		if (instance == null)
			instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		//get the position of mouse, represent by (x,y)
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 dir = ray.direction;
		pos = dir / dir.z * AudioManager.staticZ;
		
		if ( mouseObj)
		{
			mouseObj.transform.localPosition = pos;
//			//set the position of mouse
//			if ( followMethod == MouseFollowMethod.Immediately)
//			{
//				if ( state == MouseState.Drag || state == MouseState.Point)
//					mouseObj.transform.localPosition = pos;
//			}else if( followMethod == MouseFollowMethod.Delay )
//			{
//				if ( state == MouseState.Drag || state == MouseState.Point )
//					if ( mouseObj.transform.localPosition == Vector3.zero )
//				{
//					mouseObj.transform.localPosition = pos;
//				}else
//				{
//					mouseObj.transform.localPosition = 
//						delayIntense * mouseObj.transform.localPosition
//							+ ( 1 - delayIntense ) * pos;
//				}
//				
//			}else if ( followMethod == MouseFollowMethod.LimitSpeed )
//			{
//				if ( state == MouseState.Drag || state == MouseState.Point)
//				{
//					Vector3 toward = - mouseEffect.transform.localPosition + pos;
//					if ( toward.magnitude > limitSpeed )
//						toward = toward.normalized * limitSpeed;
//					mouseObj.transform.localPosition =
//						mouseObj.transform.localPosition + toward;
//				}
//			}
		}
		
//		if (mouseTriggerObj)
//			mouseTriggerObj.transform.localPosition = pos;
		
		state = MouseState.Free;
		
		if ( Input.GetMouseButtonDown(0) )
		{
			mouseStartTime = System.DateTime.Now;
			startPos = pos;
			state = MouseState.Point;
			if ( mouseObj == null )
			{
				mouseObj = (GameObject)Instantiate(mousePrefab);
				mouseObj.transform.parent = transform;
				mouseObj.transform.localPosition = Vector3.zero;
			}
		}
		else if (Input.GetMouseButton(0))
		{
			tempPos = pos;
//			if ( lightOnMouse != null )
//			{
//				DateTime tempTime = System.DateTime.Now;
//				double mouseDownTime = (tempTime-mouseStartTime).TotalSeconds;
//				if ( mouseDownTime > pointSenseTime )
//				{
//					state = MouseState.Drag;
//				}else{
//					state = MouseState.Point;
//				}
//			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
//			if ( lightOnMouse != null )
//			{
//				lightOnMouse.Destory();
//			}
//			lightOnMouse = null;
//			mouseObj = null;
		}
//		GUIDebug.add (ShowType.label, "MouseState " + state);
	}
	
//	public void Point( GameObject obj )
//	{
//		Vector3 pointToBall = obj.transform.position - tempPos;
//		Rigidbody rig = obj.GetComponent<Rigidbody> ();
//		if ( rig == null) return;
//		//Debug.Log ("point");
//		rig.AddForce( pointToBall / pointToBall.magnitude 
//		             // * AudioManager.instance.getFadeValue() 
//		             * PointIntense , ForceMode.Impulse );
//		BallAI ballAI = obj.GetComponent<BallAI> ();
//		if ( ballAI == null ) return;
//		ballAI.stopParticle ();
//	}
//	
//	public void Drag( GameObject obj )
//	{
//		
//		Vector3 ballToPoint = - obj.transform.position + tempPos;
//		Rigidbody rig = obj.GetComponent<Rigidbody> ();
//		if ( rig == null ) 
//			return;
//		//Debug.Log ("drag");
//		rig.AddForce( ballToPoint / ballToPoint.magnitude 
//		             //* AudioManager.instance.getFadeValue() 
//		             * DragIntense , ForceMode.Impulse );
//		BallAI ballAI = obj.GetComponent<BallAI> ();
//		if ( ballAI == null ) return;
//		ballAI.stopParticle ();
//	}
//	
//	public void ChangeColorTo( MyLightColor col )
//	{
//		//Debug.Log ("Mouse control Change color" + col.ToString ());
//		lightColor = col;
//		if ( mouseObj != null )
//		{
//			PaperLight light = mouseObj.GetComponent<PaperLight> ();
//			light.ChangeColorTo (col);
//		}else
//			Debug.Log("Nothing to change ");
//	}
	
//	public void  OnLoopEnd()
//	{
//		ChangeColorTo (MyLightColor.None);
//	}
	
}
