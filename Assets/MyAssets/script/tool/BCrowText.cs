using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BCrowText : MonoBehaviour {

	public string levelName;
	public string key;

	public bool isSetOnAwake = true;

	void Awake()
	{
		List<string> script = BDataManager.Instance.getDialogsWithKey( levelName , key );
		tk2dTextMesh text = GetComponent<tk2dTextMesh>();
		if ( text != null )
		{
			text.text = "";
			for( int i = 0 ; i  < script.Count ; ++ i )
			{
				text.text += script[i];
				if ( i != script.Count -1 )
					text.text += "\n";
			}
		}
	}

}
