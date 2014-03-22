
//#define CLIP_FROM_FILE
#define CLIP_FROM_SOURCE

using UnityEngine;
using System.Collections;
using System;
#if CLIP_FROM_FILE
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
		public static AudioManager instance;
		public float tempValue;
		public int index;
		public float fadeTime = 1f;
		private AudioClip clip;
		public AudioSource source;
		private float[] clip_data;
		private float[] pulse_data;
		private float[] fade_data;
		private int clip_freq;
		private int clip_samp;
		private int clip_chan;
		private DateTime startTime;
		private DateTime tempTime;
		private WWW www;
		public float pluseThreshold = 0.5f;
		public double minPluseIntervalTime = 0.2f;
		private DateTime lastPluseTime ;
		public static float staticZ = 50f;

		void Start ()
		{

				if (AudioManager.instance == null) {
						AudioManager.instance = this;
				}
#if (CLIP_FROM_FILE)
				//string path = EditorUtility.OpenFilePanel( "choose a music" , "" , "ogg");
				string path = "/Users/atwood/tem/unitys/MyTest2/Assets/Resources/music/lib.ogg";
				Debug.Log ("path = " + path);
				if (path.Length != 0) {
						www = new WWW ("file:///" + path);
						StartCoroutine (InitClip ());
				}
#endif

#if (CLIP_FROM_SOURCE)
				StartCoroutine (InitClip ());
#endif
				tempTime = System.DateTime.Now;
				lastPluseTime = System.DateTime.Now;
				index = 0;

				UnityEngine.Random.seed = startTime.Second * 23 + startTime.Minute * 19 + startTime.Hour * 17;

		}

		int pulseStep = 5000;
		int smoothRange = 100;
		float smoothK = 2f ;
		int selectRange = 10;

		void initPulseData ()
		{
		
				int clip_samp_max = clip_samp * clip_chan;

				pulse_data = new float[ clip_samp_max ];
				float[] temp_pulse_data1 = new float[clip_samp_max];
				float[] temp_pulse_data2 = new float[clip_samp_max];
				float[] temp_pulse_data3 = new float[clip_samp_max];

				if (clip_data == null)
						Debug.Log ("cannot find data in pulse init");

				for (int i = 0; i < clip_samp_max; i += pulseStep) {
						temp_pulse_data1 [i] = GetAverageSqureValue (i, pulseStep);
				}

				for (int i = 2 * pulseStep; i < clip_samp_max - 2 * pulseStep; i += pulseStep) {
						temp_pulse_data2 [i] = 
								(3 * temp_pulse_data1 [i + 2 * pulseStep]
								+ temp_pulse_data1 [i + 1 * pulseStep]
								- temp_pulse_data1 [i + 0 * pulseStep]
								- 3 * temp_pulse_data1 [i - 1 * pulseStep]);
						temp_pulse_data2 [i] = Math.Abs (temp_pulse_data2 [i]);
			
						//Debug.Log( "data2 " + temp_pulse_data2[i]);
				}

				for (int i = ( smoothRange / 2 + 1 )  * pulseStep; i < clip_samp_max - ( smoothRange / 2 + 1 ) * pulseStep; i += pulseStep) {
						float averagePulse = 0;
						for (int j = i - smoothRange / 2 * pulseStep; j < i + smoothRange / 2 * pulseStep; j += pulseStep) {
								averagePulse += temp_pulse_data2 [j];
						}
						averagePulse /= smoothRange;
						if (temp_pulse_data2 [i] > averagePulse * smoothK)
								temp_pulse_data3 [i] = temp_pulse_data2 [i];
						else
								temp_pulse_data3 [i] = 0f;
				}

				Debug.Log ("Fre " + clip_freq); 

				for (int i = ( selectRange / 2 + 1 )  * pulseStep; i < clip_samp_max - ( selectRange / 2 + 1 ) * pulseStep; i += pulseStep) {
						float maxPulse = 0;
						for (int j = i - selectRange / 2 * pulseStep; j < i + selectRange / 2 * pulseStep; j += pulseStep) {
								if (temp_pulse_data3 [j] > maxPulse)
										maxPulse = temp_pulse_data3 [j];
						}
						if (temp_pulse_data3 [i] >= maxPulse)
								pulse_data [i] = temp_pulse_data3 [i];
						else
								pulse_data [i] = 0f;
						//Debug.Log( "pulse " + pulse_data[i]);
				}

				for (int i = 0; i < clip_samp_max; i += pulseStep) {
						for (int k = 0; ( k < pulseStep && i + k < clip_samp_max ); k++) {
								pulse_data [i + k] = pulse_data [i];
						}
				}

				//for pulse fade
				int lastPulse = 0;
				fade_data = new float[clip_samp_max];
				for (int i = 0; i < clip_samp_max; i += pulseStep) {
						if (pulse_data [i] > 0)
								lastPulse = i;
						fade_data [i] = (float)Math.Pow ((float)Math.E, - (i - lastPulse) / clip_freq / clip_chan * 0.3f / fadeTime);
				
						for (int k = 0; ( k < pulseStep && i + k < clip_samp_max ); k++) {
							fade_data [i + k] = fade_data [i];
						}
				}
		
		
				Debug.Log ("samp " + clip_samp + " fre " + clip_freq + " leng " + clip.length + " data len " + clip_data.Length);
		}

		IEnumerator InitClip ( )
		{

				if (source == null) {
					source = this.gameObject.GetComponent<AudioSource> ();
				}
#if CLIP_FROM_FILE
				yield return www;
				clip = www.audioClip;
				source.clip = clip;
#endif

#if CLIP_FROM_SOURCE
				clip = source.clip;
				yield return null;
#endif

				clip_chan = clip.channels;
				clip_samp = clip.samples;
				clip_freq = clip.frequency;
				clip_data = new float[clip_chan * clip_samp];

				clip.GetData (clip_data, 0);

				source.Play ();
				startTime = System.DateTime.Now;

				initPulseData ();
		}

		// Update is called once per frame
		void Update ()
		{
				tempTime = System.DateTime.Now;
				UpdateValue ();
				checkAndPulse ();
				checkLoop ();
				GUIDebug.add (ShowType.label, "value = " + getValue ());
		}

		void UpdateValue ()
		{
				if (clip_samp <= 0) 
						return;
				index = Convert.ToInt32 ((tempTime - startTime).TotalSeconds * clip_freq) % clip_samp * clip_chan;
				//if ( index >= clip_samp * clip_chan )
				//	index = clip_samp * clip_chan - 1;
				tempValue = GetAverageValue (index, 1);
		}

		float GetAverageValue (int index, int range)
		{
				int start = index - (range + 1) / 2 * clip_chan;
				int end = index + (range + 1) / 2 * clip_chan;
				if (start < 0)
						start = 0;
				if (end >= clip_samp * clip_chan)
						end = clip_samp * clip_chan - 1;
				float total = 0;
				for (int i = start; i < end; ++i) {
						total += Math.Abs (clip_data [i]);
				}
				return total / (end - start) / clip_chan;
		}

		float GetAverageSqureValue (int index, int range)
		{
				int start = index - (range + 1) / 2 * clip_chan;
				int end = index + (range + 1) / 2 * clip_chan;
				if (start < 0)
						start = 0;
				if (end >= clip_samp * clip_chan)
						end = clip_samp * clip_chan - 1;
				float total = 0;
				for (int i = start; i < end; ++i) {
						total += (float)Math.Pow (clip_data [i], 2);
				}
				return (float)Math.Sqrt (total / (end - start) / clip_chan);
		}

		public float getValue ()
		{
				return Mathf.Abs (GetAverageValue (index, 1));
		}


		public float getFadeValue ()
		{
			return Mathf.Abs (fade_data [index]);
		}

		public void checkAndPulse ()
		{
				if (checkPulseValue ()) {
						//Debug.Log("broadcast");
						BroadcastMessage ("OnMusicPulse", SendMessageOptions.DontRequireReceiver);
						lastPluseTime = System.DateTime.Now;
				}
		}

		public void checkLoop()
		{
			if ( !source.isPlaying ) {
				BroadcastMessage ("OnLoopEnd", SendMessageOptions.DontRequireReceiver);
			Debug.Log("loop");
				source.Play ();
				startTime = System.DateTime.Now;
			}
		}

		public bool checkPulseValue ()
		{
//		DateTime tempTime = System.DateTime.Now;
//		if ( ( tempTime - lastPluseTime ).TotalSeconds < minPluseIntervalTime )
//			return false;
//
//		int range = 1000;
//		float[] squreValue = new float[4];
//		squreValue[0] = GetAverageSqureValue( index - range , range );
//		squreValue[1] = GetAverageSqureValue( index , range );
//		squreValue[2] = GetAverageSqureValue( index + range , range );
//		squreValue[3] = GetAverageSqureValue( index + 2 * range , range );
//		float pulseValue = ( -3 * squreValue[0]  - squreValue[1] + squreValue[2] + 3 * squreValue[3]) ;
//
//		if (pluseThreshold < pulseValue)
//			return true;
//
//		return false;
				//Debug.Log ("index: " + index + " pulse data " + pulse_data.Length );

			if (pulse_data != null && pulse_data [index] > 0)
				return true;

			return false;

		}

}
