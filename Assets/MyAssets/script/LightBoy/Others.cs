using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Others : Boy {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Collider> ().isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if ( other.gameObject.tag == "BoyBlock" )
		{
			BoyManager.instance.othersList.Add(this);
		}
	}
	void OnTriggerExit(Collider other) {
		if ( other.gameObject.tag == "BoyBlock" )
		{
			BoyManager.instance.othersList.Remove(this);
		}
	}
}
