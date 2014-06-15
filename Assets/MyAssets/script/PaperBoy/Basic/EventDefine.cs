using UnityEngine;
using System.Collections;

public enum EventDefine{
	NONE = 0,
	OnLoopend = NONE+1,
	OnDestroy = OnLoopend+1,
	OnSwitchLevel = OnDestroy+1,
	OnMouseInHero = OnSwitchLevel+1,
	OnMouseOutHero = OnMouseInHero+1,
	OnLevelStart = OnMouseOutHero+1,


	OnStrenchHand = OnLevelStart+1,
	OnShrinkHand = OnStrenchHand+1,
	OnMoveHand = OnShrinkHand+1,
	OnCatch = OnMoveHand+1,
	OnShrink = OnCatch+1,
	OnSpinFinish = OnShrink+1,
	OnPullFinish = OnSpinFinish+1,


	OnMouseClick = OnPullFinish+1,
	OnShowText = OnMouseClick+1,
}