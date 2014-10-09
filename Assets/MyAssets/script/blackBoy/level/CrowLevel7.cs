using UnityEngine;
using System.Collections;

public class CrowLevel7 : BLevel {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void DealTrigger (string msg)
	{
		base.DealTrigger (msg);
		if ( Global.OUT_OF_RANGE.Equals( msg ))
		{
			OnDeadResetPosition();
		}
	}
}
