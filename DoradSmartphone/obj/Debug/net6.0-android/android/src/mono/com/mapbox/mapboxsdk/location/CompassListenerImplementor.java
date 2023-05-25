package mono.com.mapbox.mapboxsdk.location;


public class CompassListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mapbox.mapboxsdk.location.CompassListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCompassAccuracyChange:(I)V:GetOnCompassAccuracyChange_IHandler:Com.Mapbox.Mapboxsdk.Location.ICompassListenerInvoker, Naxam.Mapbox.Droid\n" +
			"n_onCompassChanged:(F)V:GetOnCompassChanged_FHandler:Com.Mapbox.Mapboxsdk.Location.ICompassListenerInvoker, Naxam.Mapbox.Droid\n" +
			"";
		mono.android.Runtime.register ("Com.Mapbox.Mapboxsdk.Location.ICompassListenerImplementor, Naxam.Mapbox.Droid", CompassListenerImplementor.class, __md_methods);
	}


	public CompassListenerImplementor ()
	{
		super ();
		if (getClass () == CompassListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Mapbox.Mapboxsdk.Location.ICompassListenerImplementor, Naxam.Mapbox.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onCompassAccuracyChange (int p0)
	{
		n_onCompassAccuracyChange (p0);
	}

	private native void n_onCompassAccuracyChange (int p0);


	public void onCompassChanged (float p0)
	{
		n_onCompassChanged (p0);
	}

	private native void n_onCompassChanged (float p0);

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
