using UnityEngine;
using System.Collections;
using System;

public class Audio : MonoBehaviour {

	public Audio audio;
	private AudioClip audioClip;
	public AudioSource audioSource;
	private DateTime startTime;
	private float[] data;
	private int samples;
	private int frequency;
	private DateTime tempTime;
	private Texture2D tex;

	// Use this for initialization
	void Start () {

		//audioClip = (AudioClip)Resources.Load ("music/BG.mp3");
		//audioSource.clip = audioClip;
		audioClip = audioSource.clip;
		samples = audioClip.samples;
		frequency = audioClip.frequency;
		data = new float[samples * audioClip.channels];
		audioClip.GetData (data, 0);

		audioSource.Play ();

		startTime = System.DateTime.Now;

	}
	
	// Update is called once per frame
	void Update () {
		tempTime = System.DateTime.Now;
	}

	void OnGUI(){
		int index = Convert.ToInt32((tempTime - startTime).TotalSeconds * frequency) % samples * audioClip.channels ;
		float width = Math.Abs( data[index] ) * 100 ;
		GUILayout.HorizontalSlider (width, 0, 10);
		
		GUILayout.TextField (" samples " + samples);
		GUILayout.TextField (" frequency " + frequency);
		GUILayout.TextField (" length " + audioClip.length );
		GUILayout.TextField (" time " + tempTime.Second);
		GUILayout.TextField (" data " + data[index]);
		GUILayout.TextField (" index " + index);
		GUILayout.TextField (" width " + width);
	}
}
