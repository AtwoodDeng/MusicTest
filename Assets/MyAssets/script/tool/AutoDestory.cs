using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class AutoDestory : MonoBehaviour {

	public float destroyTime = -1;

	public bool isDestroyOnAwake = true;
	public bool isFadeOut = false;

	// Use this for initialization
	void Awake () {
		if ( isDestroyOnAwake && destroyTime > 0 )
			Invoke("DestroyMySelf" , destroyTime );
	}

	public void StartAutoDestory()
	{
		if ( destroyTime > 0 )
		{
			Invoke("DestroyMySelf" , destroyTime );
			if ( isFadeOut ){
				if ( GetComponent<tk2dSprite>() != null){
					tk2dSprite spirte =  GetComponent<tk2dSprite>();

					UnityEngine.Color toColor = spirte.color;
					toColor.a = 0;
					HOTween.To( spirte,
					           destroyTime - 0.05f,
					           "color",
					           toColor);
				}
			}
		}
	}

	void DestroyMySelf(){
		Destroy(this.gameObject);
	}
}
