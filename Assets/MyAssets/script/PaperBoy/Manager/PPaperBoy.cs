using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PPaperBoy : MonoBehaviour {
	
	private List<PLight> lights = new List<PLight>();

	void update()
	{

	}

	void OnTriggerEnter(Collider other )
	{
		if (other.gameObject.tag == "PLight")
		{
			PLight light = other.gameObject.GetComponent<PLight>();
			if ( light != null )
			{
				lights.Add (light);
				light.ConnectWith(this);
			}
		}
	}
	
	void OnTriggerExit( Collider other )
	{
		if (other.gameObject.tag == "PLight")
		{
			PLight light = other.gameObject.GetComponent<PLight>();
			if ( light != null )
			{
				lights.Remove (light);
				light.DisconnectWith(this);
			}
		}
	}

	public void DisconnectLight( PLight light )
	{
		lights.Remove (light);
	}

}
