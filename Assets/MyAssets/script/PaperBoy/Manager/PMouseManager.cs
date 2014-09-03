using UnityEngine;
using System.Collections;
using System;

public class PMouseManager : MonoBehaviour {
	
	static public PMouseManager instance;
	static public float staticZ = 50f;
	
	public GameObject mousePrefab;
	public PMouse mouse;
	
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
	private Vector3 pos;
	
	static public Vector3 getMousePos()
	{
		if (instance != null)
			return instance.tempPos;
		return Vector3.one * 99999f;
	}
	
	
	// Use this for initialization
	void Start () {
		//set the instance of Mouse Manager
		if (instance == null)
			instance = this;
		state = MouseState.Free;
	}


	// Update is called once per frame
	void Update () {
		//get the position of mouse, represent by (x,y)
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 dir = ray.direction;
		pos = dir / dir.z * staticZ;
		tempPos = pos;

//		GUIDebug.add (ShowType.label, "temp pos : " + pos);

		if ( mouse )
		{
			mouse.transform.localPosition = pos;
			mouse.gameObject.SetActive(true);
		}
		state = MouseState.Free;
		
		if ( Input.GetMouseButtonDown(0) )
		{
			mouseStartTime = System.DateTime.Now;
			startPos = pos;
			state = MouseState.Point;
			if ( mouse == null )
			{
				CreateNewMouse();
			}
		}
		else if (Input.GetMouseButton(0))
		{
			tempPos = pos;
			DateTime tempTime = System.DateTime.Now;
			double mouseDownTime = (tempTime-mouseStartTime).TotalSeconds;
			if ( mouseDownTime > pointSenseTime )
			{
				state = MouseState.Drag;
			}else{
				state = MouseState.Point;
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			if ( mouse != null )
				mouse.Destroy( Global.MouseDestroyTime );
			mouse = null;
		}
		GUIDebug.add (ShowType.label, "MouseState " + state);
	}

	PMouse CreateNewMouse()
	{
		if ( mouse == null )
		{
			if ( mousePrefab != null )
			{
				mouse = ((GameObject)Instantiate(mousePrefab)).GetComponent<PMouse>();
				mouse.transform.parent = this.transform;
				mouse.gameObject.SetActive(false);
			}

		}
		return mouse;
	}

}
