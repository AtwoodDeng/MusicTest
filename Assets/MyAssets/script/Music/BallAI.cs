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
				particle.enableEmission = false;
				
				transform.position.Set (transform.position.x, transform.position.y, 15f);
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				//Debug.Log (getDivertDirection () * getForce ());
				rigidbody.AddForce ( getDivertDirection () * getForce (), ForceMode.Impulse);
		}

		Vector3 getDivertDirection ()
		{
				Vector3 tempAngel = rigidbody.velocity.normalized;
				float diffAngel = Random.Range (- Mathf.PI, Mathf.PI);
				return (tempAngel 
						+ (new Vector3 (Mathf.Cos (diffAngel), 0, Mathf.Sin (diffAngel))) 
						* directionIntense).normalized;
		}

		Vector3 getRandomDirection ()
		{
			float diffAngel = Random.Range (- Mathf.PI, Mathf.PI);
			return  (new Vector3 (Mathf.Cos (diffAngel), 0, Mathf.Sin (diffAngel) ) ).normalized;
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
				rigidbody.AddForce (getRandomDirection () * getPulse () , ForceMode.Impulse);
				startParticle ();
				Invoke ("stopParticle", emissionTime);
		}

		public void startParticle()
		{
				particle.enableEmission = true;
		}
		public void stopParticle ()
		{
				particle.enableEmission = false;
		}
		
		public void OnTrigger()
	{
		Debug.Log ("On Trigger");
	}

		

}
