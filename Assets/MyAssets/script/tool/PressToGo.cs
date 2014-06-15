using UnityEngine;
using System.Collections;

public class PressToGo : MonoBehaviour {

	public string nextSenceName;
	public ParticleSystem switchPS;
	public float delay = 5f;

	
	// Update is called once per frame
	void Update () {
		if ( Input.anyKeyDown )
		{
			switchPS.enableEmission = true;
			Invoke( "gotoNextSence" , delay );
		}
	}

	void gotoNextSence()
	{
		Application.LoadLevel( nextSenceName );
	}
}
