using UnityEngine;
using System.Collections;

public class gestureGuide : MonoBehaviour {

	void OnCustomGesture( PointCloudGesture gesture )
	{
		Debug.Log( "Recognized custom gesture: " + gesture.RecognizedTemplate.name +
		          ", match score: " + gesture.MatchScore +
		          ", match distance: " + gesture.MatchDistance );
	}
}
