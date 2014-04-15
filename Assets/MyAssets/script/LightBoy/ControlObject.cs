using UnityEngine;
using System.Collections;

public enum ControlType
{
	None,
	EnterWithTag,
	CallLevelManager,
	CheckLightType,
}

public class ControlObject : MonoBehaviour {

	public GameObject controlObj;
	public string callFuncName;

	public ControlType controlType;

	public string identifyTag;
	public MyLightColor identifyColor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter( Collider other )
	{
		//Debug.Log ("con enter " + other.gameObject.name);
		if ( controlType == ControlType.EnterWithTag )
		{
			if ( other.gameObject.tag == identifyTag || identifyTag == null  )
			{
				//Debug.Log("con send " );
				controlObj.SendMessage( callFuncName , this.gameObject );
			}
		}else if ( controlType == ControlType.CallLevelManager )
		{
			if ( other.gameObject.tag == identifyTag || identifyTag == null  )
			{
				LevelManager.instance.SendMessage( callFuncName , this.gameObject );
			}

		}else if ( controlType == ControlType.CheckLightType )
		{
			if ( other.gameObject.GetComponent<PaperLight>() != null )
			{
				if ( other.gameObject.GetComponent<PaperLight>().lightColor == identifyColor )
				{
						controlObj.SendMessage( callFuncName , this.gameObject );
				}
			}
		}
	}

}
