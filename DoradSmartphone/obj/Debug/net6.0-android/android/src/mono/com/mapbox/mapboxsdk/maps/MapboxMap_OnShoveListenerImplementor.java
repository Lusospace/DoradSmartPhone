package mono.com.mapbox.mapboxsdk.maps;


public class MapboxMap_OnShoveListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mapbox.mapboxsdk.maps.MapboxMap.OnShoveListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onShove:(Lcom/mapbox/android/gestures/ShoveGestureDetector;)V:GetOnShove_Lcom_mapbox_android_gestures_ShoveGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnShoveListenerInvoker, Naxam.Mapbox.Droid\n" +
			"n_onShoveBegin:(Lcom/mapbox/android/gestures/ShoveGestureDetector;)V:GetOnShoveBegin_Lcom_mapbox_android_gestures_ShoveGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnShoveListenerInvoker, Naxam.Mapbox.Droid\n" +
			"n_onShoveEnd:(Lcom/mapbox/android/gestures/ShoveGestureDetector;)V:GetOnShoveEnd_Lcom_mapbox_android_gestures_ShoveGestureDetector_Handler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnShoveListenerInvoker, Naxam.Mapbox.Droid\n" +
			"";
		mono.android.Runtime.register ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnShoveListenerImplementor, Naxam.Mapbox.Droid", MapboxMap_OnShoveListenerImplementor.class, __md_methods);
	}


	public MapboxMap_OnShoveListenerImplementor ()
	{
		super ();
		if (getClass () == MapboxMap_OnShoveListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnShoveListenerImplementor, Naxam.Mapbox.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onShove (com.mapbox.android.gestures.ShoveGestureDetector p0)
	{
		n_onShove (p0);
	}

	private native void n_onShove (com.mapbox.android.gestures.ShoveGestureDetector p0);


	public void onShoveBegin (com.mapbox.android.gestures.ShoveGestureDetector p0)
	{
		n_onShoveBegin (p0);
	}

	private native void n_onShoveBegin (com.mapbox.android.gestures.ShoveGestureDetector p0);


	public void onShoveEnd (com.mapbox.android.gestures.ShoveGestureDetector p0)
	{
		n_onShoveEnd (p0);
	}

	private native void n_onShoveEnd (com.mapbox.android.gestures.ShoveGestureDetector p0);

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
