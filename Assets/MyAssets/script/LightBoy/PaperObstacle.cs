using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class PaperObstacle : MonoBehaviour {

	public float fadeInTime = 2.5f;
	public float fadeOutTime = 2.5f;

	public float fadeInDistance = 20f;
	public float fadeOutDistance = 40f;

	// Use this for initialization
	void Start () {
		Vector3 toPos = transform.localPosition;
		transform.localPosition += new Vector3 (0, 0, -fadeInDistance);
		HOTween.To (transform, Random.Range( fadeInTime-1f,fadeInTime+1f) , new TweenParms ().Prop("localPosition", toPos).Ease (EaseType.EaseOutCubic).Delay(Random.Range(0f,1.5f)));

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Destory( GameObject sender ) {
		//Debug.Log ("obstacle destory");
		Vector3 toPos = transform.localPosition + new Vector3( 0 , 0 , fadeOutDistance );
		HOTween.To (transform, Random.Range( fadeOutTime-1f,fadeOutTime+1f)  , new TweenParms ().Prop("localPosition", toPos).Ease (EaseType.EaseOutCubic).Delay(Random.Range(0f,0.5f)));
	}

	void DestoryFinnal(){

	}
}
