using UnityEngine;
using System.Collections;

public class WhileBallAI : BallAI {

	private float tempDivertAngle = -2;

	Vector3 getDivertDirection()
	{
		if ( tempDivertAngle < -1 )
		{
			tempDivertAngle = Random.Range( -1f , 1f );
		}
		Vector3 speed = rigidbody.velocity;

		return	tempDivertAngle * directionIntense * Vector3.Cross( speed , new Vector3( 0 , 0 , 1f ));
	}

	float getPulse()
	{
		tempDivertAngle = -2;
		return pulseIntense;
	}
}
