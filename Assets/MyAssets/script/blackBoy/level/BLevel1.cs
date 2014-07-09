using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Holoville.HOTween;

public class BLevel1 : BLevel {

	public enum State{
		Init,
		Begin,
		WakeUp,
		GetWatch,
		FatherEar,
		FatherNose,
		FatherMouth,
		FatherEye,
		FatherMoney,
		PhotoIn,
		PhotoGet,
		PhotoOut,
		TagIn,
		CatIn,
		CatOut,
		Final,
		FadeOut,
		End,
	}
	public State state = State.Init;

	List<string> msgs = new List<string>();
	int itemCount = 0;

	public ParticleSystem earSmoke;
	public ParticleSystem noseSmoke;
	public ParticleSystem mouthSmoke;
	public ParticleSystem eyeSmoke;
	public Pullable eye;
	public GameObject eye2;
	public GameObject money;

	public Triggerable w_f;
	public Triggerable f_p;
	public Triggerable p_t;
	public Triggerable t_c;

	public Spinable catBody;

	public Camera cc;
	public float fadeOutHoldTime = 8f;
	public float fadeOutTime = 15f;
	public float fadeOutTempTime = 0f;

	void OnEnable() {
		base.OnEnable();
		BEventManager.Instance.RegisterEvent (EventDefine.OnSpinFinish ,OnSpinFinish );
		BEventManager.Instance.RegisterEvent (EventDefine.OnPullFinish ,OnPullFinish );
	}
	
	void OnDisable() {
		base.OnDisable();
		BEventManager.Instance.RegisterEvent (EventDefine.OnSpinFinish ,OnSpinFinish );
		BEventManager.Instance.RegisterEvent (EventDefine.OnPullFinish ,OnPullFinish );
	}
	
	public void OnSpinFinish(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		string name = msg.GetMessage("Name");
		if ( name == null )
			return;
		if ( "ear".Equals(name))
		{
			earSmoke.enableEmission = true;
			earSmoke.Emit( 25 );
			msgs.Add("get_ear");
		}
		if ( "nose".Equals(name))
		{
			noseSmoke.enableEmission = true;
			noseSmoke.Emit( 25 );
			msgs.Add("get_nose");
		}
	}

	public void OnPullFinish(EventDefine eventName, object sender, EventArgs args)
	{
		MessageEventArgs msg = (MessageEventArgs)args;
		string name = msg.GetMessage("Name");
		if ( name == null )
			return;
		if ( "mouth2".Equals(name))
		{
			msgs.Add("get_mouth");
			mouthSmoke.enableEmission = true;
			mouthSmoke.Emit( 25 );
		}
		if ( "eye".Equals(name))
		{
			msgs.Add("get_eye");
			eyeSmoke.enableEmission = true;
			eyeSmoke.Emit( 25 );
		}
	}

	void Update()
	{
		DealWith(Time.deltaTime);
		GUIDebug.add( ShowType.label , state.ToString());
	}

