using UnityEngine;
using System.Collections;

public class BLevel0 : BLevel {

	public enum State{
		Init,
		End,
	}
	public State state = State.Init;

	public override void DealWith (float deltaTime)
	{
		switch( state )
		{
		case State.Init:
			break;
		}
	}

	public override void DealTrigger (string msg)
	{

	}
}
