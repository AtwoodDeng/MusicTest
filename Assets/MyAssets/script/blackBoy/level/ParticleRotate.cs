using UnityEngine;
using System.Collections;

public class ParticleRotate : MonoBehaviour {

	public float minRotate = 0f;
	public float maxRotate = 360f;

	public float rotate;

	// Use this for initialization
	void Awake () {
		rotate = Random.Range( minRotate , maxRotate );

		transform.Rotate( new Vector3( 0 , 0 , rotate ) );

		ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>();

		//Debug.Log( "ParticleRotate rotate " + rotate );

		foreach( ParticleSystem ps in pss )
		{
			//Debug.Log("ParticleRotate rotate " + ps.startRotation + " " + ( ps.startRotation - rotate )); 
			ps.startRotation -= rotate / 180f * Mathf.PI;

		}

	}
}
