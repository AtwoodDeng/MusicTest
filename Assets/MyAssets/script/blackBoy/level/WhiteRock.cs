using UnityEngine;
using System.Collections;

public class WhiteRock : Catchable {
	
	void Awake()
	{
		gameObject.tag = "ROCK";
	}

	public override Vector3 getForceMain (Vector3 toBody)
	{
		return forceIntense * Vector3.Cross( toBody.normalized , Vector3.forward ) / transform.localPosition.magnitude;
	}
}
