﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Triggerable : MonoBehaviour {

	public List<string> senseTags = new List<string>();
	private GameObject obj;
	public string message="";
	public bool isSendMessage = false;
	public bool isDestroyOnEnter = false ;
	public bool isBarrier = false;

	public Vector3 barrierDirection ;
	public float barrierVelocityLimit = 2f;
	public float barrierIntense = 0.05f;

	public bool isUpdateWhenExit = false;

	// Use this for initialization
	void Start () {
		collider.isTrigger = true;
	}
	
	void OnTriggerEnter( Collider collider)
	{
		if ( senseTags.Contains( collider.tag ))
		{
			if ( isBarrier )
			{
				if ( collider.gameObject.rigidbody != null )
				{
//					Vector3 toward = -collider.gameObject.rigidbody.velocity;
//					if ( rigidbody.velocity.sqrMagnitude > barrierVelocityLimit )
//						rigidbody.velocity = rigidbody.velocity.normalized * barrierVelocityLimit;
					collider.gameObject.rigidbody.AddForce( barrierDirection.normalized * barrierIntense , ForceMode.Impulse);

				}
			}else
			{
				obj = collider.gameObject;
				if ( isSendMessage )
				{
					MessageEventArgs msg = new MessageEventArgs();
					msg.AddMessage("msg" , message );
					BEventManager.Instance.PostEvent( EventDefine.OnTriggerable , msg );
				}
				if ( isDestroyOnEnter )
				{
					AutoDestory autoDestory = gameObject.AddComponent<AutoDestory>();
					autoDestory.destroyTime = Global.ObjDestroyTime;
					autoDestory.isFadeOut = true;
					autoDestory.StartAutoDestory();
					isDestroyOnEnter = false;
				}
			}
		}
	}

	void OnTriggerExit( Collider collider )
	{
		if ( isUpdateWhenExit )
			if ( senseTags.Contains( collider.tag ) && collider.gameObject == obj )
				{}
	}


}