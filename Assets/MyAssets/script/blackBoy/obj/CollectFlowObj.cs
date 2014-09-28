using UnityEngine;
using System.Collections;

public class CollectFlowObj : MonoBehaviour {
	
	public tk2dSprite sprite;
	public float forceIntense = 1f;
	public float TorqueIntense = 1f;

	
	private  float timeOffset = 0;
	public float timeRange = 5f;
	public float DiffRangeInAwake = 2f;

	// Use this for initialization
	void Awake () {

		//setting
		rigidbody.useGravity = false;
		timeOffset = UnityEngine.Random.Range( -10f , 10f);
		transform.Rotate( new Vector3( 0 , 0 , UnityEngine.Random.Range( 0 , 360f )));

		transform.position += getRandomDir() * DiffRangeInAwake;
	}

	Vector3 getRandomDir()
	{
		float degree = UnityEngine.Random.Range(0 , 2 * Mathf.PI);
		return new Vector3( Mathf.Cos( degree ) , Mathf.Sin( degree ));
	}
	
	Vector3 getRandomTorque()
	{
		return new Vector3( 0 , 0 , UnityEngine.Random.Range( -1.0f, 1.0f ));
	}


	// Update is called once per frame
	void Update () {
//		rigidbody.AddForce( getRandomDir() * forceIntense * Mathf.Sin(Time.time * 2 * Mathf.PI / timeRange + timeOffset) , ForceMode.Force );
//		rigidbody.AddTorque( getRandomTorque() * TorqueIntense * Mathf.Sin(Time.time * 2 * Mathf.PI / timeRange + timeOffset) , ForceMode.Force );
	
	}
}
