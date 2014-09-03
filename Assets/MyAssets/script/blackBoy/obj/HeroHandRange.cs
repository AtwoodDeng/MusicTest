using UnityEngine;
using System.Collections;

public class HeroHandRange : MonoBehaviour {

	public HeroHand heroHand;
	public Catchable target;
	public float forceIntense = 0.1f;

	// Use this for initialization
//	void Start () {
//		if ( heroHand == null )
//			heroHand = transform.parent.GetComponent<HeroHand>();
//	}
//
//	void Update() {
//		if ( heroHand.state == HeroHand.HandState.Fly )
//		{
//			Vector3 toTarget = target.transform.position - heroHand.transform.position;
//			heroHand.transform.position += toTarget * forceIntense;
//		}
//	}
//
//	void OnTriggerEnter( Collider collider )
//	{
//		if ( heroHand.state == HeroHand.HandState.Fly )
//		{
//			if ( Global.HandStayTag.Equals( collider.gameObject.tag ))
//			{
//				Catchable catchable = collider.gameObject.GetComponent<Catchable>();
//				if ( catchable != null )
//				{
//					target = catchable;
//				}
//			}
//		}
//	}
//
//	void OnTriggerExit( Collider collider )
//	{
//		if ( heroHand.state == HeroHand.HandState.Fly )
//		{
//			if ( target.gameObject == collider.gameObject )
//				target = null;
//		}
//	}
}
