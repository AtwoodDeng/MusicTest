using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlyCreator : MonoBehaviour {
	
	
	public int maxObj = 200;
	public List<Fly> flowList = new List<Fly>();
//	public List<BackFlowObj> disactList = new List<BackFlowObj>();
	public float createRange = 5f;
	public float createMinRange = 2f;
	public GameObject CreatePrefab;
	public float createRate = 1f;
	public float createBuffThreshold = 0.5f;
	public int createBuffIncrease = 3;
	public int createCount = 1;
	public int initCount = 100;
	//	public int maxDealClick = 200;
	
	// Use this for initialization
	void Awake() {
		for( int i = 0 ;i < maxObj ; ++ i)
			Create();
	}

	
	// Update is called once per frame
	float timeRecord = 0;
	void Update () {
		timeRecord+= Time.deltaTime;
		if ( timeRecord > 1f / createRate  && flowList.Count < maxObj )
		{
			timeRecord = 0;
			if ( flowList.Count < maxObj * createBuffThreshold )
				for ( int i = 0 ; i < createCount * createBuffIncrease ; ++ i )
					Create();
			else
				for ( int i = 0 ; i < createCount ; ++ i )
					Create();
		}
		
	}
	
	void Create()
	{
		if ( CreatePrefab != null )
		{
			Vector3 createPos = Vector3.zero;
			while( true )
			{
				createPos = transform.position 
					+ new Vector3( UnityEngine.Random.Range( -createRange , createRange )
					              , UnityEngine.Random.Range( -createRange , createRange ) , 0 );
				if ( (createPos - BObjManager.Instance.BHeroBody.transform.position ).sqrMagnitude > createMinRange )
				{
					break;
				}
			}
			GameObject obj = Instantiate( CreatePrefab , createPos , new Quaternion()) as GameObject;
			obj.transform.parent = BObjManager.Instance.BackFlows.transform;
			Fly fly =  obj.GetComponent<Fly>();
			if (fly != null )
			{
				fly.parent = this;
				flowList.Add( obj.GetComponent<Fly>() );
			}
		}
	}

	
	public void destoryObj(Fly obj )
	{
		flowList.Remove( obj );
	}
	
	public void disactiveObj(Fly obj )
	{
		flowList.Remove( obj );
	}
}
