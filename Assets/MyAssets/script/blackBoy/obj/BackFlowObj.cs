using UnityEngine;
using System.Collections;
using System;
using Holoville.HOTween;

[RequireComponent(typeof(tk2dSprite))]
[RequireComponent(typeof(Rigidbody))]
public class BackFlowObj : MonoBehaviour {

	public tk2dSprite sprite;
	public float forceIntense = 1f;
	public float TorqueIntense = 1f;

	public float maxAffectRange = 2f;
	public float minAffectRange = 0.2f;
	public float ClickIntense = 1f;

	public float scaleFloat = 0.3f;
	public float colorFloat = 0.3f;
	public float alphaFloat = 0.5f;

	public float appearTime = 1f;

	private  float timeOffset = 0;
	public float timeRange = 5f;
	public BackFlowCreator parent;

//	protected void OnEnable() {
//		BEventManager.Instance.RegisterEvent (EventDefine.OnBackClick ,OnBackClick );
//	}
//	
//	protected void OnDisable() {
//		BEventManager.Instance.UnregisterEvent (EventDefine.OnBackClick, OnBackClick);
//	}

	 void OnBackClick(EventDefine eventName, object sender, EventArgs args)
	{
		BackClick( args );
	}

	public void BackClick(EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		Vector3 ClickPos = Global.Str2V3( msg.GetMessage( "globalPos" ));
		Vector3 toClickPos = ClickPos - transform.position;
		toClickPos.z = 0;
		float distance = toClickPos.sqrMagnitude;
		if ( distance < minAffectRange )
			distance = minAffectRange;
		if ( distance < maxAffectRange )
			rigidbody.AddForce( - toClickPos.normalized * ClickIntense / distance , ForceMode.Impulse );
	}

	// Use this for initialization
	void Awake () {
		Init();
		Refresh();
	}

	public void Init(){

		if (sprite == null )
			sprite = gameObject.GetComponent<tk2dSprite>();

		//set scale
		float scaleDiff =  1f + UnityEngine.Random.Range( - scaleFloat , scaleFloat );
		transform.localScale *= scaleDiff;
		rigidbody.mass *= scaleDiff * scaleDiff;

		
		//set color
		Color col = sprite.color;
		float colDiff = UnityEngine.Random.Range( -colorFloat , colorFloat );
		col.r = Mathf.Lerp( 0 , 1 , col.r + colDiff); 
		col.g = Mathf.Lerp( 0 , 1 , col.g + colDiff);
		col.b = Mathf.Lerp( 0 , 1 , col.b + colDiff);
		col.a = Mathf.Lerp( 0 , 1 , col.a + UnityEngine.Random.Range( -alphaFloat , alphaFloat ));
		sprite.color = col;

		//setting
		rigidbody.useGravity = false;
		timeOffset = UnityEngine.Random.Range( -10f , 10f);
		transform.Rotate( new Vector3( 0 , 0 , UnityEngine.Random.Range( 0 , 360f )));
	}
	public void Refresh() {
		//active
		gameObject.SetActive( true );

		Color col;
		//appear animator
		float temp_a = sprite.color.a;
		col = sprite.color;
		col.a = 0;
		sprite.color = col;
		col.a = temp_a;
		HOTween.To( sprite , appearTime , new TweenParms().Prop("color" , col ) );

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
		rigidbody.AddForce( getRandomDir() * forceIntense * Mathf.Sin(Time.time * 2 * Mathf.PI / timeRange + timeOffset) , ForceMode.Force );
		rigidbody.AddTorque( getRandomTorque() * TorqueIntense * Mathf.Sin(Time.time * 2 * Mathf.PI / timeRange + timeOffset) , ForceMode.Force );
	}

	void OnBecameInvisible(){
		gameObject.SetActive(false);
		if (parent != null )
			parent.disactiveObj( this );
//		if ( parent != null )
//			parent.destoryObj(this);
//		AutoDestory destory = gameObject.AddComponent<AutoDestory>();
//		destory.destroyTime = 0.1f;
//		destory.StartAutoDestory();
//		enabled = false;
	}
}
