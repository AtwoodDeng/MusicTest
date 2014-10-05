using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomInit : MonoBehaviour {

	public List<string> spriteNameList =new List<string>();
	public Vector3 positionDiffRange;
	public float scaleDiffRange;
	public float delayTimeBeg = 0;
	public float delayTimeEnd = 0 ;
	public float angleRange = 360f;

	// Use this for initialization
	void Awake () {
		Vector3 posDiff = Vector3.zero;
		posDiff.x = Random.Range( - positionDiffRange.x , positionDiffRange.x );
		posDiff.y = Random.Range( - positionDiffRange.y , positionDiffRange.y );
		posDiff.z = Random.Range( - positionDiffRange.z , positionDiffRange.z );
		transform.position += posDiff;
	
		Vector3 scaleDiff = Vector3.one;
		scaleDiff.x = scaleDiff.y = scaleDiff.z = Mathf.Exp( Random.Range( - scaleDiffRange , scaleDiffRange ) );
		transform.localScale = Vector3.Scale( transform.localScale , scaleDiff );

		float angle = Random.Range( 0 , angleRange );
		transform.rotation = Quaternion.Euler( 0 , 0 , angle );

		float delayTime = Random.Range( delayTimeBeg , delayTimeEnd );
		gameObject.SetActive( false );
		Invoke( "Appear" , delayTime );

		if (spriteNameList.Count > 0 )
		{
			int spriteI = Random.Range( 0 , spriteNameList.Count );
			tk2dSprite sprit = gameObject.GetComponent<tk2dSprite>();
			sprit.SetSprite( spriteNameList[spriteI] );
		}
		
	}

	void Appear() {
		gameObject.SetActive( true );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
