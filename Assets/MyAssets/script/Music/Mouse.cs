using UnityEngine;
using System.Collections;
using Holoville.HOTween;

[RequireComponent(typeof(TrailRenderer))]
public class Mouse : MonoBehaviour {

	TrailRenderer trailRender;
	public ParticleSystem particle;

	// Use this for initialization
	void Start () {
		trailRender = this.gameObject.GetComponent<TrailRenderer> ();
		if (trailRender == null)
						Debug.Log ("No trail render");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DragOn()
	{
		//Debug.Log ("drag on");
		trailRender = this.gameObject.GetComponent<TrailRenderer> ();
		if (trailRender == null)
		{
			return;
		}
		trailRender.enabled = true;
	}
	public void DragOff()
	{
		trailRender.enabled = false;
	}
	public void Destory()
	{
		//.Log ("Destory mouse");
		float trailTime = 0f;
		float parTime = 0f;
		if ( particle != null )
		{
			parTime = particle.startLifetime;
			particle.enableEmission = false;
		}
		if ( trailRender != null && trailRender.enabled )
			trailTime = trailRender.time;

		float time = Mathf.Max (parTime, trailTime);
		Invoke("DestoryFinnal", time );
	}
	public void DestoryFinnal()
	{
		this.gameObject.transform.position = new Vector3 (99999f, 999999f, 99999f);
		this.gameObject.SetActive (false);
	}
}
