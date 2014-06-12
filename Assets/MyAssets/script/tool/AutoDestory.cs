using UnityEngine;
using System.Collections;

public class AutoDestory : MonoBehaviour {

	public float destroyTime = 1f;

	public bool isDestroyOnAwake = true;

	// Use this for initialization
	void Awake () {
		if ( isDestroyOnAwake )
			Invoke("DestroyMySelf" , destroyTime );
	}

	public void StartAutoDestory()
	{
		Invoke("DestroyMySelf" , destroyTime );
	}

	void DestroyMySelf(){
		Destroy(this.gameObject);
	}
}
