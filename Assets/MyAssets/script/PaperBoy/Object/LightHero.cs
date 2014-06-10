using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class LightHero : Hero {

	[HideInInspector] public PMouse controlMouse;
	public HeroState state;

	public void OnTriggerEnter( Collider other){
		Debug.Log ("light hero enter");
		if (other.tag == "Mouse")
			if ( state == HeroState.Control )
			{
				controlMouse = other.gameObject.GetComponent<PMouse> ();
				PEventManager.Instance.PostEvent( EventDefine.OnMouseInHero , new MessageEventArgs());
			}
	}

	public void OnTriggerExit( Collider other){
		if (other.tag == "Mouse")
		{
			if ( state == HeroState.Control )
				if (other.gameObject.GetComponent<PMouse>() == controlMouse )
				{
					controlMouse = null;
					PEventManager.Instance.PostEvent( EventDefine.OnMouseOutHero , new MessageEventArgs());
				}
		}
	}

	public override void SetState (HeroState _state)
	{
		state = _state;
	}


	public float forceIntense = 0.5f;

	public float getForceI( Vector3 dis )
	{
		return Mathf.Exp (dis.magnitude * forceIntense);
	}
	
	public override void Force ()
	{
		if (controlMouse == null)
			return;
		Vector3 dir = controlMouse.transform.localPosition - transform.localPosition;
		if ( dir.magnitude > 9999f ) 
		{
			controlMouse = null;
			return;
		}
		GUIDebug.add(ShowType.label , dir.ToString() );

		// if get too close
		if (dir.magnitude < 1e-7)
				return;

		rigidbody.AddForce ( getForceI(dir) * dir.normalized, ForceMode.Acceleration);
	}

	public override void Create( float time )
	{
		Debug.Log("Hero Create" );
		Vector3 tempPos = transform.localPosition;
		transform.localPosition += new Vector3( 0 , 0 , 100f );
		HOTween.To( transform 
		           , time 
		           , new TweenParms()
		           .Prop( "localposition" , tempPos )
		           .Ease(EaseType.EaseInQuint )
		           );
		     

	}

	public override void Destory( float time )
	{
		Debug.Log("Hero Destroy");
		HOTween.To( transform 
		           , time 
		           , new TweenParms()
		           .Prop( "localposition" , new Vector3(0,0,100f) , true )
		           .Ease(EaseType.EaseInQuint )
		           );
	}
}
