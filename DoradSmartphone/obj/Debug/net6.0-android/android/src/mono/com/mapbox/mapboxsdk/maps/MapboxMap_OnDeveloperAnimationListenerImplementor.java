package mono.com.mapbox.mapboxsdk.maps;


public class MapboxMap_OnDeveloperAnimationListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mapbox.mapboxsdk.maps.MapboxMap.OnDeveloperAnimationListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDeveloperAnimationStarted:()V:GetOnDeveloperAnimationStartedHandler:Com.Mapbox.Mapboxsdk.Maps.MapboxMap/IOnDeveloperAnimationListenerInvoker, Naxam.Mapbox.Droid\n" +
			"";
		mono.android.Runtime.register ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnDeveloperAnimationListenerImplementor, Naxam.Mapbox.Droid", MapboxMap_OnDeveloperAnimationListenerImplementor.class, __md_methods);
	}


	public MapboxMap_OnDeveloperAnimationListenerImplementor ()
	{
		super ();
		if (getClass () == MapboxMap_OnDeveloperAnimationListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Mapbox.Mapboxsdk.Maps.MapboxMap+IOnDeveloperAnimationListenerImplementor, Naxam.Mapbox.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onDeveloperAnimationStarted ()
	{
		n_onDeveloperAnimationStarted ();
	}

	private native void n_onDeveloperAnimationStarted ();

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
