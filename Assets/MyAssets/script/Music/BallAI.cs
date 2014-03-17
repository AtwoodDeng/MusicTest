using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(SphereCollider))]
public class BallAI : MonoBehaviour
{

		SphereCollider collider;
		public float forceIntense = 0.5f;
		public float forceMusicRelate = 0.5f;
		public float pulseIntense = 5f;
		public float directionIntense = 0.1f;
		public	ParticleSystem particle;
		public  float emissionTime;
		
		// Use this for initialization
		void Start ()
		{
				rigidbody.useGravity = false;
				collider = gameObject.GetComponent<SphereCollider> ();
				collider.isTrigger = true;
				if ( particle )
					particle.enableEmission = false;
					
				transform.localPosition.Set (transform.localPosition.x, transform.localPosition.y, AudioManager.staticZ);
				
		}
	
		// Update is called once per frame
		void Update ()
		{
			Force ();
		}

		void Force()
		{
			//Debug.Log (getDivertDirection () * getForce ());
			rigidbody.AddForce ( getForceDirection ().normalized * getForce (), ForceMode.Impulse);
		}

		Vector3 getForceDirection ()
		{
				Vector3 tempAngel = rigidbody.velocity.normalized;
				float diffAngel = Random.Range (- Mathf.PI, Mathf.PI);
				return (tempAngel 
						+ (new Vector3 (Mathf.Cos (diffAngel), Mathf.Sin (diffAngel) , 0 )) 
						* directionIntense).normalized;
		}

		Vector3 getPulseDirection ()
		{
			float diffAngel = Random.Range (- Mathf.PI, Mathf.PI);
			return  (new Vector3 (Mathf.Cos (diffAngel), Mathf.Sin (diffAngel) , 0 ) ).normalized;
		}

		float getForce ()
		{
			float value = AudioManager.instance.getValue ();
			return Mathf.Pow( value * 100 , forceMusicRelate ) * forceIntense ;
		}

		float getPulse()
		{
			return pulseIntense;
		}

		void OnGUI ()
		{
				//GUILayout.TextField( "speed X " + rigidbody.velocity.x );
				//GUILayout.TextField( "speed Z " + rigidbody.velocity.z );

		}

		public void OnMusicPulse ()
		{
				//Debug.Log ("pulse" + gameObject.name);
				rigidbody.AddForce (getPulseDirection () * getPulse () , ForceMode.Impulse);
				startParticle ();
				Invoke ("stopParticle", emissionTime);
		}

		public void startParticle()
		{
			if ( particle )
				particle.enableEmission = true;
		}
		public void stopParticle ()
		{
		    if ( particle )
				particle.enableEmission = false;
		}
		
		public void OnTrigger()
	{
		Debug.Log ("On Trigger");
	}

		

}
