﻿using UnityEngine;
using System.Collections;

public class CrowLevel3 : BLevel {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void DealTrigger (string msg)
	{
		base.DealTrigger( msg );
		if ( "out_of_range".Equals( msg ) )
		{
			Restart( "" );
		}
	}

}