using UnityEngine;
using System.Collections;

public class SceneSwitcher : MonoBehaviour {

	public string nextLevelName;
	public int nextLevelIndex = -1;

	public enum SwitcherType
	{
		None,
		Timer,
	}
	public SwitcherType type;

	public float delay = 1f;

	void Awake()
	{
		if ( type == SwitcherType.Timer )
		{
			Invoke( "GotoNextScene" , delay );
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void GotoNextScene()
	{
		if ( nextLevelIndex >= 0 )
			Application.LoadLevel( nextLevelIndex );
		if ( nextLevelName != null )
			Application.LoadLevel( nextLevelName );
	}
}
