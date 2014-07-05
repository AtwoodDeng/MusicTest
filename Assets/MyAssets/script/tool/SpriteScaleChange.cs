using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class SpriteScaleChange : MonoBehaviour {

	public float delay;
	public float fallInTime;
	public EaseType easeType;
	public bool ifStartOnAwake;
	public Vector3 fromScale;
	public Vector3 toScale;
	public tk2dSprite sprite;

	// Use this for initialization
	void Awake () {
		sprite = GetComponent<tk2dSprite>();
		if ( sprite == null )
		{
			Debug.Log("Cannot find sprite");
			return;
		}
		if ( toScale == Vector3.zero )
			toScale = sprite.scale;
		if ( ifStartOnAwake )
			StartPlay();
	}

	public void StartPlay() {
		sprite.scale = fromScale;
		HOTween.To( sprite ,
		           fallInTime ,
		           "scale" ,
		           toScale ,
		           false ,
		           easeType ,
		           delay );

	}
}
