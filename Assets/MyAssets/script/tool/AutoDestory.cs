using UnityEngine;
using System.Collections;

public class AutoDestory : MonoBehaviour {

	public float destroyTime = -1;

	public bool isDestroyOnAwake = true;

	// Use this for initialization
	void Awake () {
		if ( isDestroyOnAwake && destroyTime > 0 )
			Invoke("DestroyMySelf" , destroyTime );
	}

	public void StartAutoDestory()
	{
		if ( destroyTime > 0 )
			Invoke("DestroyMySelf" , destroyTime );
	}

	void DestroyMySelf(){
		Destroy(this.gameObject);
	}
}
