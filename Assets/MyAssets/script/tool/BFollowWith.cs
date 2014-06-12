using UnityEngine;
using System.Collections;

public class BFollowWith : MonoBehaviour {

	 GameObject target = null;

	public enum FollowState{
		Relatively,
		At,

	}

	public enum MoveState{
		Direct,
		Close,
	}

	public enum TargetType{
		UserDefine,
		MainCamera,

	}

	public FollowState followState;
	public MoveState moveState;
	public TargetType targetType;
	public bool fixZ = true;

	Vector3 relativePos = Vector3.zero;
	float moveRate = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( target == null )
		{
			switch(targetType)
			{
			case TargetType.MainCamera:
				UpdateTarget( Camera.main.gameObject );
				break;
			case TargetType.UserDefine:
				break;
			default:
				break;
			}
		}
		if ( target != null )
		{
			Vector3 targetPos = GetTargetPosition();
			MoveTo( targetPos );
		}
	}

	public void UpdateTarget( GameObject _target )
	{
		target = _target;
		if ( followState == FollowState.Relatively )
			relativePos = transform.position -  target.transform.position ;
	}

	Vector3 GetTargetPosition() 
	{
		switch( followState )
		{
		case FollowState.Relatively:
			return target.transform.position + relativePos;
		case FollowState.At:
			return target.transform.position;
		default:
			return transform.position;
		}
	}

	void MoveTo( Vector3 pos )
	{
		Vector3 toward = pos - transform.position;
		if ( fixZ )
			toward.z = 0;
		switch( moveState )
		{
		case MoveState.Direct:
			transform.position += toward;
			break;
		case MoveState.Close:
			transform.position += moveRate * toward;
			break;
		default:
			break;
		}
	}
}
