using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {
	
	public ParticleSystem switchPS;
	public float delay = 2f;
	private string nextSenceName;
	public bool enableSwitchScene = true;

	void Awake()
	{
		enableSwitchScene = true;
	}

	void OnBegin()
	{
		if ( enableSwitchScene )
		{
			switchPS.enableEmission = true;
			nextSenceName = "begin";
			enableSwitchScene = false;
			Invoke( "gotoNextSence" ,delay );
		}
	}


	void OnCollection()
	{
		if ( enableSwitchScene )
		{
			switchPS.enableEmission = true;
			nextSenceName = "collection";
			enableSwitchScene = false;
			Invoke( "gotoNextSence" ,delay );
		}
	}
	
	void gotoNextSence()
	{
		Application.LoadLevel( nextSenceName );
	}
}

