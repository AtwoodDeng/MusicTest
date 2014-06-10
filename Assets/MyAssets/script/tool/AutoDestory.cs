using UnityEngine;
using System.Collections;

public class AutoDestory : MonoBehaviour {

	public float destroyTime = 1f;

	// Use this for initialization
	void Awake () {
		Invoke("DestroyMySelf" , destroyTime );
	}

	void DestroyMySelf(){
		Destroy(this.gameObject);
	}
}
