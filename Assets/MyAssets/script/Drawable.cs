using UnityEngine;
using System.Collections;

public class Drawable : MonoBehaviour {
	
	public Texture2D texture;
	public int width;
	public int  height;

	public float radius;
	public Color col;
	public float hardness;

	void Start() {
		if (texture == null)
			texture = new Texture2D( width , height );
	}

	// Update is called once per frame
	void Update () {

	}

	public void Draw( Vector2 dragEnd , Vector2 preDrag )
	{
		//Debug.Log ("draw " + dragEnd.ToString () + " " + preDrag.ToString ());
		drawSimple (dragEnd, preDrag);
	}

	void drawSimple( Vector2 from , Vector2 to )
	{
		if (to == Vector2.zero)
			to = from;
		texture = Drawing.PaintLine (from, to, radius, col, hardness, texture);
		texture.Apply ();
	}


}
