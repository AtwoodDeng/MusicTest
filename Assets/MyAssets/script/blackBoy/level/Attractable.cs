using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Attractable : MonoBehaviour {

	public float forceIntense = 0.01f;
	public HeroHand hand;

	// Use this for initialization
	void Start () {
		collider.isTrigger = true;
	}

	void OnTriggerEnter( Collider collider )
	{
		if ( collider.gameObject.tag == Global.HeroHandTag )
		{
			hand = collider.gameObject.GetComponent<HeroHand>();
		}

	}

	void OnTriggerStay( Collider collider )
	{
		if ( collider.gameObject.tag == Global.HeroHandTag && hand != null )
		{
			if ( hand.state == HeroHand.HandState.Fly )
			{
				ForceAttract( hand );
			}
		}
	}

	void OnTriggerExit( Collider collider )
	{
		if ( collider.gameObject.tag == Global.HeroHandTag )
		{
			hand = null ;
		}
	}

	void ForceAttract( HeroHand hand )
	{
		Vector3 hand2center = transform.position - hand.transform.position;

		hand.rigidbody.AddForce( hand2center * forceIntense , ForceMode.Impulse );
	}

}
