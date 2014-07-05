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
	
	// Use this for initialization
	void Awake () {
		sprite = GetComponent<tk2dSprite>();

		if ( ifStartOnAwake )
			StartPlay();
	}
	
	public void StartPlay() {
		if ( sprite == null )
			return;
		sprite.color = fromColor;
		HOTween.To( sprite ,
		           fadeInTime ,
		           "color" ,
		           toColor ,
		           false ,
		           easeType ,
		           delay );
		
	}
}
