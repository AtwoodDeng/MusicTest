using UnityEngine;
using System.Collections;

public class BFollowWith : MonoBehaviour {

	GameObject target = null;

	public enum FollowState{
		Relatively,   // put at the target's 
		HalfRelatively,  
		HalfAt,   //put between the target's ori position and temp position 
		At,   //put at the target's position

	}

	public enum MoveState{
		Direct,
		Close,
	}

	public enum TargetType{
		UserDefine,
		MainCamera,
		Hero,
	}

	public FollowState followState;
	public MoveState moveState;
	public TargetType targetType;
	public bool fixZ = true;

	private Vector3 relativePos = Vector3.zero;
	private Vector3 targetOriginalPos = Vector3.zero;
	private Vector3 thisOriginalPos = Vector3.zero;
	public float RelativelyRate = 1.0f;
	public float moveRate = 0.5f;


	// Use this for initialization
	void Awake () {
		if ( target != null )
		{
			UpdateTarget( target );
		}
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
			case TargetType.Hero:
				UpdateTarget( BObjManager.Instance.Hero );
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
		relativePos = transform.position -  target.transform.position ;
		targetOriginalPos = target.transform.position;
		thisOriginalPos = transform.position;
		Update();

//		if ( followState == FollowState.Relatively )
//			relativePos = transform.position -  target.transform.position ;
//		else if ( followState == FollowState.HalfAt )
//			targetOriginalPos = target.transform.position;

	}

	Vector3 GetTargetPosition() 
	{
		switch( followState )
		{
		case FollowState.Relatively:
			return target.transform.position + relativePos;
		case FollowState.HalfRelatively:
			Vector3 thisOriToTarTemp = target.transform.position - thisOriginalPos;
			return thisOriginalPos + thisOriToTarTemp * RelativelyRate;
		case FollowState.HalfAt:
			Vector3 targetOriToTemp = target.transform.position - targetOriginalPos;
			return targetOriginalPos + targetOriToTemp * RelativelyRate;
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

	public void OnBecameVisible()
	{
	}

	public void OnBecameInvisible()
	{
	}
}
