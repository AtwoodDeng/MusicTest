using UnityEngine;
using System.Collections;
using Holoville.HOTween;

[RequireComponent(typeof(TrailRenderer))]
public class Mouse : MonoBehaviour {

	TrailRenderer trailRender;
	bool ifDrag = false;

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
		trailRender = this.gameObject.GetComponent<TrailRenderer> ();
		if (trailRender == null)
		{
			Debug.Log ("No trail render");
			return;
		}
		trailRender.enabled = true;
		ifDrag = true;
	}
	public void DragOff()
	{
		trailRender.enabled = false;
		ifDrag = false;
	}
	public void Destory()
	{
		Invoke("DestoryFinnal",0.1f);
	}
	public void DestoryFinnal()
	{
		Destroy(this.gameObject);
	}
}
