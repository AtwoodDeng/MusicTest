using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class MouseTrigger : MonoBehaviour {

	public List<GameObject> ballList = new List<GameObject>();
	public List<GameObject> catchList = new List<GameObject>();

	public delegate void Deal( GameObject obj );

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)  {
		//Debug.Log ("enter " + other.gameObject.name);
		catchList.Add (other.gameObject);
		if ( other.gameObject.GetComponent<BallAI>() != null )
			ballList.Add (other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		//Debug.Log ("Exit " + other.gameObject.name);
		catchList.Remove (other.gameObject);
		if (ballList.Contains (other.gameObject))
			ballList.Remove (other.gameObject);
	}


	public void EachBall( Deal deal )
	{
		foreach (GameObject ball in ballList)
		{
			deal (ball);
		}
	}

	public void EachCatch( Deal deal )
	{
		foreach (GameObject cat in catchList)
		{
			deal (cat);
		}
	}

}
