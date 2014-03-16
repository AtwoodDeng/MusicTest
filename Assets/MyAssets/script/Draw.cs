using UnityEngine;
using System.Collections;

public class Draw : MonoBehaviour {

	private Vector2 dragStart;
	private Vector2 dragEnd;
	private Vector2 preDrag;

	private Vector2 mouse;


	public Drawable drawPanel;

	int zoom = 1; 

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Texture2D tex = drawPanel.texture;
		//Rect imgRect = new Rect (100 , 20 , tex.width * zoom, tex.height * zoom);
		Rect imgRect = new Rect( 100 , 20 , tex.width * zoom , tex.height * zoom );
		//Vector2 mouse = Input.mousePosition;
		mouse = Input.mousePosition;
		mouse.y = Screen.height - mouse.y;


		if ( Input.GetMouseButtonDown( 0 ) )
		{
			if ( imgRect.Contains(mouse) ) {
				dragStart = screenToImage( mouse , imgRect , zoom );
				dragEnd  = screenToImage( mouse , imgRect , zoom);

//				dragStart = mouse - Vector2( imgRect.x , imgRect.y );
//				dragStart.y = imgRect.height - dragStart.y;
//
//				dragStart.x = Mathf.Round( dragStart.x / zoom );
//				dragStart.y = Mathf.Round( dragStart.y / zoom );
//
//				dragEnd = mouse - Vector2( imgRect.x , imgRect.y );
//				dragEnd.x = Mathf.Clamp( dragEnd.x , 0 , imgRect.width );
//				dragEnd.y = imgRect.height - Mathf.Clamp( dragEnd.y , 0 , imgRect.height );
//				
//				dragEnd.x = Mathf.Round( dragEnd.x / zoom );
//				dragEnd.y = Mathf.Round( dragEnd.y / zoom );
			}else
			{
				dragStart = Vector2.zero;
			}
		}

		if ( Input.GetMouseButton( 0 ) )
		{
			
			//Debug.Log( "mouse on " );
			if ( dragStart == Vector2.zero )
				return;
			dragEnd = screenToImage( mouse , imgRect , zoom );
			drawPanel.Draw( dragEnd , preDrag );
			//Debug.Log("draw " + dragEnd.ToString() + " " + preDrag.ToString());
		}

		if ( Input.GetMouseButtonUp( 0 ) )
		{
			
			//Debug.Log( "mouse up" );

			dragStart = Vector2.zero;
			dragEnd = Vector2.zero;
		}
		preDrag = dragEnd;

	}

	Vector2 screenToImage ( Vector2 screenPoint , Rect imgRect , int zoom )
	{
		Vector2 res;
		res = screenPoint - new Vector2( imgRect.x , imgRect.y );
		res.x = Mathf.Clamp( res.x , 0 , imgRect.width );
		res.y = imgRect.height - Mathf.Clamp( res.y , 0 , imgRect.height );
		
		res.x = Mathf.Round( res.x / zoom );
		res.y = Mathf.Round( res.y / zoom );
		return res;
	}
	
	void OnGUI(){

		Texture2D tex = drawPanel.texture;
		GUI.DrawTexture (new Rect( 100 , 20 , tex.width * zoom , tex.height * zoom ) ,tex);

		GUILayout.TextField ("dragStart " + dragStart.ToString ());
		GUILayout.TextField ("dragEnd " + dragEnd.ToString ());
		GUILayout.TextField ("preDrag " + dragStart.ToString ());
		GUILayout.TextField ("mouse " + mouse.ToString ());
	}
}
