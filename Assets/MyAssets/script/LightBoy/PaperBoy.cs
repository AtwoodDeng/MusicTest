using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider))]
public class PaperBoy : MonoBehaviour {

	public GameObject boyBlock;
	public Transform head;
	public GameObject boyFix;
	public SpringJoint fixSpring;
	 SphereCollider collider;

	
	public float lightForce = 0.5f;
	public float lightDistance = 5f;
	public float lightForceRate = 0.1f;
	public float fixSpringSpring = 5f;

	private List<PaperLight> lights = new List<PaperLight>();

	// Use this for initialization
	void Start () {
		collider = gameObject.GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (lights.Count > 0) {
			PaperLight nearestLight = findTheNearestLight();
			if( nearestLight != null )
				ForceTo(nearestLight.transform.position );
				//ForceTo( MouseControl.instance.pos );
		}
		collider.center = boyBlock.transform.localPosition;
	}

	protected PaperLight findTheNearestLight()
	{
		PaperLight res = null ;
		float minDis = 100000;
		for(int i = 0 ; i < lights.Count ; ++ i )
		{
			if ( lights[i] == null )
			{
				continue;
			}
			float dis = (lights[i].gameObject.transform.position - boyBlock.transform.position).sqrMagnitude;
			if ( dis < minDis )
			{
				dis = minDis;
				res = lights[i];
			}

		}
		return res;
	}

	void ForceTo( Vector3 aim )
	{
		float dis = (boyFix.transform.position - boyBlock.transform.position).sqrMagnitude;
		Vector3 toLight = aim - boyBlock.transform.position;
		Vector3 newAim = dis * toLight.normalized + boyBlock.transform.position;
		Vector3 toAim = newAim - boyBlock.transform.position;
		toLight.z = 0;
		Debug.Log ("to light " + toLight);
		Vector3 force;
		if ( toAim.sqrMagnitude > lightDistance  )
		{
			force = toAim.normalized * lightForce;
		}
		else
			force = toAim * lightForceRate;
		//boyFix.transform.position += force;
		
		Debug.Log ("shake rate " + ShakeVector (force));


		boyFix.transform.localPosition += ShakeVector (force);

//		boyFix.transform.position = new Vector3( aim.x , aim.y , 0 );
//		fixSpring.spring = ShakeFloat (fixSpringSpring);

	}


	public void DisconnectLight( PaperLight light )
	{
		lights.Remove (light);
	}
	
	public float shakeTime = 1f;
	protected Vector3 ShakeVector( Vector3 input )
	{
		DateTime time = System.DateTime.Now;
		float k = Mathf.PI * 2 / shakeTime / 1000f;
		return input * (1 - 1f * Mathf.Sin ( k * (float)time.Millisecond));
	}

	protected float ShakeFloat( float input )
	{
		DateTime time = System.DateTime.Now;
		float k = Mathf.PI * 2 / shakeTime / 1000f;
		return input * (1 - 1f * Mathf.Sin ( k * (float)time.Millisecond));
	}

	void OnTriggerEnter(Collider other )
	{
		if (other.gameObject.tag == "BoyLight")
		{
			PaperLight light = other.gameObject.GetComponent<PaperLight>();
			if ( light != null )
			{
				lights.Add (light);
				light.ConnectWith(this);
			}
		}
	}
	
	void OnTriggerExit( Collider other )
	{
		if (other.gameObject.tag == "BoyLight")
		{
			PaperLight light = other.gameObject.GetComponent<PaperLight>();
			if ( light != null )
			{
				lights.Remove (light);
				light.DisconnectWith(this);
			}
		}
	}

	void ChangeMouseColor( GameObject obj )
	{
		Debug.Log ("Change color " + obj.name);
		if (obj.name == "LigChaGreen" || obj.name == "LigChaGreen(Clone)")
			MouseControl.instance.ChangeColorTo (MyLightColor.Green);
		if ( obj.name == "LigChaRed" || obj.name == "LigChaRed(Clone)" )
			MouseControl.instance.ChangeColorTo (MyLightColor.Red);
			
	}
	void ChangeMouseColor( )
	{
		Debug.Log ("Change color void ");
	}

}
