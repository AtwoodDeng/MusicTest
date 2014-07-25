using UnityEngine;
using System.Collections;

public class SceneSwitch : MonoBehaviour {

	public string nextSenceName;
	public ParticleSystem switchPS;
	public float delay = 5f;
	public bool ifGoOnPress = true;
	public bool ifGoOnAwak = false;

	void Awake()
	{
		if ( ifGoOnAwak )
		{
			if ( switchPS != null )
				switchPS.enableEmission = true;
			Invoke( "gotoNextSence" , delay );
		}
	}

	// Update is called once per frame
	void Update () {
		if ( Input.anyKeyDown & ifGoOnPress)
		{
			if ( switchPS != null )
				switchPS.enableEmission = true;
			Invoke( "gotoNextSence" , delay );
		}
	}

	void gotoNextSence()
	{
		Application.LoadLevel( nextSenceName );
	}
}
