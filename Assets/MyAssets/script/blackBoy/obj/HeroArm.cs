using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class HeroArm : MonoBehaviour {

	public HeroBody body;
	public HeroHand hand;
	public LineRenderer lineRenderer;
	
	// Update is called once per frame
	void Update () {

		if ( lineRenderer == null )
			lineRenderer = GetComponent<LineRenderer>();
		if ( body != null && hand != null )
		{
			lineRenderer.SetPosition( 0 , body.getArmPos() + Global.BHeroArmOff);
			lineRenderer.SetPosition( 1 , hand.getArmPos() + Global.BHeroArmOff);
		}
	}

	public void Init( HeroBody _body , HeroHand _hand )
	{
		body = _body;
		hand = _hand;
		transform.parent = _body.transform;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		Update();
	}
}
