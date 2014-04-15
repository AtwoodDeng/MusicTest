using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class CollisionSend : MonoBehaviour {

	public string configureTag;
	public string enterFunc;
	public string exitFunc;
	public bool isSendParent = false;

	// Use this for initialization
	void Start () {
		collider.isTrigger = true;
	}

	void OnTriggerEnter(Collider other )
	{
		//Debug.Log ("Enter collider " + other.name);
		if ( configureTag != null && other.gameObject.tag == configureTag )
		{
			//Debug.Log("Ready to send");
			if ( enterFunc != null )
			{
				//Debug.Log("Send Message " + enterFunc + " from " + this.gameObject.name );
				other.gameObject.SendMessage( enterFunc , this.gameObject , SendMessageOptions.DontRequireReceiver );
				this.gameObject.SendMessage( enterFunc , SendMessageOptions.DontRequireReceiver );
				if ( isSendParent )
				other.gameObject.transform.parent.gameObject.SendMessage( enterFunc , this.gameObject , SendMessageOptions.DontRequireReceiver );
			}
		}

	}

	void OnTriggerExit(Collider other )
	{
		if ( configureTag != null && other.gameObject.tag == configureTag )
		{
			if ( exitFunc != null )
			{
				other.gameObject.SendMessage( exitFunc , this.gameObject , SendMessageOptions.DontRequireReceiver );
				this.gameObject.SendMessage( enterFunc , SendMessageOptions.DontRequireReceiver );
				if ( isSendParent )
					other.gameObject.transform.parent.gameObject.SendMessage( exitFunc , this.gameObject , SendMessageOptions.DontRequireReceiver );
			}
		}

	}

}
