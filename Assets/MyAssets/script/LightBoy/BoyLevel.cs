using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class BoyLevel : MonoBehaviour {

	public List<BoyCastle> castles = new List<BoyCastle>();
	public List<BoyFloor> floors = new List<BoyFloor>();
	public GameObject start;

	public void FadeIn()
	{
	}

	public void FadeOut ()
	{
		//TODO
		//DestoryFinnal ();
		gameObject.BroadcastMessage ("Destory", this.gameObject , SendMessageOptions.DontRequireReceiver);

	}
	public void DestoryFinnal()
	{
		Destroy(this.gameObject);
	}
}
