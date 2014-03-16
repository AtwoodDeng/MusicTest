using UnityEngine;
using System.Collections;
using System;

public class Drawing : MonoBehaviour {


	static public Texture2D PaintLine ( Vector2 from , Vector2 to 
	                              , float radius , Color col 
	                              , float hardness, Texture2D tex )
	{
		float width = radius * 2;
		float extent = radius;

		int stY = Convert.ToInt32( Mathf.Clamp (Mathf.Min (from.y, to.y) - extent, 0, tex.height));
		int stX = Convert.ToInt32( Mathf.Clamp (Mathf.Min (from.x, to.x) - extent, 0, tex.width)); 
		int endY = Convert.ToInt32( Mathf.Clamp (Mathf.Max (from.y, to.y) + extent, 0, tex.height));
		int endX = Convert.ToInt32( Mathf.Clamp (Mathf.Max (from.x, to.x) + extent, 0, tex.width)); 

		//Debug.Log ("from " + from.ToString () + " to " + to.ToString ());
		//Debug.Log (" stX " + stX + " stY " + stY + " endX " + endX + " endY " + endY);

		//Debug.Log ("color " + col);

		int lengthX = endX - stX;
		int lengthY = endY - stY;

		float sqrRad = radius * radius;
		float sqrRad2 = (radius + 1) * (radius + 1);
		Color[] pixels = tex.GetPixels( stX, stY,lengthX,lengthY,0);
		Vector2 start = new Vector2( stX , stY );
		for ( int y = 0 ; y < lengthY ; y++ )
		{
			for( int x = 0 ; x < lengthX ; x++ )
			{
				Vector2 p = new Vector2( x , y ) + start;
				Vector3 center = new Vector3( p.x + 0.5f , p.y + 0.5f , 0f );
				float dist = ( center - NearestPointStrict( from , to , center) ).sqrMagnitude;
				if ( dist > sqrRad2 ) 
				{
					continue;
				}
				dist = GaussFalloff( Mathf.Sqrt(dist),radius) * hardness;
				Color c;
				if ( dist > 0 )
				{
					c = Color.Lerp( pixels[y*lengthX+x] , col  , dist );
				}else {
					c = pixels[ y * lengthX + x ];
				}
				pixels[ y  * lengthX + x] = c;
			}

		}
		tex.SetPixels ( stX , stY , lengthX, lengthY, pixels, 0);
		tex.Apply ();
		return tex;
	}

	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 fullDirection = lineEnd-lineStart;
		Vector3 lineDirection = Vector3.Normalize(fullDirection);
		float closestPoint = Vector3.Dot((point-lineStart),lineDirection)/Vector3.Dot(lineDirection,lineDirection);
		return lineStart+(Mathf.Clamp(closestPoint,0.0f,Vector3.Magnitude(fullDirection))*lineDirection);
	}

	public static float GaussFalloff(float distance, float inRadius)
	{
		return Mathf.Clamp01(Mathf.Pow(360f, -Mathf.Pow(distance / inRadius, 2.5f) - 0.01f));
	}

}
