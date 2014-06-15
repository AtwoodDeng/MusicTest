using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Emitable : MonoBehaviour {

	public GameObject emitPrefab;

	public Vector3 emitToward;
	public Vector3 startPos;
	public Vector3 startScale = Vector3.one;
	public float emitSpeed;
	public float emitTime;
	public EaseType emitType;

	public bool isBeginEmitOnStart;
	public float delayTime = 0f;
	public float repeatTime = 1f;
	public bool isLocal = true;

	// Use this for initialization
	void Start () {
		if ( isBeginEmitOnStart )
			BeginEmit();
	}

	public void BeginEmit()
	{
		InvokeRepeating( "Emit" , delayTime , repeatTime );
	}

	public void Emit()
	{
		//create a emit
		if ( emitPrefab == null )
		{
			Debug.Log("Cannot Emit for no prefab");
			return;
		}
		GameObject e = Instantiate( emitPrefab ) as GameObject;
		e.transform.parent = this.transform;
		e.transform.localPosition = startPos;
		e.transform.localScale = startScale;

		//set auto destory
		AutoDestory autoDestory = e.GetComponent<AutoDestory>();
		if ( autoDestory == null )
		{
			autoDestory = e.AddComponent<AutoDestory>();
		}
		autoDestory.destroyTime = emitTime;
		autoDestory.isDestroyOnAwake = false;
		autoDestory.StartAutoDestory();

		string proName = "position";
		if ( isLocal )
			proName = "localPosition";

		HOTween.To( e.transform
		           , emitTime
		           , new TweenParms()
		           .Prop( proName , emitToward.normalized * emitSpeed * emitTime , true )
		           .Ease( emitType ));


	}
}
