using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class MouseNormal : PMouse {

	public Light light;
	//public float destroyTime = 0.5f;
	public ParticleSystem par;

	void OnTriggerEnter(Collider other )
	{
//		if (other.gameObject.tag == "BoyBlock")
//			BoyManager.instance.lightList.Add (this);
	}
	
	void OnTriggerExit( Collider other )
	{
//		if (other.gameObject.tag == "BoyBlock")
//			BoyManager.instance.lightList.Remove (this);
	}



	public override void Destroy ( float time )
	{
		callDestroyWithin (par.startLifetime);
		if ( light != null )
		{
			par.enableEmission = false;
			HOTween.To (light, par.startLifetime, new TweenParms().Prop("intensity" , 0 ).Ease(EaseType.EaseOutCubic));
		}
	}

	public override void Create ( float time )
	{
		if ( light != null )
		{
			par.enableEmission = true;
			light.intensity = 0f;
			HOTween.To (light, par.startLifetime, new TweenParms().Prop("intensity" , 1f ).Ease(EaseType.EaseOutCubic));
		}
	}
}
