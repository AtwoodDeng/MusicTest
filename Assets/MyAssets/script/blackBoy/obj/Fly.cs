using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Fly : MonoBehaviour {

	public List<string> senseTags = new List<string>();
	public tk2dSprite sprite;
	public float forceIntense = 1f;
	public float TorqueIntense = 1f;
	public GameObject effect;
	public float HitScaleDiff = 0;
	
	private  float timeOffset = 0;
	public float timeRange = 5f;

	// Use this for initialization
	void Awake () {
		collider.isTrigger = true;
		sprite = gameObject.GetComponent<tk2dSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.AddForce( getRandomDir() * forceIntense * Mathf.Sin(Time.time * 2 * Mathf.PI / timeRange + timeOffset) , ForceMode.Force );
		rigidbody.AddTorque( getRandomTorque() * TorqueIntense * Mathf.Sin(Time.time * 2 * Mathf.PI / timeRange + timeOffset) , ForceMode.Force );
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

	
	void OnBecameInvisible(){
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider collider){
		if ( senseTags.Contains( collider.tag ))
		{
			//effect
			GameObject e = Instantiate( effect ) as GameObject;
			Vector3 colliderPoint = transform.position;

			float sizeDiff = Random.Range( -HitScaleDiff , HitScaleDiff);
			e.transform.parent = BObjManager.Instance.Effect.transform;
			e.transform.localScale *= ( 1 + sizeDiff );
			e.transform.position = transform.position;
			
			
			//particle
			ParticleSystem ps = e.GetComponent<ParticleSystem>();
			if ( ps != null )
			{
					ps.startSize *= ( 1 + sizeDiff );
	//				ps.emissionRate = 1000f;
					ps.startLifetime = 9999f;
					
			}

			//sprite disappear
			Color col = sprite.color;
			col.a = 0;
			sprite.color = col;
			
		}
	}
		
	
}
