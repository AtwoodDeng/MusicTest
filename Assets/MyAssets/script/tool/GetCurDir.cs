using UnityEngine;
using System.Collections;
using System.IO;

public class GetCurDir : MonoBehaviour {

	public string dir = "";

	int total = 0;
	// Use this for initialization
	void Start () {
		dir = Directory.GetCurrentDirectory();
	}

	public string LvlName2Doc( string levelName , string docName = "" )
	{
		if ( docName == "" )
			return System.Environment.CurrentDirectory + "/" + Global.OPP_HISTORY_DIR + "/" + levelName;
		return System.Environment.CurrentDirectory + "/" + Global.OPP_HISTORY_DIR + "/" + levelName + "/" + docName;
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyDown( KeyCode.Escape ))
		{
			Application.Quit();
		}
	}
	string fileName = "";

	void OnGUI() {
		GUILayout.TextArea( "current Dir " + dir ); 

		if ( !Directory.Exists( LvlName2Doc("LV0") ) )
		{
			DirectoryInfo directory = Directory.CreateDirectory( LvlName2Doc("LV0") );
		}else
		{	
			string[] files =  Directory.GetFiles( LvlName2Doc("LV0") );
			
			if ( files != null && files.Length > 0 )
				GUILayout.TextArea( "there are " + files.Length + " files. The last one is " + files[files.Length-1] );
		}


		fileName = GUILayout.TextField( fileName , 20 );
		if ( GUILayout.Button("Save" ) )
		{ 
			StreamWriter sw = new StreamWriter(LvlName2Doc("LV0", fileName + ".his" ) );
			try{
				sw.WriteLine( total++);
				sw.Close();
			}catch( IOException e )
			{
				GUILayout.TextArea("[Error]" + e);
			}
		}

	}
}
