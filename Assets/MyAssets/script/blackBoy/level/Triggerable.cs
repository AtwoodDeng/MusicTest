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
	public bool isDisableOnDestroy = true;
	public bool isRefill = false;
	public bool isSmallOnDestory = false;
	public bool isBarrier = false;
	public bool isCenterBarrier = false;
	public bool isSlowBarrier = false;
	public bool isShrinkOnEnter = false;

	public GameObject DestoryEffect;
	public float destoryTime = 10f;

	public Vector3 barrierDirection ;
	//public float barrierVelocityLimit = 2f;
	public float barrierIntense = 0.05f;
	public float slowRate = 0.85f;

	public bool isUpdateWhenExit = false;
	public bool isOnlyNontrigger = true;


	public float handSenseRange = 0;
	// Use this for initialization
	void Start () {
		collider.isTrigger = true;
	}
	
	void OnTriggerEnter( Collider collider)
	{
		Debug.Log("OnTriggerEnter tag" + collider.gameObject.tag);
		if ( collider.isTrigger && isOnlyNontrigger )
		{
			//do nothing
		}else 
		{
			if ( senseTags.Contains( collider.tag ) )
			{
				if ( isSendMessage )
				{
					MessageEventArgs msg = new MessageEventArgs();
					msg.AddMessage("msg" , message );
					msg.AddMessage("pos" , Global.V32Str( transform.position ) );
					BEventManager.Instance.PostEvent( EventDefine.OnTriggerable , msg );
				}

				if ( isBarrier || isCenterBarrier )
				{
					if ( collider.gameObject.rigidbody != null )
					{
	//					Vector3 toward = -collider.gameObject.rigidbody.velocity;
	//					if ( rigidbody.velocity.sqrMagnitude > barrierVelocityLimit )
	//						rigidbody.velocity = rigidbody.velocity.normalized * barrierVelocityLimit;
						if ( isCenterBarrier )
						{
							barrierDirection = collider.gameObject.transform.position - transform.position;
						}
						collider.gameObject.rigidbody.AddForce( barrierDirection.normalized * barrierIntense , ForceMode.Impulse);

					}

				}else if ( isSlowBarrier )
				{
					collider.gameObject.rigidbody.velocity *= slowRate;
				}
				else
				{
					obj = collider.gameObject;

				}

				if ( isDestroyOnEnter )
				{
					AutoDestory autoDestory = gameObject.AddComponent<AutoDestory>();
					autoDestory.destroyTime = destoryTime;
					autoDestory.isFadeOut = true;
					autoDestory.isStopParticle = true;
					autoDestory.isBecomeSmall = isSmallOnDestory;
					autoDestory.StartAutoDestory();
					isDestroyOnEnter = false;
					
					if ( DestoryEffect != null )
					{
						GameObject Deffect = Instantiate ( DestoryEffect ) as GameObject;
						Deffect.transform.position = gameObject.transform.position;
						//						AutoDestory DDestory = Deffect.AddComponent<AutoDestory>();
						//						autoDestory.destroyTime = Global.ObjDestroyTime;
						//						autoDestory.isStopParticle = true;
						//						autoDestory.StartAutoDestory();
						
					}

					if ( isDisableOnDestroy )
					{
						isSendMessage = false;
						isBarrier = false;
					}
					if ( isRefill )
					{
						SendMessageUpwards("RefillObj" , SendMessageOptions.DontRequireReceiver );
						isRefill = false;
					}
				}

				if ( isShrinkOnEnter )
				{
					MessageEventArgs shrinkHandMessage = new MessageEventArgs();
					shrinkHandMessage.AddMessage("both" , "1" );
					BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , shrinkHandMessage );
				}
			}
		}
	}
	

//	void OnTriggerExit( Collider collider )
//	{
//		if ( isUpdateWhenExit )
//			if ( senseTags.Contains( collider.tag ) && collider.gameObject == obj )
//				{}
//	}


}
