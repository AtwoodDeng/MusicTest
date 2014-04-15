using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Holoville.HOTween;

public class BoyManager : MonoBehaviour {

	static public BoyManager instance;

	//light 
	public List<MyLight> lightList = new List<MyLight>();

	public float lightForce = 1f;
	public float lightDistance = 3f;
	public float lightForceRate = 0.1f;


	//boy
	public GameObject boyPrefab;
	public GameObject boyObj;
	public Boy boyCom;

	//Floor
	public List<BoyFloor> floorList = new List<BoyFloor> ();

	//public Vector3 initPosition;

	//others
	public List<Others> othersList = new List<Others>();
	public float othersForce = 0.05f;
	public float othersForceDis = 8f;
	
	// Use this for initialization
	void Start () {
		if ( instance == null )
			instance = this;

		//create part
//		fixObj = (GameObject)Instantiate (fixPrefab);
//		boyBlockObj = (GameObject)Instantiate (boyBlockPrefab);
//		boyGraPreObj = (GameObject)Instantiate (boyGraPrefab);
//		fixObj.transform.position = new Vector3 (fixPosX, fixPosY, AudioManager.staticZ);
//		fixObj.transform.parent = transform;
//		boyBlockObj.transform.position = new Vector3 (0, 0, AudioManager.staticZ);
//		boyBlockObj.transform.parent = transform;
//		boyGraPreObj.transform.position = new Vector3 (graPosX, graPosY, AudioManager.staticZ);
//		boyGraPreObj.transform.parent = transform;

		//joint part 
//		HingeJoint joint = fixObj.GetComponent<HingeJoint> ();
//		if ( joint == null )
//		{
//			Debug.LogError("no joint found in fix ");
//		}else
//		{
//			joint.connectedBody = boyBlockObj.GetComponent<Rigidbody>();
//		}
//
//		FixedJoint fJoint = boyGraPreObj.GetComponent<FixedJoint> ();
//		if ( fJoint == null )
//		{
//			Debug.LogError("no joint found in gra");
//		}else
//		{
//			fJoint.connectedBody = boyBlockObj.GetComponent<Rigidbody>();
//		}

		//create boy
		floorList.Clear ();
		if ( boyObj == null )
		{
			boyObj = (GameObject)Instantiate (boyPrefab);
			boyCom = boyObj.GetComponent<Boy> ();
			boyObj.transform.parent = transform;
			boyObj.transform.localPosition = LevelManager.instance.startPosition();
		}
	
	}
	
	void Update(){
//		if ( Application.loadedLevelName == "LightBoy" )
//			fixObj.transform.position = MouseControl.instance.pos;
//		else
//		{
//			if ( lightList.Count > 0 )
//			{
//				MyLight light = findTheNearestLight();
//				if ( light != null )
//					followLight(light);
//			}
//		}
		if ( lightList.Count > 0 )
		{
			MyLight light = findTheNearestLight();
			if ( light != null )
				followLight(light);
		}
		//DealOthers ();
		//if (! checkFloor ())
		//	DestroyBoy ();
	}

	protected MyLight findTheNearestLight()
	{
		MyLight res = null ;
		float minDis = 100000;
		for(int i = 0 ; i < lightList.Count ; ++ i )
		{
			if ( lightList[i] == null )
			{
				lightList.RemoveAt(i);
				i--;
				continue;
			}
			float dis = (lightList[i].gameObject.transform.position - boyCom.boyBlock.transform.position).sqrMagnitude;
			if ( dis < minDis )
			{
				dis = minDis;
				res = lightList[i];
			}
		}
		return res;
	}

	protected void followLight( MyLight light )
	{
		if ( light.type == LightType.Normal )
		{
			ForceToLight( light );
		}
	}

	protected void ForceToLight( MyLight light )
	{
		Vector3 toLight = light.gameObject.transform.position - boyCom.boyBlock.transform.position;
		toLight.z = 0;
		Vector3 force;
		if ( toLight.sqrMagnitude > lightDistance  )
			force = toLight.normalized * lightForce;
		else
			force = toLight * lightForceRate;
		boyCom.boyFix.transform.position += ShakeVector (force);
	}	

	public float shakeTime = 1f;
	protected Vector3 ShakeVector( Vector3 input )
	{
		DateTime time = System.DateTime.Now;
		float k = Mathf.PI * 2 / shakeTime / 1000f;
		return input * (1 -1f * Mathf.Sin ( k * (float)time.Millisecond));
	}

	//static DateTime checkTime;
	public bool checkFloor()
	{
		//DateTime time = System.DateTime.Now;
		if (floorList.Count < 1 )// && (time-checkTime).Milliseconds < 300 )
			return false;
		//checkTime = System.DateTime.Now;
		return true;
	}

	public void DealOthers()
	{
		for( int i = 0 ; i < othersList.Count ; ++ i )
		{
			Vector3 toOther = othersList[i].boyBlock.gameObject.transform.position - boyCom.boyBlock.transform.position;
			toOther.z = 0;
			Debug.Log ("Deal" + i.ToString () + " " + toOther.ToString());
			if ( toOther.sqrMagnitude < othersForceDis)
			{
				boyCom.boyFix.transform.position -= toOther.normalized * ( othersForceDis - toOther.sqrMagnitude );
				othersList[i].boyFix.transform.position += 
					toOther.normalized * ( othersForceDis - toOther.sqrMagnitude ) / 10;
//				Debug.Log("Force others");
//				boyCom.boyFix.transform.position -= toOther.normalized * othersForce;
			}
		}
	}

	public void OnGUI()
	{
		GUILayout.Label( "floor" + floorList.Count );
		GUILayout.Label ("others" + othersList.Count);
	}

	public void Clear()
	{
		lightList.Clear ();
		floorList.Clear ();
		othersList.Clear ();
	}

	public void DestroyBoy()
	{
		Vector3 toPos = boyCom.boyFix.transform.position + new Vector3 (0, 0, 10);
		HOTween.To (boyCom.boyFix.transform, 3f, "position", new Vector3 (0, 0, 10), true, EaseType.EaseOutCubic, 0f);
		Invoke ("DestroyBoyFinnal" , 3f );
	}

	public void DestroyBoyFinnal()
	{
		GameObject.Destroy (boyObj);
		boyObj = null;

		Start ();
	}
}
