using UnityEngine;  
using System.Collections;  

public class Fps : MonoBehaviour  
{  
	float updateInterval = 0.5f;  
	private float accum = 0.0f;   
	private float frames = 0;   
	private float timeleft;   
	private string show;
	// Use this for initialization  
	void Start()  
	{  
//		if (!guiText)  
//		{  
//			enabled = false;  
//			return;  
//		}  
		timeleft = updateInterval;  
	}  
	
	// Update is called once per frame  
	void Update()  
	{  
		timeleft -= Time.deltaTime;  
		accum += Time.timeScale / Time.deltaTime;  
		++frames;  
		
		if (timeleft <= 0.0)  
		{  
			//guiText.text = "FPS:" + (accum / frames).ToString("f2");  
			show =  "FPS:" + (accum / frames).ToString("f2");
			timeleft = updateInterval;  
			accum = 0.0f;  
			frames = 0;  
		}  

		GUIDebug.add(ShowType.label ,show );
	}  
}  