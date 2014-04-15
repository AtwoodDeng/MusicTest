using UnityEngine;
using System.Collections;
using System;

public class MyGestureRender : PointCloudGestureRenderer {

	public ParticleSystem particle;
	public float BlinkDelay = 0.5f;
	public float BlinkLast = 0.5f;
	public AudioSource acheiveSound;
	static string[] toName = {"4" , "2" , "1" , "3" , "5" };
	public Color blinkColor;
	public Color finishColor;

	void Start()
	{
		if( GestureTemplate )
			Render( GestureTemplate );
	}

	public void Blink ()
	{
		Debug.Log ("Blink");

		Invoke ("BlinkParticle", BlinkDelay);
	}

	public void BlinkParticle ()
	{
		StartParticle ();
		particle.startColor = blinkColor;
		Invoke ("StopParticle", BlinkLast);
	}
	public void StartParticle ()
	{
		
		if (particle == null)
			particle = gameObject.GetComponent<ParticleSystem> ();
		particle.startColor = finishColor;
		particle.enableEmission = true;
		if ( !acheiveSound.isPlaying )
			acheiveSound.Play ();
	}
	public void StopParticle ()
	{
		particle.enableEmission = false;
	}

	public bool isOn()
	{
		return particle.enableEmission;
	}

	public string getName ()
	{
		return GestureTemplate.name;
	}

	public bool Render( PointCloudGestureTemplate template )
	{
		Debug.Log ("Render");
		if (!base.Render (template))
			return false;
		if ( acheiveSound != null )
		{
			int i = Convert.ToInt32( template.name ) - 1;
			acheiveSound.clip =  Resources.Load("music/GuestureTest2/" + toName[i] , typeof(AudioClip)) as AudioClip ;
			if ( acheiveSound.clip == null )
			{
				Debug.Log( "Gesture template cannot read " + template.name );
			}else
			{
				Debug.Log( "Gesture successfully read  " + template.name );
			}
		}
		return true;
	}

}
