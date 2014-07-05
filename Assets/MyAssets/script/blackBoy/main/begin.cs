using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class begin : MonoBehaviour {

	public List<GameObject> buttons;
	public FollowCamera follow;

	void Awake()
	{
		if ( follow == null )
			follow = GetComponent<FollowCamera>();
	}

	void HoverLevel0()
	{
		Debug.Log("Hover Level 0");
	}

	void HoverLevel1()
	{
		HoverLevel( 1 );

	}

	void HoverLevel2()
	{
		HoverLevel( 2 );
	}

	void HoverLevel3()
	{
		HoverLevel( 3 );
	}

	void HoverLevel4()
	{
		HoverLevel( 4 );
	}

	void HoverLevel5()
	{
		HoverLevel( 5 );
	}

	void HoverLevel( int level )
	{
		Debug.Log("hover " + level );
		if ( buttons.Count > level )
			follow.target = buttons[level];
	}
}


