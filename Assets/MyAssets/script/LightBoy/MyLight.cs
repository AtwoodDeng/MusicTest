using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public enum LightState
{
	None,
	Drag,
}

public enum LightType
{
	Normal,
	Red
}

[RequireComponent(typeof(SphereCollider))]
public class MyLight : Mouse {

	public LightState state;
	public LightType type;
	public Light light;
	

	void Start()
	{
		collider.isTrigger = true;
	}

	void OnTriggerEnter(Collider other )
	{
		if (other.gameObject.tag == "BoyBlock")
			BoyManager.instance.lightList.Add (this);
	}

	void OnTriggerExit( Collider other )
	{
		if (other.gameObject.tag == "BoyBlock")
			BoyManager.instance.lightList.Remove (this);
	}


	
	public void Destory()
	{
		base.Destory ();
		particle.enableEmission = false; 
		Debug.Log ("Destory light");
		HOTween.To (light, particle.startLifetime, "intensity", 0, true , EaseType.Linear, 0f);
	}

}
