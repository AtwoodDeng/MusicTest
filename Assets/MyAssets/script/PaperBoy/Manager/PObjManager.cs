using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PObjManager : MonoBehaviour {

	public static PObjManager instance = null;

	[HideInInspector] public PMouse tempMouse
	{
		get
		{
			if ( PMouseManager.instance != null )
				return PMouseManager.instance.mouse;
			return null;
		}
		set{
		}

	}

	private Hero _tempHero;
	[HideInInspector] public Hero tempHero
	{
		get{
			if (_tempHero==null)
			{
				GameObject heroObj = GameObject.FindGameObjectWithTag("Hero");
				if (heroObj !=null)
				{
					_tempHero = heroObj.GetComponent<Hero>();
					return _tempHero;
				}
			}
			return _tempHero;
		}
		set{
		}
	}

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = new PObjManager ();
	}

	public void LateUpdate(){
		setHero ();
	}

	public void setHero()
	{
		if (tempHero != null )
		{
//			Vector3 dir = tempMouse.transform.localPosition - tempHero.transform.localPosition;
//			tempHero.rigidbody.AddForce( dir * heroForceIntense , ForceMode.Acceleration );
			tempHero.Force();
		}
	}

	public Vector3 getHeroPosition()
	{
		if ( tempHero != null )
			return tempHero.transform.localPosition;
		return Vector3.one * 9999f;
	}
}
