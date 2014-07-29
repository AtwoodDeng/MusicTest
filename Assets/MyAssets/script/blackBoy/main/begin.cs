using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class begin : MonoBehaviour {

	public List<GameObject> buttons;
	public FollowCamera follow;
	
	public ParticleSystem switchPS;
	public float delay = 2f;
	private string nextSenceName;
	public bool enableSwitchScene = true;

	void Awake()
	{
		if ( follow == null )
			follow = GetComponent<FollowCamera>();
		Screen.showCursor = true;
	}

	void HoverLevel0()
	{
		Debug.Log("Hover Level 0");
	}

	void HoverLevel1()
	{
		HoverLevel( 1 );

	}

	void HoverLevel2()
	{
		HoverLevel( 2 );
	}

	void HoverLevel3()
	{
		HoverLevel( 3 );
	}

	void HoverLevel4()
	{
		HoverLevel( 4 );
	}

	void HoverLevel5()
	{
		HoverLevel( 5 );
	}

	void HoverLevel( int level )
	{
		Debug.Log("hover " + level );
		if ( buttons.Count > level )
			follow.target = buttons[level];
	}

	void OnLeve3()
	{
		if ( enableSwitchScene )
		{
			switchPS.enableEmission = true;
			nextSenceName = "CrowLevel3";
			enableSwitchScene = false;
			Invoke( "gotoNextSence" ,delay );
		}
	}

	void OnLevel2()
	{
		if ( enableSwitchScene )
		{
			switchPS.enableEmission = true;
			nextSenceName = "CrowLevel2";
			enableSwitchScene = false;
			Invoke( "gotoNextSence" ,delay );
		}
	}

	void OnLevel0()
	{
		if ( enableSwitchScene )
		{
			switchPS.enableEmission = true;
			nextSenceName = "CrowLevel0";
			enableSwitchScene = false;
			Invoke( "gotoNextSence" ,delay );
		}
	}

	void gotoNextSence()
	{
		Application.LoadLevel( nextSenceName );
	}
}


