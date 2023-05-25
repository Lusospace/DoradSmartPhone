package mono.com.mapbox.mapboxsdk.maps;


public class MapboxMap_OnMoveListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mapbox.mapboxsdk.maps.MapboxMap.OnMoveListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMove:(Lcom/mapbox/android/gestures/MoveGestureDetector;)V:GetOnMove_Lcom_mapbox_android_gestures_MoveGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnMoveListenerInvoker, Naxam.Mapbox.Droid\n" +
			"n_onMoveBegin:(Lcom/mapbox/android/gestures/MoveGestureDetector;)V:GetOnMoveBegin_Lcom_mapbox_android_gestures_MoveGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnMoveListenerInvoker, Naxam.Mapbox.Droid\n" +
			"n_onMoveEnd:(Lcom/mapbox/android/gestures/MoveGestureDetector;)V:GetOnMoveEnd_Lcom_mapbox_android_gestures_MoveGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnMoveListenerInvoker, Naxam.Mapbox.Droid\n" +
			"";
		mono.android.Runtime.register ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnMoveListenerImplementor, Naxam.Mapbox.Droid", MapboxMap_OnMoveListenerImplementor.class, __md_methods);
	}


	public MapboxMap_OnMoveListenerImplementor ()
	{
		super ();
		if (getClass () == MapboxMap_OnMoveListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnMoveListenerImplementor, Naxam.Mapbox.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onMove (com.mapbox.android.gestures.MoveGestureDetector p0)
	{
		n_onMove (p0);
	}

	private native void n_onMove (com.mapbox.android.gestures.MoveGestureDetector p0);


	public void onMoveBegin (com.mapbox.android.gestures.MoveGestureDetector p0)
	{
		n_onMoveBegin (p0);
	}

	private native void n_onMoveBegin (com.mapbox.android.gestures.MoveGestureDetector p0);


	public void onMoveEnd (com.mapbox.android.gestures.MoveGestureDetector p0)
	{
		n_onMoveEnd (p0);
	}

	private native void n_onMoveEnd (com.mapbox.android.gestures.MoveGestureDetector p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
