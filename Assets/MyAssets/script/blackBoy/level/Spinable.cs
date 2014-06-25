using UnityEngine;
using System.Collections;
using System;
using Holoville.HOTween;

public class Spinable : Catchable {

	public enum SpinType
	{
		Continuous,
		Loop,
	}
	
	public enum TestAccumulateType{
		Time,
		Energy,
	}
	public TestAccumulateType testAccumulateType;
	public SpinType type;
	float OriginalAngle = 0;
	Vector3 catchPosition;
	bool isReadySpin = false;

	public Transform spinPoint;
	public float spinLimit = 0.03f;
	public float spinTime = 1f;
	public float testTime = 1f;
	public float testEnergy = 0.05f;
	private float nowTestTime = 0f;
	private float nowTestEnergy = 0f;
	public bool isShrink = true;
	public bool isShrinkOnFinish = true;

	public float spinAngle = 45f;
	public int spinCount = -1;

	public enum TestType{
		Larger,
		Smallar,
	}
	public TestType testType;



	public enum TestSpinType{
		None,
		Shake,
		SpinLittle
	}
	public TestSpinType readySpinType;
	public float shakeRate = 0.1f;
	public float spinLittleRate = 0.1f;

	void Awake () {
		OriginalAngle = transform.rotation.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
		if ( isReadySpin )
			dealSpin( Time.deltaTime);
	}

	public override void DealCatch( MessageEventArgs msg )
	{	
		Debug.Log("Spin DealCatch " + getID() + gameObject.name);
		char[] split = {' '};
		string[] pos =  msg.GetMessage("CatchPosition").Split( split , StringSplitOptions.RemoveEmptyEntries );
		catchPosition = new Vector3( float.Parse(pos[0]) , float.Parse(pos[1]) , float.Parse(pos[2]) );
		ReadySpin();
	}

	public override void DealShrink( MessageEventArgs msg )
	{
		Debug.Log("Spin DealShrink " + getID() + gameObject.name);
		EndSpin();
	}

	public void ReadySpin()
	{
		isReadySpin = true;
	}

	public void EndSpin()
	{
		isReadySpin = false;
	}

	public void dealSpin( float deltaTime)
	{
//		Vector3 toHero = BObjManager.Instance.Hero.transform.position - transform.position;
//		Vector3 force = - BObjManager.Instance.BHeroBody.getForce();
//		Vector3 NForce = Vector3.Dot( toHero.normalized , force ) * toHero.normalized;
//		NForce.z = 0 ;

		Vector3 NForce = BObjManager.Instance.BHeroBody.GetHandByID( Convert.ToInt32( HandID )).getForceN();
		switch( forceType )
		{
		case ForceType.In:
			break;
		case ForceType.Out:
			NForce *= -1f;
			break;
		}
		Vector3 point = transform.position;
		if ( spinPoint != null )
			point = spinPoint.transform.position;
		Vector3 forceArm = catchPosition - point;
		forceArm.z = 0;
		Vector3 torque = Vector3.Cross( forceArm , NForce );
		GUIDebug.add(ShowType.label , "NForce " + NForce * 10 );
		GUIDebug.add( ShowType.label , "Arm " + forceArm );
		GUIDebug.add(ShowType.label , "Torque " + torque * 10 + " " + torque.z );

		//test spin
		if ( TestReady ( torque ) )
		{
			nowTestTime += deltaTime;
			nowTestEnergy += deltaTime * Math.Abs( torque.z ) ;
			DoTestSpin( deltaTime );
		}else
		{
			nowTestTime = 0f;
			nowTestEnergy = 0f;
			EndTestSpin();
		}
		if ( TestSpin() )
		{
			Spin ();
			nowTestTime = 0f;
			nowTestEnergy = 0f;
		}

	}

	public bool TestSpin()
	{
		switch( testAccumulateType )
		{
		case TestAccumulateType.Energy:
			return nowTestEnergy > testEnergy;
		case TestAccumulateType.Time:
			return nowTestTime > testTime;
		}
		return false;
	}

	public bool TestReady( Vector3 torque )
	{
		switch( testType )
		{
		case TestType.Larger:
			return torque.z > spinLimit;
		case TestType.Smallar:
			return torque.z < spinLimit;
		}
		return false;
	}

	public void DoTestSpin(float deltaTime )
	{
		switch ( readySpinType )
		{
		case TestSpinType.Shake:
			float ranX = UnityEngine.Random.Range( - testTime * shakeRate , testTime * shakeRate );
			float ranY = UnityEngine.Random.Range( - testTime * shakeRate , testTime * shakeRate );
			transform.position += new Vector3( ranX , ranY , 0 );
			break;
		case TestSpinType.SpinLittle:
			Quaternion from = transform.rotation;
			Quaternion to = Quaternion.Euler( ( from.eulerAngles + new Vector3( 0 , 0 , spinAngle * spinLittleRate ) ) );
			HOTween.To( transform , spinTime * spinLittleRate , new TweenParms().Prop("rotation" , to ).Ease(EaseType.EaseInOutBack).Loops(2, LoopType.Yoyo));
			break;

		default:
			break;
		}
	}

	public void EndTestSpin()
	{

	}

	public void Spin()
	{
		//set the spin animation
		Debug.Log("Spin" );
		EndSpin();
		Quaternion from = transform.rotation;
		Quaternion to = Quaternion.Euler( ( from.eulerAngles + new Vector3( 0 , 0 , spinAngle ) ) );
		HOTween.To( transform , spinTime , new TweenParms().Prop("rotation" , to ).Ease(EaseType.EaseInOutBack)
		           .OnComplete(this.gameObject,"ReadySpin"));

		Debug.Log("After hotween");
		//send the message to shrink the hands
		if ( isShrink )
		{
			MessageEventArgs msg = new MessageEventArgs();
			msg.AddMessage("HandID" , HandID );
			BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , msg );
		}

		//set spin count 
		if ( spinCount > 0 )
			spinCount--;
		if ( spinCount == 0 )
		{
			enabled = false;
			isReadySpin = false;
			forceType = ForceType.In;
			if ( isShrinkOnFinish )
			{
				MessageEventArgs msg = new MessageEventArgs();
				msg.AddMessage("HandID" , HandID );
				BEventManager.Instance.PostEvent( EventDefine.OnShrinkHand , msg );
			}
			{
				MessageEventArgs msg = new MessageEventArgs();
				msg.AddMessage("CatchableID" , getID().ToString() );
				msg.AddMessage("Name" , gameObject.name );
				BEventManager.Instance.PostEvent( EventDefine.OnSpinFinish , msg );
			}

		}

		//reset the test type
		switch( type )
		{
		case SpinType.Continuous:
			break;
		case SpinType.Loop:
			testType = switchTestType( testType );
			spinLimit = - spinLimit;
			spinAngle = - spinAngle;
			break;
		default:
			break;
		}
	}

	TestType switchTestType( TestType type)
	{
		switch( type )
		{
		case TestType.Larger:
			return TestType.Smallar;
		case TestType.Smallar:
			return TestType.Larger;
		}
		return type;
	}
}
