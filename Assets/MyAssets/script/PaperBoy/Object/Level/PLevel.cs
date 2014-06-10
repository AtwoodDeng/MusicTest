using UnityEngine;
using System.Collections;

public abstract class PLevel : MonoBehaviour {

	public abstract int GetLevelID();
	public abstract void SwitchOut();
	public abstract void SwitchIn();
}
