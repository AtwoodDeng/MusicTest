using UnityEngine;
using System.Collections;
using System;

public class MouseControl : MonoBehaviour {

	public float staticZ = 15f;
	public GameObject MousePrefab;
	private GameObject followObj;
	private Mouse mouse;
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

	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 dir = ray.direction;
		Vector3 pos = dir / dir.z * staticZ;

		if (followObj)
			followObj.transform.localPosition = pos;

		if ( Input.GetMouseButtonDown(0) )
		{
			mouseStartTime = System.DateTime.Now;
			startPos = pos;
			state = MouseState.Point;
			followObj = (GameObject)Instantiate(MousePrefab);
			mouse = followObj.GetComponent<Mouse>();

		}
		if (Input.GetMouseButton(0))
		{
			tempPos = pos;
			DateTime tempTime = System.DateTime.Now;
			double mouseDownTime = (tempTime-mouseStartTime).TotalSeconds;
			if ( mouseDownTime > pointSenseTime )
				mouse.DragOn();
			state = MouseState.Drag;
		}
		if (Input.GetMouseButtonUp(0))
		{
			mouse.Destory();
			state = MouseState.Free;

		}
	}


}