	public override void DealWith(float deltaTime)
	{
		switch( state )
		{
		case State.Init:
			state = State.Begin;
			itemCount = 0;
			string text = BDataManager.Instance.getDialogWithKey( levelName , "AIM"  );
			text += "(" + itemCount.ToString() + "/6)";
			showTips( text );
			break;
		case State.Begin:
			showNextDialogGroup();
//			showNextDialog(4);
			state = State.WakeUp;
			break;
		case State.WakeUp:
			if ( msgs.Contains("wakeup"))
			{
				showNextDialogGroup();
//				showNextDialog(3);
				state = State.GetWatch;
			}
			break;
		case State.GetWatch:
			if ( msgs.Contains("get_watch" ))
			{
				w_f.isBarrier = false;
				state = State.FatherEar;
			}
			break;
		case State.FatherEar:
			if ( msgs.Contains("get_ear"))
			{
				state = State.FatherNose;
				
				showNextDialogGroup();
//				showNextDialog();
			}
			break;
			
		case State.FatherNose:
			if ( msgs.Contains("get_nose"))
			{
				state = State.FatherMouth;
				
				showNextDialogGroup();
//				showNextDialog();
			}
			break;
			
		case State.FatherMouth:
			if ( msgs.Contains("get_mouth"))
			{
				eye.enabled = true;
				state = State.FatherEye;
				
				showNextDialogGroup();
//				showNextDialog();
			}
			break;
			
		case State.FatherEye:
			if ( msgs.Contains("get_eye"))
			{
				HOTween.To( eye2.transform
				           , 2f 
				           , "eulerAngles"
				           , new Vector3( 0 , 0 , 52f )
				           , true
				           , EaseType.EaseInOutBack
				           , 0 );
				HOTween.To( money.transform
				           , 2f 
				           , "position"
				           , new Vector3( 2.8f , -1.5f , 0 )
				           , true
				           , EaseType.EaseOutQuart
				           , 0 );
				state = State.FatherMoney;
				
				showNextDialogGroup();

//				showNextDialog(3);
			}
			break;
			
		case State.FatherMoney:
			if ( msgs.Contains("get_money"))
			{
				f_p.isBarrier = false;
				state = State.PhotoIn;
			}
			break;
		case State.PhotoIn:
			if ( msgs.Contains("enter_photo"))
			{
				
				showNextDialogGroup();
//				showNextDialog(5);
				state = State.PhotoGet;
			}
			break;
		case State.PhotoGet:
			if ( msgs.Contains("get_photo"))
			{
				
				showNextDialogGroup();
//				showNextDialog();
				state = State.TagIn;
				p_t.isBarrier = false;
			}
			break;
		case State.TagIn:
			if ( msgs.Contains("enter_tag"))
			{
				
				showNextDialogGroup();
//				showNextDialog( 4 );
				state = State.CatIn;
				t_c.isBarrier = false;
			}
			break;
		case State.CatIn:
			if ( msgs.Contains("enter_cat"))
			{
				
				showNextDialogGroup();
//				showNextDialog( 5 );
				
				HOTween.To( catBody.transform
				           , 40f
				           , new TweenParms()
				           .Prop("eulerAngles" , new Vector3( 0 , 0 , 322f ) , true )
				           .Ease( EaseType.EaseInOutSine)
				           .Loops( 100 , LoopType.Incremental));
				BObjManager.Instance.BHeroBody.ShrinkHand( true );
				BObjManager.Instance.BHeroBody.ShrinkHand( false );
				BObjManager.Instance.BHeroBody.transform.position = catBody.transform.position;
				state =  State.Final;
			}
			break;
		case State.Final:
			if ( itemCount >= 6 )
			{
				
				showNextDialogGroup();
//				showNextDialog( 4 );
				state = State.FadeOut;
				fadeOutTempTime = 0;
			}
			break;
		case State.FadeOut:
			
			if ( cc == null )
				cc= Camera.main.GetComponent<Camera>();
			if ( fadeOutTempTime < fadeOutTime + fadeOutHoldTime )
			{
				if ( fadeOutTempTime > fadeOutHoldTime )
				{
					cc.orthographicSize = ( 80 - 3 ) / Mathf.Pow( fadeOutTime , 3f ) * Mathf.Pow( fadeOutTempTime - fadeOutHoldTime , 3f ) + 3;
				}
				fadeOutTempTime += deltaTime;

			}else
			{
				state = State.End;
			}
			break;
		default:
			break;
		}
	}

	public override void DealTrigger (string msg)
	{

		if ( msgs.Contains( msg ))
		{
			return;
		}

		msgs.Add( msg );
		if ( "get_watch".Equals(msg) )
		{
				itemCount ++;
				string text = BDataManager.Instance.getDialogWithKey( levelName , "GETITEM"  );
				text += BDataManager.Instance.getDialogWithKey( levelName , "WATCH"  );
				text += "(" + itemCount.ToString() + "/6)";
				showTips( text );

		}
		if ( "get_money".Equals(msg) )
		{
			itemCount ++;
			string text = BDataManager.Instance.getDialogWithKey( levelName , "GETITEM"  );
			text += BDataManager.Instance.getDialogWithKey( levelName , "MONEY"  );
			text += "(" + itemCount.ToString() + "/6)";
			showTips( text );
			
		}
		if ( "get_photo".Equals(msg) )
		{
			itemCount ++;
			string text = BDataManager.Instance.getDialogWithKey( levelName , "GETITEM"  );
			text += BDataManager.Instance.getDialogWithKey( levelName , "PHOTO"  );
			text += "(" + itemCount.ToString() + "/6)";
			showTips( text );
			
		}
		
		if ( "get_walkman".Equals(msg) )
		{
			itemCount ++;
			string text = BDataManager.Instance.getDialogWithKey( levelName , "GETITEM"  );
			text += BDataManager.Instance.getDialogWithKey( levelName , "WALKMAN"  );
			text += "(" + itemCount.ToString() + "/6)";
			showTips( text );
			
		}
		
		if ( "get_shirt".Equals(msg) )
		{
			itemCount ++;
			string text = BDataManager.Instance.getDialogWithKey( levelName , "GETITEM"  );
			text += BDataManager.Instance.getDialogWithKey( levelName , "SHIRT"  );
			text += "(" + itemCount.ToString() + "/6)";
			showTips( text );
			
		}
		
		if ( "get_cellphone".Equals(msg) )
		{
			itemCount ++;
			string text = BDataManager.Instance.getDialogWithKey( levelName , "GETITEM"  );
			text += BDataManager.Instance.getDialogWithKey( levelName , "CELLPHONE"  );
			text += "(" + itemCount.ToString() + "/6)";
			showTips( text );
			
		}

		if ( "get_coat".Equals(msg) )
		{
			string text = BDataManager.Instance.getDialogWithKey( levelName , "COAT"  );
			showText( text );
		}

		if ( "get_backpack".Equals(msg) )
		{
			string text = BDataManager.Instance.getDialogWithKey( levelName , "BACKPACK"  );
			showText( text );
		}

	}

}
