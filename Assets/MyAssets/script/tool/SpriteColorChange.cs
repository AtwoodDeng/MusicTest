using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class SpriteColorChange : MonoBehaviour {

	public float delay;
	public float fadeInTime;
	public EaseType easeType;
	public bool ifStartOnAwake;
	public Color fromColor;
	public Color toColor;

	public tk2dSprite sprite;
	public tk2dTextMesh text;
	
	// Use this for initialization
	void Awake () {
		sprite = GetComponent<tk2dSprite>();
		if ( sprite == null )
		{
			text = GetComponent<tk2dTextMesh>();
			if ( text == null )
			{
				enabled = false;
				ifStartOnAwake = false;
			}
		}

			if ( ifStartOnAwake )
				StartPlay();
	}
	
	public void StartPlay() {
		if ( sprite != null )
		{
		sprite.color = fromColor;
		HOTween.To( sprite ,
		           fadeInTime ,
		           "color" ,
		           toColor ,
		           false ,
		           easeType ,
		           delay );
		}
		else if ( text != null )
		{
			text.color = fromColor;
			HOTween.To( text ,
			           fadeInTime ,
			           "color" ,
			           toColor ,
			           false ,
			           easeType ,
			           delay );
		}
		
	}
}
