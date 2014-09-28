using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BackFlowCreator : MonoBehaviour {


	public int maxObj = 200;
	public List<BackFlowObj> flowList = new List<BackFlowObj>();
	public List<BackFlowObj> disactList = new List<BackFlowObj>();
	public float createRange = 5f;
	public GameObject CreatePrefab;
	public float createRate = 1f;
	public float createBuffThreshold = 0.5f;
	public int createBuffIncrease = 3;
	public int createCount = 1;
	public int initCount = 100;
	public int maxDealClick = 200;

	// Use this for initialization
	void Awake() {
		for( int i = 0 ;i < maxObj ; ++ i)
			Create();
		for( int i = initCount ; i < maxObj ; ++ i )
		{
			BackFlowObj obj = flowList[i];
			obj.gameObject.SetActive( false );
			disactiveObj( obj );
		}
	}

	protected void OnEnable() {
		BEventManager.Instance.RegisterEvent (EventDefine.OnBackClick ,OnBackClick );
	}
	
	protected void OnDisable() {
		BEventManager.Instance.UnregisterEvent (EventDefine.OnBackClick, OnBackClick);
	}

	int backBeg = 0;
	EventArgs backArgs;
	void OnBackClick(EventDefine eventName, object sender, EventArgs args)
	{
//		for( int i = 0; i < flowList.Count; i += maxDealClick  )
//		{
//			backBeg = 0;
//			backArgs = args;
//			Invoke( "BackClick" , Time.time * ( i / maxDealClick ) );
//		}
		foreach( BackFlowObj obj in flowList )
		{
			obj.BackClick( args );
		}
	}


//	void BackClick( )
//	{
//		int i = backBeg;
//		for( ; i < flowList.Count && i < backBeg + maxDealClick ; ++i )
//		{
//			flowList[i].BackClick( backArgs );
//		}
//		backBeg = i;
//	}

	// Update is called once per frame
	float timeRecord = 0;
	void Update () {
		timeRecord+= Time.deltaTime;
		if ( timeRecord > 1f / createRate  && flowList.Count < maxObj )
		{
			timeRecord = 0;
			if ( flowList.Count < maxObj * createBuffThreshold )
				for ( int i = 0 ; i < createCount * createBuffIncrease ; ++ i )
					Active();
			else
				for ( int i = 0 ; i < createCount ; ++ i )
					Active();
		}

	}

	void Create()
	{
		if ( CreatePrefab != null )
		{
			GameObject obj = Instantiate( CreatePrefab , transform.position 
			                             + new Vector3( UnityEngine.Random.Range( -createRange , createRange )
			              			, UnityEngine.Random.Range( -createRange , createRange ) , 0 ) , new Quaternion()) as GameObject;
			obj.transform.parent = BObjManager.Instance.BackFlows.transform;
			BackFlowObj backFlow =  obj.GetComponent<BackFlowObj>();
			if (backFlow != null )
			{
				backFlow.parent = this;
				flowList.Add( obj.GetComponent<BackFlowObj>() );
			}
		}
	}

	void Active()
	{
		if ( disactList.Count > 0 )
		{
			BackFlowObj obj = disactList[0];
			disactList.RemoveAt( 0 );

			flowList.Add( obj );
			obj.transform.position = transform.position 
				+ new Vector3( UnityEngine.Random.Range( -createRange , createRange )
				              , UnityEngine.Random.Range( -createRange , createRange ) , 0 );

			obj.Refresh();
			
		}

	}

	public void destoryObj(BackFlowObj obj )
	{
		flowList.Remove( obj );
	}

	public void disactiveObj(BackFlowObj obj )
	{
		flowList.Remove( obj );
		disactList.Add(obj);
	}
}
