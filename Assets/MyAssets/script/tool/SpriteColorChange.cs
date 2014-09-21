using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class SpriteColorChange : MonoBehaviour {

	public float delay;
	public float fadeTime;
	public EaseType easeType;
	public bool ifStartOnAwake;
	public Color fromColor;
	public Color toColor;
	public bool ifInitColor;

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
			if ( ifInitColor )
				sprite.color = fromColor;
			HOTween.To( sprite ,
			           fadeTime ,
			           "color" ,
			           toColor ,
			           false ,
			           easeType ,
			           delay );
		}
		else if ( text != null )
		{
			if ( ifInitColor )
				text.color = fromColor;
			HOTween.To( text ,
			           fadeTime ,
			           "color" ,
			           toColor ,
			           false ,
			           easeType ,
			           delay );
		}
		
	}
}
