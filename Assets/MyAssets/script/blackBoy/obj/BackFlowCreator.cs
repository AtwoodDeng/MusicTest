using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackFlowCreator : MonoBehaviour {


	public int maxObj = 200;
	public List<BackFlowObj> flowList = new List<BackFlowObj>();
	public float createRange = 5f;
	public GameObject CreatePrefab;
	public float rate = 1f;
	public int initCount = 100;
	// Use this for initialization
	void Awake() {
		for( int i = 0 ;i < initCount ; ++ i)
			Create();
	}
	
	// Update is called once per frame
	float timeRecord = 0;
	void Update () {
		timeRecord+= Time.deltaTime;
		if ( timeRecord > 1f / rate  && flowList.Count < maxObj )
		{
			timeRecord = 0;
			Create();
		}
	}

	void Create()
	{
		if ( CreatePrefab != null )
		{
			GameObject obj = Instantiate( CreatePrefab , transform.position 
			                             + new Vector3( Random.Range( -createRange , createRange )
			              				, Random.Range( -createRange , createRange ) , 0 ) , new Quaternion()) as GameObject;
			obj.transform.parent = transform;
			BackFlowObj backFlow =  obj.GetComponent<BackFlowObj>();
			if (backFlow != null )
			{
				backFlow.parent = this;
				flowList.Add( obj.GetComponent<BackFlowObj>() );
			}
		}
	}

	public void destoryObj(BackFlowObj obj )
	{
		flowList.Remove( obj );
	}
}
