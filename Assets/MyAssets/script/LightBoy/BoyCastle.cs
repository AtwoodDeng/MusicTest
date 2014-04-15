using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class BoyCastle : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Collider> ().isTrigger = true;
	}
	
	void OnTriggerEnter(Collider other)
	{
		//Debug.Log ("enter" + other.gameObject.name);
		if (other.gameObject.tag == "BoyFoot")
			LevelManager.instance.EnterCastle ();
	}
	void OnTriggerExit( Collider other)
	{
		//Debug.Log ("exit" + other.gameObject.name);
		if (other.gameObject.tag == "BoyFoot")
			LevelManager.instance.EnterCastle ();
	}
}
