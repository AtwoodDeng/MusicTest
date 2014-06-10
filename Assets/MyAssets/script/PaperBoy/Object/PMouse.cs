using UnityEngine;
using System.Collections;

public abstract class PMouse : MonoBehaviour {

	abstract public void Destroy ( float time );
	abstract public void Create ( float time );

	protected void callDestroyWithin( float destroyTime )
	{
		Invoke ("DestroyFinal", destroyTime);
	}

	protected void DestroyFinal()
	{
		this.gameObject.transform.position = new Vector3 (99999f, 999999f, 99999f);
		this.gameObject.SetActive (false);
	}
}
