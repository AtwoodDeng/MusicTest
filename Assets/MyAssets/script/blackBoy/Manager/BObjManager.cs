using UnityEngine;
using System.Collections;

public class BObjManager : MonoBehaviour {
	
	public BObjManager() { s_Instance = this; }
	public static BObjManager Instance { get { return s_Instance; } }
	private static BObjManager s_Instance;

	public GameObject World
	{
		get {
			if ( _World == null )
			{
				_World = GameObject.Find( "World" );
			}
			return _World;
		}
	}
	GameObject _World;

	public GameObject Effect
	{
		get {
			if ( _Effect == null )
			{
				_Effect = GameObject.Find( "Effect" );
			}
			return _Effect;
		}
	}
	GameObject _Effect;

	public GameObject Hero
	{
		get {
			if (_Hero == null )
			{
				_Hero = GameObject.FindGameObjectWithTag("HERO");
			}
			return _Hero;
		}
	}	
	GameObject _Hero;

	public HeroBody BHeroBody
	{
		get {
			if (_BHeroBody == null )
			{
				_BHeroBody = Hero.GetComponent<HeroBody>();
			}
			return _BHeroBody;
		}
	}
	HeroBody _BHeroBody;

	public GameObject[] RecoverPoints
	{
		get {
			if ( _RecoverPoints == null )
			{
				_RecoverPoints = GameObject.FindGameObjectsWithTag( Global.RECOVER_POINT_TAG );
			}
			return _RecoverPoints;
		}
	}
	GameObject[] _RecoverPoints;

	public BLevel tempLevel
	{
		get {
			if ( _tempLevel == null )
			{
				_tempLevel = World.GetComponent<BLevel>();
			}
			return _tempLevel;
		}
	}
	BLevel _tempLevel;
}
