using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Hitable : MonoBehaviour {

	public enum SenseType
	{
		Hero,
	}
	public SenseType senseType;

	public enum EffectType
	{
		Removeable,
		Unremoveable
	}
	public EffectType effectType = EffectType.Removeable;

	public GameObject effect;

	public float RelativeVelocityRate = 10f;
	public float SizeRate = 0.1f;
	public float HitScaleDiff = 0.1f;


	public float splitTime = 0.05f;
	private float timmer = 0f;

	public float minSpeed =0.1f;

	public bool ifHurt = true;
	public float HurtRate = 1.0f;


	void Start()
	{
		collider.isTrigger = false;
	}

	void Update()
	{
		timmer += Time.deltaTime;
	}

	void OnCollisionEnter (Collision col)
	{
		if ( ifCheck( col ) )
		{
			//effect
			GameObject e = Instantiate( effect ) as GameObject;
			Vector3 colliderPoint = col.collider.transform.position;
			ContactPoint hitPoint = col.contacts[0];
			Vector3 velocity = col.relativeVelocity;

			Vector3 toward = hitPoint.normal;
			float angle = Mathf.Atan( toward.y / toward.x ) /Mathf.PI * 180f;

			e.transform.parent = BObjManager.Instance.Effect.transform;
			e.transform.localScale *= ( 1 + HitScaleDiff * velocity.sqrMagnitude );
			e.transform.position = hitPoint.point;
			//e.transform.eulerAngles += new Vector3( angle , 0 , 0 );


			//particle
			ParticleSystem ps = e.GetComponent<ParticleSystem>();
			if ( ps != null )
			{
				if ( effectType == EffectType.Removeable )
				{
					ps.startSize *= ( 1 + Mathf.Pow( velocity.sqrMagnitude , 2f ) * SizeRate );
					ps.emissionRate = velocity.sqrMagnitude * RelativeVelocityRate ;
					ps.startLifetime *= ( 1 + Mathf.Pow( velocity.sqrMagnitude , 2f ) * SizeRate );
				}
				else if ( effectType == EffectType.Unremoveable )
				{
					ps.startSize *= ( 1 + velocity.sqrMagnitude * SizeRate );
					ps.maxParticles = (int)(velocity.sqrMagnitude * RelativeVelocityRate) ;
					ps.emissionRate = 1000f;
					ps.startLifetime = 9999f;

				}

			}

			//hurt
			if ( ifHurt )
			{
				HeroBody hb = col.collider.GetComponent<HeroBody>();
				if ( hb != null )
				{
					hb.Harm( col.relativeVelocity * HurtRate );
				}
			}
		}
	}

	bool ifCheck(Collision col)
	{
		if ( timmer > splitTime && col.relativeVelocity.sqrMagnitude > minSpeed )
		{
			if ( senseType == SenseType.Hero )
			{
				if ( col.collider.tag == Global.HeroTag )
				{
					timmer = 0;
					return true;
				}
			}
		}
		return false;
	}
}
