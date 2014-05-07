using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PObjManager : MonoBehaviour {

	static PObjManager instance;

	PPaperBoy mainPPaperBoy;
	List<PPaperBoy> paperBoyList;

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		paperBoyList = new List<PPaperBoy> ();
	}

	public GameObject getPaperBoy(int i)
	{
		return null;
	}

	public Vector3 getPBoyPos()
	{
		return null; 
	}

}
