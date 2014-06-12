using UnityEngine;
using System.Collections;

public class BObjManager : MonoBehaviour {
	
	public BObjManager() { s_Instance = this; }
	public static BObjManager Instance { get { return s_Instance; } }
	private static BObjManager s_Instance;

	public GameObject World
	{
		get {
			if ( _World == null )
			{
				_World = GameObject.Find( "World" );
			}
			return _World;
		}
	}
	GameObject _World;

	public GameObject Effect
	{
		get {
			if ( _Effect == null )
			{
				_Effect = GameObject.Find( "Effect" );
			}
			return _Effect;
		}
	}
	GameObject _Effect;

}
