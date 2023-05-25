package mono.com.mapbox.mapboxsdk.maps;


public class MapboxMap_OnScaleListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mapbox.mapboxsdk.maps.MapboxMap.OnScaleListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onScale:(Lcom/mapbox/android/gestures/StandardScaleGestureDetector;)V:GetOnScale_Lcom_mapbox_android_gestures_StandardScaleGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnScaleListenerInvoker, Naxam.Mapbox.Droid\n" +
			"n_onScaleBegin:(Lcom/mapbox/android/gestures/StandardScaleGestureDetector;)V:GetOnScaleBegin_Lcom_mapbox_android_gestures_StandardScaleGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnScaleListenerInvoker, Naxam.Mapbox.Droid\n" +
			"n_onScaleEnd:(Lcom/mapbox/android/gestures/StandardScaleGestureDetector;)V:GetOnScaleEnd_Lcom_mapbox_android_gestures_StandardScaleGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnScaleListenerInvoker, Naxam.Mapbox.Droid\n" +
			"";
		mono.android.Runtime.register ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnScaleListenerImplementor, Naxam.Mapbox.Droid", MapboxMap_OnScaleListenerImplementor.class, __md_methods);
	}


	public MapboxMap_OnScaleListenerImplementor ()
	{
		super ();
		if (getClass () == MapboxMap_OnScaleListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnScaleListenerImplementor, Naxam.Mapbox.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onScale (com.mapbox.android.gestures.StandardScaleGestureDetector p0)
	{
		n_onScale (p0);
	}

	private native void n_onScale (com.mapbox.android.gestures.StandardScaleGestureDetector p0);


	public void onScaleBegin (com.mapbox.android.gestures.StandardScaleGestureDetector p0)
	{
		n_onScaleBegin (p0);
	}

	private native void n_onScaleBegin (com.mapbox.android.gestures.StandardScaleGestureDetector p0);


	public void onScaleEnd (com.mapbox.android.gestures.StandardScaleGestureDetector p0)
	{
		n_onScaleEnd (p0);
	}

	private native void n_onScaleEnd (com.mapbox.android.gestures.StandardScaleGestureDetector p0);

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
