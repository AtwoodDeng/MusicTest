using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	public GameObject target;
	public float followRate = 0.85f;

	public bool onlyX = false;
	public bool onlyY = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if ( target != null )
			FollowTo( target.transform.position );
	}

	void FollowTo( Vector3 pos )
	{
		Vector3 toward = pos - transform.position;
		toward.z = 0;
		if ( onlyX )
			toward.y = 0;
		if ( onlyY )
			toward.x = 0;
		transform.position = transform.position + ( 1 - followRate ) * toward;
	}
}
