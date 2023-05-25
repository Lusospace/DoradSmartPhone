package mono.com.mapbox.mapboxsdk.maps;


public class MapboxMap_OnRotateListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mapbox.mapboxsdk.maps.MapboxMap.OnRotateListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onRotate:(Lcom/mapbox/android/gestures/RotateGestureDetector;)V:GetOnRotate_Lcom_mapbox_android_gestures_RotateGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnRotateListenerInvoker, Naxam.Mapbox.Droid\n" +
			"n_onRotateBegin:(Lcom/mapbox/android/gestures/RotateGestureDetector;)V:GetOnRotateBegin_Lcom_mapbox_android_gestures_RotateGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnRotateListenerInvoker, Naxam.Mapbox.Droid\n" +
			"n_onRotateEnd:(Lcom/mapbox/android/gestures/RotateGestureDetector;)V:GetOnRotateEnd_Lcom_mapbox_android_gestures_RotateGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnRotateListenerInvoker, Naxam.Mapbox.Droid\n" +
			"";
		mono.android.Runtime.register ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnRotateListenerImplementor, Naxam.Mapbox.Droid", MapboxMap_OnRotateListenerImplementor.class, __md_methods);
	}


	public MapboxMap_OnRotateListenerImplementor ()
	{
		super ();
		if (getClass () == MapboxMap_OnRotateListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnRotateListenerImplementor, Naxam.Mapbox.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onRotate (com.mapbox.android.gestures.RotateGestureDetector p0)
	{
		n_onRotate (p0);
	}

	private native void n_onRotate (com.mapbox.android.gestures.RotateGestureDetector p0);


	public void onRotateBegin (com.mapbox.android.gestures.RotateGestureDetector p0)
	{
		n_onRotateBegin (p0);
	}

	private native void n_onRotateBegin (com.mapbox.android.gestures.RotateGestureDetector p0);


	public void onRotateEnd (com.mapbox.android.gestures.RotateGestureDetector p0)
	{
		n_onRotateEnd (p0);
	}

	private native void n_onRotateEnd (com.mapbox.android.gestures.RotateGestureDetector p0);

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
