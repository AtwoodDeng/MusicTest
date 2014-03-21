using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum ShowType
{
	label
};

public class GUIDebug : MonoBehaviour {
	
	static List<ShowType> types = new List<ShowType>();
	static List<string> contents = new List<string>();

	static bool hasNew = false;

	static public void add( ShowType type , string content )
	{
		if ( !hasNew )
		{
			types.Clear();
			contents.Clear();
			hasNew = true;
		}
		types.Add (type);
		contents.Add (content);
		//Debug.Log ("add");
	}


	void OnGUI()
	{
		hasNew = false;
		for( int i = 0 ; i < types.Count ; ++i )
		{
			//if ( ShowType.label == types[i] )
				GUILayout.Label( contents[i] );

		}

		GUILayout.Label (AudioManager.instance.getValue ().ToString());
		GUI.HorizontalSlider (new Rect (100, 20, 300, 50), 0.05f
		                     , AudioManager.instance.getValue (), 0.05f - AudioManager.instance.getValue ());


	}
}
