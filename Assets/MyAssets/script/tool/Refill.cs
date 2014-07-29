using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Refill : MonoBehaviour {

	public GameObject ObjPre;

	public float delay = 0f;

	public float growTime = 2f;
	public float scaleRate = 10f;

	public bool isFillOnAwake = true;
	public bool isScaleChange = false;

	void Awake()
	{
		if ( isFillOnAwake )
			Fill();
	}

	void RefillObj()
	{
			Invoke( "Fill" , delay );
	}

	void Fill()
	{

		GameObject obj = Instantiate( ObjPre ) as GameObject;
		obj.transform.parent = this.transform;
		obj.transform.localPosition = Vector3.zero;

		if ( isScaleChange )
		{
			Vector3 OriScale = obj.transform.localScale;
			obj.transform.localScale /= scaleRate;

			HOTween.To( obj.transform ,
			           growTime ,
			           "localScale",
			           OriScale ,
			           false ,
			           EaseType.Linear ,
		         	  0f );
		}

	}
}
