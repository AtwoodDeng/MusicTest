using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Fly : MonoBehaviour {

	public List<string> senseTags = new List<string>();
	public tk2dSprite flySprite;
	public float forceIntense = 1f;
	public float TorqueIntense = 1f;
	public GameObject effect;
	public float HitScaleDiff = 0;

	private  float timeOffset = 0;
	public float timeRange = 5f;
	public bool isDead = false;

	public List<string> deadPicName = new List<string>();
	public tk2dSprite deadSprite;

	public float initMoveRange = 3f;
	public float initMoveTime = 1f;
	public float initScaleDiff = 0.5f;
	public EaseType initMoveEaseType = EaseType.EaseInOutQuart;
	public bool isInit = false;
	
	[HideInInspector]public FlyCreator parent;

	// Use this for initialization
	void Awake () {
		Init();
	}

	public void Init ()
	{
		collider.isTrigger = true;
		if ( flySprite == null )
			flySprite = gameObject.GetComponent<tk2dSprite>();
		if ( deadSprite == null )
			deadSprite = gameObject.GetComponent<tk2dSprite>();

		Vector3 pos = transform.position;
		transform.position += getRandomDir() * initMoveRange;
		HOTween.To( transform 
		           , initMoveTime
		           , new TweenParms().NewProp( "position" , pos , false ).OnComplete( initFinish ).Ease( initMoveEaseType) );

		transform.localScale = Vector3.one * Mathf.Exp( Random.Range( - initScaleDiff , initScaleDiff ));
		
	}

	public void initFinish()
	{
			isInit = false;
	}

	public void Refresh()
	{
	}

	void OnTriggerEnter(Collider collider){
		if ( senseTags.Contains( collider.tag ) && !isDead)
		{
			//test hand 
			HeroHand hand = collider.transform.parent.gameObject.GetComponent<HeroHand>();
			if ( hand == null || hand.state != HeroHand.HandState.Fly )
				return;

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
					ps.startLifetime = 9999f;
					
			}

			//sprite disappear
			
			Color col = flySprite.color;
			col.a = 0;
			flySprite.color = col;

			//dead sprite appear
			col = deadSprite.color;
			col.a = 1;
			deadSprite.color = col;
			if ( deadPicName.Count > 0 )
			{

				deadSprite.SetSprite( deadPicName[ Random.Range( 0 , deadPicName.Count ) ] );
			}

			//stop
			this.rigidbody.isKinematic = true;

			//dead
			isDead = true;
			enabled = false;

			Disactive();
		}
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

	void Update()
	{
		UpdateForce();
	}

	 void UpdateForce()
	{
		rigidbody.AddForce( getRandomDir() * forceIntense * Mathf.Sin(Time.time * 2 * Mathf.PI / timeRange + timeOffset) , ForceMode.Force );
		rigidbody.AddTorque( getRandomTorque() * TorqueIntense * Mathf.Sin(Time.time * 2 * Mathf.PI / timeRange + timeOffset) , ForceMode.Force );
		
	}

	void OnBecameInvisible(){
		if ( !isInit )
		{
			gameObject.SetActive(false);
			Disactive();
		}
	}

	void Disactive()
	{
		if (parent != null )
			parent.disactiveObj( this );
	}
		
	
}
