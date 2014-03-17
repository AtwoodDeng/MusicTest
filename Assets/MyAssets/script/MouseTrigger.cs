using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class MouseTrigger : MonoBehaviour {

	public List<GameObject> ballList;

	public delegate void DealBall( GameObject obj );

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)  {
		if ( other.gameObject.GetComponent<BallAI>() != null )
			ballList.Add (other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		if (ballList.Contains (other.gameObject))
						ballList.Remove (other.gameObject);
	}


	public void EachBall( DealBall deal )
	{
		foreach (GameObject ball in ballList)
		{
			//Debug.Log("Deal Ball");
						deal (ball);
		}
	}

}
