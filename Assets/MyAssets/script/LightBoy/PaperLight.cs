using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public enum MyLightColor{
	None,
	Red,
	Blue,
	Green,

}

public class PaperLight : Mouse {

	public Light light;
	public  PaperBoy connectBoy = null ;
	public MyLightColor lightColor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeColorTo( MyLightColor color )
	{
		//Debug.Log ("PaperLight Change color " + color.ToString ());
		if ( color != lightColor )
		{
		switch(color)
		{
		case MyLightColor.Blue:
			particle.startColor = Color.blue;
			light.color = Color.blue;
			break;
		case MyLightColor.Green:
				particle.startColor = Color.green;
				light.color = Color.green;
			break;
		case MyLightColor.Red:
				particle.startColor = Color.red;
				light.color = Color.red;
			break;
		default:
				particle.startColor = Color.white;
				light.color = Color.white;
			break;
		}
			lightColor = color;
		}
	}

	public void ConnectWith( PaperBoy boy )
	{
		connectBoy = boy;
	}

	public void DisconnectWith( PaperBoy boy )
	{
		if (boy == connectBoy)
						connectBoy = null;
	}

	public void Destory()
	{ 
		//Debug.Log ("Destory light");
		if (connectBoy != null)
			connectBoy.DisconnectLight (this);
		base.Destory ();
		particle.enableEmission = false;
		HOTween.To (light, particle.startLifetime, new TweenParms().Prop("intensity" , 0 ).Ease(EaseType.EaseOutCubic));
	}
}
