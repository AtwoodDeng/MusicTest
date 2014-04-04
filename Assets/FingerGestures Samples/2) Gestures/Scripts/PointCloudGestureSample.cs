using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This sample demonstrates how to use the PointCloudGestureRecognizer to recognize custom gestures from a list of templates
/// </summary>
[RequireComponent(typeof(PointCloudRegognizer))]
public class PointCloudGestureSample : SampleBase
{
    public PointCloudGestureRenderer GestureRendererPrefab;
    public float GestureScale = 8.0f;
    public Vector2 GestureSpacing = new Vector2( 1.25f, 1.0f );
    public int MaxGesturesPerRaw = 2;
	public Vector3 GestureRootPos ;
	public static int[] OrderList = {2,1,3,0,4};

    List<MyGestureRender> gestureRenderers;
    
    protected override void Start()
    {
        base.Start();
        RenderGestureTemplates();
    }

    // Message send by the PointCloudRecognizer when it recognized a valid gesture pattern
    void OnCustomGesture( PointCloudGesture gesture )
    {
		OnCustomGesture (gesture.RecognizedTemplate.name);
//        string scorePercentText = ( gesture.MatchScore * 100 ).ToString( "N2" );
//
//        UI.StatusText = "Matched " + gesture.RecognizedTemplate.name + " (score: " + scorePercentText + "% distance:" + gesture.MatchDistance.ToString( "N2" ) + ")";
//        Debug.Log( UI.StatusText );
//
//        // make the corresponding gesture visualizer blink
//        MyGestureRender gr = FindGestureRenderer( gesture.RecognizedTemplate );
//		Debug.Log ("find : " + gr);
//        if( gr )
//            gr.Blink();
    }

	public void OnCustomGesture( string name )
	{

		//string scorePercentText = ( gesture.MatchScore * 100 ).ToString( "N2" );
		
		//UI.StatusText = "Matched " + gesture.RecognizedTemplate.name + " (score: " + scorePercentText + "% distance:" + gesture.MatchDistance.ToString( "N2" ) + ")";
		//Debug.Log( UI.StatusText );
		
		// make the corresponding gesture visualizer blink

		//Debug.Log ("On Custom Gesture " + name);

		if ( CheckGestureOrder( name ))
		{
			MyGestureRender gr = FindGestureRenderer( name );
			Debug.Log ("find : " + gr);
			gr.StartParticle();
		}else
		{
			MyGestureRender gr = FindGestureRenderer( name );
			Debug.Log ("find : " + gr);
			gr.Blink();
		}
	}

	bool CheckGestureOrder( string name )
	{
		for( int i = 0 ;i < gestureRenderers.Count ; ++i )
		{
			int order = OrderList[i];
			if ( gestureRenderers[order].getName() == name )
			{
				Debug.Log( "Find Gesture! " + name ) ;
				return true;
			}else
			{
				if ( gestureRenderers[order].isOn() )
					continue;
				else
					{
					Debug.Log("Error Gesture " + gestureRenderers[order].getName() + " expected but " 
						          + name + " found " ); 
					return false;
					}
			}
		}
		return false;
	}

    void OnFingerDown( FingerDownEvent e ) 
    {
        UI.StatusText = string.Empty;
    }
   
    #region Misc

    void RenderGestureTemplates()
    {
        gestureRenderers = new List<MyGestureRender>();

        Transform gestureRoot = new GameObject( "Gesture Templates" ).transform;
        gestureRoot.parent = this.transform;
        gestureRoot.localScale = GestureScale * Vector3.one;

        PointCloudRegognizer recognizer = GetComponent<PointCloudRegognizer>();
        Vector3 pos = Vector3.zero;
        int gesturesOnRow = 0;
        int rows = 0;
        float rowWidth = 0;
        
        foreach( PointCloudGestureTemplate template in recognizer.Templates )
        {
			//Debug.Log(" a template " + template.name );
            MyGestureRender gestureRenderer = Instantiate( GestureRendererPrefab, gestureRoot.position, gestureRoot.rotation ) as MyGestureRender;
            gestureRenderer.GestureTemplate = template;
            gestureRenderer.name = template.name;
            gestureRenderer.transform.parent = gestureRoot;
            gestureRenderer.transform.localPosition = pos;
            gestureRenderer.transform.localScale = Vector3.one;
            
            pos.x += GestureSpacing.x;

            rowWidth = Mathf.Max( rowWidth, pos.x );

            if( ++gesturesOnRow >= MaxGesturesPerRaw )
            {
                pos.y += GestureSpacing.y;
                pos.x = 0;
                gesturesOnRow = 0;
                rows++;
            }

            gestureRenderers.Add( gestureRenderer );
        }

        // center
        Vector3 rootPos = Vector3.zero;
        rootPos.x -= GestureScale * 0.5f * ( rowWidth - GestureSpacing.x );

        if( rows > 0 )
            rootPos.y -= GestureScale * 0.5f * ( pos.y - GestureSpacing.y );

		gestureRoot.localPosition = rootPos;
		gestureRoot.localPosition += GestureRootPos;
	}
	
	MyGestureRender FindGestureRenderer( PointCloudGestureTemplate template )
    {
        return gestureRenderers.Find( gr => gr.GestureTemplate == template );
    }

	MyGestureRender FindGestureRenderer( string name )
	{
		return gestureRenderers.Find( gr => gr.GestureTemplate.name == name );
	}

    protected override string GetHelpText()
    {
        return @"This sample demonstrates how to use the PointCloudGestureRecognizer to recognize custom gestures from a list of templates";
    }

	public void OnLoopEnd()
	{
		for (int i = 0; i < gestureRenderers.Count; ++i)
						gestureRenderers [i].StopParticle ();
	}

    #endregion 
}
