using UnityEngine;
using System.Collections;
using System;

public class BInputManager : MonoBehaviour {

	public GameObject cursor;

	Camera mainCamera;


	// Update is called once per frame
	void Update () {

		DealMouse();

		DealCursor();

		if ( Input.GetKeyDown( KeyCode.Escape ))
		{
			BEventManager.Instance.PostEvent(EventDefine.OnFrontMenu , new MessageEventArgs() );
		}
	}

	public enum MouseState
	{
		Point,
		Drag,
		Free
	}
	public MouseState state;
	public enum CursorState
	{
		Free,
		PointCatch,
		PointCW,
		PointACW,
	}
	public CursorState cursorState;

	public Vector3 MousePosition
	{
		get{
			return mousePos;
		}
	}
	private Vector3 mousePos;
	public Vector3 mousePosGlobal{
		get{
			return cursor.transform.position;
		}
	}
	DateTime mouseStartTime;
	public float pointSenseTime = 0.2f;

	void DealCursor()
	{
		Screen.showCursor = false;

		CursorState oriState = cursorState;
		updateCursorState();

		if ( oriState != cursorState || cursor == null )
		{
			updateCursorObj();
		}

		updateCursorPos();
	}

	void updateCursorState()
	{
		if ( state == MouseState.Free )
			cursorState = CursorState.Free;
		if ( state == MouseState.Point || state == MouseState.Drag )
		{
			if ( cursorState == CursorState.Free )
			{
				if ( Input.GetMouseButton(Global.MouseLeftInt) )
				{
					cursorState = CursorState.PointCatch;
				}else if ( Input.GetMouseButton(Global.MouseRightInt) )
				{
					if ( BObjManager.Instance.BHeroBody.tempForceType == HeroHand.ForceType.SpinAntiCW )
					{
						cursorState = CursorState.PointCW;
					}else
					{
						cursorState = CursorState.PointACW;
					}
				}
			}
		}
	}

	void updateCursorObj()
	{
		if ( cursor != null )
			Destroy( cursor );
		GameObject prefab = Resources.Load(Global.CursorDict[cursorState.ToString()]) as GameObject;
		cursor = Instantiate( prefab ) as GameObject;

		updateCursorPos();
	}

	void updateCursorPos()
	{
		if (cursor != null )
			cursor.transform.localPosition = new Vector3( mousePos.x ,mousePos.y, Global.BCursorPosition.z);
	}

	void DealMouse()
	{
		//get the position of mouse, represent by (x,y)
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		Vector3 dir = ray.direction;
//		pos = dir / dir.z * Global.BstaticPosition.z;
		if ( mainCamera == null )
			mainCamera = Camera.main;
//		GUIDebug.add(ShowType.label , Screen.width + " " + Screen.height );

		mousePos = Input.mousePosition;
		mousePos.x -= Screen.width / 2;
		mousePos.y -= Screen.height / 2 ;
		mousePos.x *= mainCamera.orthographicSize / ( Screen.height / 2 ) ;
		mousePos.y *= mainCamera.orthographicSize / ( Screen.height / 2 ) ;
		mousePos += mainCamera.transform.position;
		mousePos.z = Global.BstaticPosition.z;



//		GUIDebug.add(ShowType.label , mousePos.ToString());


//		pos = Input.mousePosition;
//		GUIDebug.add( ShowType.label , Input.mousePosition.ToString() );
//		GUIDebug.add( ShowType.label , mousePos.ToString());
		
		state = MouseState.Free;
		
		if ( Input.GetMouseButtonDown(Global.MouseLeftInt) )
		{
			state = MouseState.Point;
			MessageEventArgs msg = new MessageEventArgs();
			msg.AddMessage("posX" , mousePos.x.ToString() );
			msg.AddMessage("posY" , mousePos.y.ToString() );
			msg.AddMessage("mousePos" , Global.V32Str( mousePos ) );
			msg.AddMessage("type" , Global.MouseLeft );
			msg.AddMessage("globalPos" , Global.V32Str( mousePosGlobal) );
			BEventManager.Instance.PostEvent(EventDefine.OnMouseClick , msg );
		}
		else if (Input.GetMouseButton(Global.MouseLeftInt))
		{
			DateTime tempTime = System.DateTime.Now;
			double mouseDownTime = (tempTime-mouseStartTime).TotalSeconds;
			if ( mouseDownTime > pointSenseTime )
			{
				state = MouseState.Drag;
			}else{
				state = MouseState.Point;
			}
		}
		else if (Input.GetMouseButtonUp(Global.MouseLeftInt))
		{
		}

		if ( Input.GetMouseButtonDown(Global.MouseRightInt) )
		{
			state = MouseState.Point;
			MessageEventArgs msg = new MessageEventArgs();
			msg.AddMessage("posX" , mousePos.x.ToString() );
			msg.AddMessage("posY" , mousePos.y.ToString() );
			msg.AddMessage("mousePos" , Global.V32Str( mousePos ) );
			msg.AddMessage("globalPos" , Global.V32Str( mousePosGlobal) );
			msg.AddMessage("type" , Global.MouseRight );
			BEventManager.Instance.PostEvent(EventDefine.OnMouseClick , msg );
		}
		else if (Input.GetMouseButton(Global.MouseRightInt))
		{
			DateTime tempTime = System.DateTime.Now;
			double mouseDownTime = (tempTime-mouseStartTime).TotalSeconds;
			if ( mouseDownTime > pointSenseTime )
			{
				state = MouseState.Drag;
			}else{
				state = MouseState.Point;
			}
		}
		else if (Input.GetMouseButtonUp(Global.MouseRightInt))
		{
		}
//		GUIDebug.add (ShowType.label, "MouseState " + state);
	}

	
	public BInputManager() { s_Instance = this; }
	public static BInputManager Instance { get { return s_Instance; } }
	private static BInputManager s_Instance;

}
