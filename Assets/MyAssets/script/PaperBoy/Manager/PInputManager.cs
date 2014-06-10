using UnityEngine;
using System.Collections;
using System;

public class PInputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	private bool switchOn = true;

	// Update is called once per frame
	void Update () {
		GUIDebug.add( ShowType.label , Input.GetAxisRaw( "left" ) + " " + Input.GetAxis( "left" )  );

		if ( Input.GetAxisRaw( "left" ) == 1 )
		{
			if ( switchOn )
			{
				MessageEventArgs msg = new MessageEventArgs();
				msg.AddMessage("toward" , "last" );
				msg.AddMessage("levelID" , PLevelManager.instance.tempLevelID().ToString());
				msg.AddMessage("index" , PLevelManager.instance.getIndex().ToString());
				PEventManager.Instance.PostEvent( EventDefine.OnSwitchLevel , msg );
				switchOn = false ;
			}
		} else if ( Input.GetAxisRaw( "right" ) == 1 )
		{
			if ( switchOn )
			{
				MessageEventArgs msg = new MessageEventArgs();
				msg.AddMessage("toward" , "next" );
				msg.AddMessage("levelID" , PLevelManager.instance.tempLevelID().ToString());
				msg.AddMessage("index" , PLevelManager.instance.getIndex().ToString());
				PEventManager.Instance.PostEvent( EventDefine.OnSwitchLevel , msg );
				switchOn = false;
			}
		}else
		{
			switchOn = true;
		}
	}
}
