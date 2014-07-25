using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class AutoDestory : MonoBehaviour {

	public float destroyTime = -1;

	public bool isDestroyOnAwake = true;
	public bool isFadeOut = false;
	public bool isStopParticle = false;
	public bool isBecomeSmall = false;
	public Vector3 smallFactor = Vector3.zero;

	// Use this for initialization
	void Awake () {
		if ( isDestroyOnAwake && destroyTime > 0 )
			Invoke("DestroyMySelf" , destroyTime );
	}

	public void StartAutoDestory()
	{
		if ( destroyTime > 0 )
		{
			Invoke("DestroyMySelf" , destroyTime );
			if ( isFadeOut )
			{
				if ( GetComponent<tk2dSprite>() != null){
					tk2dSprite spirte =  GetComponent<tk2dSprite>();

					UnityEngine.Color toColor = spirte.color;
					toColor.a = 0;
					HOTween.To( spirte,
					           destroyTime - 0.05f,
					           "color",
					           toColor);
				}

				if ( GetComponentsInChildren<tk2dSprite>() != null )
				{
					tk2dSprite[] spirtes =  GetComponentsInChildren<tk2dSprite>();

					foreach( tk2dSprite spirte in spirtes )
					{
						
						UnityEngine.Color toColor = spirte.color;
						toColor.a = 0;
						HOTween.To( spirte,
						           destroyTime - 0.05f,
						           "color",
						           toColor);
					}
				}
			}
			if ( isStopParticle )
			{
				if ( GetComponent<ParticleSystem>() != null ){
					GetComponent<ParticleSystem>().enableEmission = false;
				}
				ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
				if ( particles != null )
				{
					foreach( ParticleSystem ps in particles )
					{
						ps.enableEmission = false;
					}
				}
			}
			if ( isBecomeSmall )
			{
				HOTween.To( this.transform
				           , destroyTime
				           , "localScale"
				           , smallFactor
				           , false
				           , EaseType.Linear
				           , 0
				           );

			}

		}
	}

	void DestroyMySelf(){
		Destroy(this.gameObject);
	}
}
