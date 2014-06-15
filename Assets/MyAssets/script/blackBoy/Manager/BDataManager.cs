using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BDataManager : MonoBehaviour {
	
	public BDataManager() { s_Instance = this; }
	public static BDataManager Instance { get { return s_Instance; } }
	private static BDataManager s_Instance;

	public LevelScript tempLevelScript;

	public string getScript(string levelName )
	{
		TextAsset text = Resources.Load( Global.LevelScriptDictionary[levelName] ) as TextAsset;
		return text.text;
	}

	public class LevelScript{
		List<string> title;
		List<List<string>> content;
		public LevelScript(){
			title = new List<string>();
			content = new List<List<string>>();
		}

		void initTitle( string titleLine )
		{

		}
	}

}
