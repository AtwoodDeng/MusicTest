using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class PLight : MonoBehaviour {

	public ParticleSystem inParticle;
	public Light inLight;
	public PPaperBoy connectBoy = null ;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ConnectWith( PPaperBoy boy )
	{
		connectBoy = boy;
	}
	
	public void DisconnectWith( PPaperBoy boy )
	{
		if (boy == connectBoy)
			connectBoy = null;
	}
	
	public void Destory()
	{
		if (connectBoy != null)
			connectBoy.DisconnectLight (this);
		inParticle.enableEmission = false;
		HOTween.To (inLight, inParticle.startLifetime, new TweenParms().Prop("intensity" , 0 ).Ease(EaseType.EaseOutCubic));
	}

}

