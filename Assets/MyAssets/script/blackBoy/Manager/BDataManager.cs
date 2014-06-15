using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BDataManager : MonoBehaviour {
	
	public BDataManager() { s_Instance = this; }
	public static BDataManager Instance { get { return s_Instance; } }
	private static BDataManager s_Instance;

	public string language = "ENGLISH";

	private LevelScript tempLevelScript = new LevelScript();

	public string getNextDialog(string levelName , string lan = null )
	{
		if ( lan == null )
			lan = language;
		if ( !levelName.Equals(tempLevelScript.levelName) )
		{
			TextAsset text = Resources.Load( Global.LevelScriptDictionary[levelName] ) as TextAsset;
			tempLevelScript.init( levelName , text.text );
		}
		return tempLevelScript.getNextDialog(lan , true);
	}

	public string getDialogWithKey(string levelName , string key , string lan = null )
	{
		if ( lan == null )
			lan = language;
		if ( !levelName.Equals(tempLevelScript.levelName) )
		{
			TextAsset text = Resources.Load( Global.LevelScriptDictionary[levelName] ) as TextAsset;
			tempLevelScript.init( levelName , text.text );
		}
		return tempLevelScript.getDialogWithKey( key , lan );
	}

	public class LevelScript{
		public string levelName;
		string[] title;
		string[][] content;

		int index;

		public LevelScript(){
			levelName = "";
			index = 0;
		}

		public void init( string _levelName , string text )
		{
			levelName = _levelName;

			string[] lineArray = text.Split("\r"[0]);
			title = lineArray[0].Split(";"[0]);

			content = new string[lineArray.Length-1][];
			for ( int i = 1 ; i < lineArray.Length ; ++i )
			{
				content[i-1] = lineArray[i].Split(";"[0]);
			}

			index = 0;
		}

		public string getNextDialog( string language , bool isPlusIndex = false)
		{
			int i;
			if ( string.IsNullOrEmpty(language))
				return "";
			language.ToUpper();
			for ( i = 0 ; i < title.Length  ; ++ i )
				if ( language.Equals(title[i]))
					break;
			if ( i >= title.Length )
				return "";
			string res = content[index][i];
			if ( isPlusIndex )
				index++;
			return res;
		}

		public string getDialogWithKey( string key ,  string language )
		{
				
			int i,j;
			if ( string.IsNullOrEmpty(language) || string.IsNullOrEmpty(key))
				return "";
			
			for ( i = 0 ; i < content.Length  ; ++ i )
				if ( key.Equals(content[i][Global.LevelScriptKeyIndex]))
					break;
			if ( i >= content.Length )
				return "";

			language.ToUpper();
			for ( j = 0 ; j < title.Length  ; ++ j )
				if ( language.Equals(title[j]))
					break;
			if ( j >= title.Length )
				return "";

			return content[i][j];

		}

	}

}
