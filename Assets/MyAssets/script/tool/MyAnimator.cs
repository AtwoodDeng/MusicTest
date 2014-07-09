using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class MyAnimator : MonoBehaviour {

	public enum ShowType
	{
		Merge,
		Switch,
	}
	public ShowType showType;

	public List<float> timeLine;
	public List<float> fadeTimeIn;
	public List<float> fadeTimeOut;
	public List<string> spriteNames;
	//public List<string> extraSpriteNames;
	//public string collectionName;

	public tk2dSprite sprite;
	public tk2dSprite extraSprite;
	private int index = 0 ;

	private float time = -0.01f;

	private string tempSpriteName;
	private float tempTime;

	public EaseType fadeInEaseType;
	public EaseType fadeOutEaseType;

	public bool ifPlayOnAwake = false;
	private bool ifPlay = false;


	public enum LoopType
	{
		PingPong,
		Loop,
	}
	public LoopType loopType;

	void Awake() {
		if ( sprite == null )
			sprite = this.GetComponent<tk2dSprite>();
		if ( spriteNames.Count > 0 )
		{
			tempSpriteName = spriteNames[index];
			sprite.SetSprite( tempSpriteName );
		}
//		if ( extraSpriteNames.Count > 0 && extraSprite != null )
//		{
//			extraSprite.SetSprite( extraSpriteNames[index] );
//		}

		if ( ifPlayOnAwake )
			StartPlay();
	}

	// Update is called once per frame
	void Update () {
		if ( ifPlay )
		{
			time += Time.deltaTime;
			if ( time > tempTime )
			{
				time = 0;
				setAnimator( getNextIndex(true ));
			}
		}
	}

	void StartPlay()
	{
		index = 0;
		time = 0;
		ifPlay = true;

		setAnimator(index);
	}

	void setAnimator( int index )
	{
		if ( timeLine.Count <= index || fadeTimeIn.Count <= index || fadeTimeOut.Count <= index ||
		    spriteNames.Count <= index )
		{
			Debug.LogError("[MyAnimator] index out of range");
		}


//		if ( extraSpriteNames.Count > index && extraSprite != null )
//		{
//			extraSprite.SetSprite( extraSpriteNames[index] );
//		}

		//set animater of sprite
		if ( showType == ShowType.Switch )
		{
			tempTime = timeLine[index];
			float fadeInTime = fadeTimeIn[index];
			float fadeOutTime = fadeTimeOut[index];
			
			sprite.SetSprite(spriteNames[index]);

			if ( fadeInTime + fadeOutTime > tempTime )
				fadeInTime = fadeOutTime = tempTime / 2;

			Color colorIn = sprite.color;
			Color colorOut = colorIn;
			if ( fadeOutTime > 0 )
				colorOut.a = 0f;

			if ( fadeInTime > 0 )
			{
				Color colorBI = sprite.color;
				colorBI.a = 0;
				sprite.color = colorBI;
				colorIn.a = 1f;
			}

			//sprite.color = colorOut;

			HOTween.To( sprite ,
			           fadeInTime ,
			           "color" ,
			           colorIn ,
			           false ,
			           fadeInEaseType ,
			           0 );

			HOTween.To( sprite ,
			           fadeOutTime ,
			           "color" ,
			           colorOut ,
			           false ,
			           fadeOutEaseType ,
			           tempTime - fadeOutTime );
		}

		if ( showType == ShowType.Merge &&  extraSprite != null )
		{
			tempTime = timeLine[index];
			float fadeInTime = fadeTimeIn[index];
//			float fadeOutTime = fadeTimeOut[index];
			tempSpriteName = spriteNames[index];
			
//			if ( fadeInTime + fadeOutTime > tempTime )
//				fadeInTime = fadeOutTime = tempTime / 2;
			if ( fadeInTime > tempTime )
				fadeInTime = tempTime;

			extraSprite.SetSprite( sprite.spriteId );
			sprite.SetSprite( spriteNames[index] );

			Color colorIn = sprite.color;
			Color colorOut = colorIn;
			colorIn.a = 1f;
			colorOut.a = 0;

			sprite.color = colorOut;
			extraSprite.color = colorIn;
			
			HOTween.To( sprite ,
			           fadeInTime ,
			           "color" ,
			           colorIn ,
			           false ,
			           fadeInEaseType ,
			           0 );
			
			HOTween.To( extraSprite ,
			           fadeInTime ,
			           "color" ,
			           colorOut ,
			           false ,
			           fadeOutEaseType ,
			           0 );

		}

	}

	int getNextIndex(bool ifChange = false)
	{
		switch( loopType )
		{
		case LoopType.Loop:
		{
			int i = index + 1;
			if ( i >= spriteNames.Count )
				i = 0;
			if ( ifChange )
				index = i ;
			return i;
		}
			break;
		case LoopType.PingPong:
		{
			int i = index;
			i++;
			if (i >= spriteNames.Count )
				i = - spriteNames.Count+2;
			
			if ( ifChange )
				index = i;

			if ( i < 0 )
				i = -i;
			return i;
		}
			break;
		}
		return 0;
	}

}
