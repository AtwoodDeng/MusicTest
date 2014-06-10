using UnityEngine;
using System.Collections;

public enum HeroState
{
	Control,
	Histroy
};

[RequireComponent(typeof(Rigidbody))]
public abstract class Hero : MonoBehaviour {


	public abstract void Force ();
	public abstract void SetState( HeroState state );
	public abstract void Create( float time );
	public abstract void Destory( float time );

}
