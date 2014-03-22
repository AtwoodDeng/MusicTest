using UnityEngine;
using System.Collections;

public class FollowWith : MonoBehaviour {

	public enum FollowState
	{
		Mouse
	}

	public FollowState state;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case FollowState.Mouse:
			this.gameObject.transform.position = MouseControl.instance.pos;
			break;
		};
	}
}
