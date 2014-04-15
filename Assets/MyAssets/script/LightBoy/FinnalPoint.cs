using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class FinnalPoint : MonoBehaviour {

	
	public ParticleSystem main;
	public ParticleSystem flare;
	public ParticleSystem trailCylinder;
	public Light light;

	// Use this for initialization
	void Start () {
		float initIntensity = light.intensity;
		HOTween.To (light, Random.Range(4f , 10f ), new TweenParms ().Prop ("intensity", 0.5f * initIntensity)
		           .Loops (99, LoopType.Yoyo).Ease (EaseType.EaseInBack));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Destory( GameObject sender ){
		main.enableEmission = false;
		flare.enableEmission = false;
		trailCylinder.enableEmission = false;
		if ( collider != null )
			collider.enabled = false;
		HOTween.To (light, main.startLifetime, new TweenParms ().Prop ("intensity", 0).Ease (EaseType.EaseInOutQuad));
		Invoke ("DestoryFinnal", main.startLifetime);
	}
	
	void DestoryFinnal(){
		transform.position = new Vector3 (9999f, 9999f, 9999f);
		gameObject.SetActive (false);
	}
}
