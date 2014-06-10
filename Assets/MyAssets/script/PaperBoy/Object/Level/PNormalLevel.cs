using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PNormalLevel : PLevel {

	public int LevelID = 0;
	public List<PBlock> blockList = new List<PBlock>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override int GetLevelID ()
	{
		return LevelID;
	}

	public override void SwitchIn ()
	{

	}	

	public override void SwitchOut ()
	{

	}
}
