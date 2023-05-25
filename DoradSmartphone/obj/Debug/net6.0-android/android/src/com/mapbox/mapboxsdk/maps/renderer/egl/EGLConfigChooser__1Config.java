package com.mapbox.mapboxsdk.maps.renderer.egl;


public class EGLConfigChooser__1Config
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.lang.Comparable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_compareTo:(Ljava/lang/Object;)I:GetCompareTo_Ljava_lang_Object_Handler:Java.Lang.IComparableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Com.Mapbox.Mapboxsdk.Maps.Renderer.Egl.EGLConfigChooser+_1Config, Naxam.Mapbox.Droid", EGLConfigChooser__1Config.class, __md_methods);
	}


	public EGLConfigChooser__1Config ()
	{
		super ();
		if (getClass () == EGLConfigChooser__1Config.class)
			mono.android.TypeManager.Activate ("Com.Mapbox.Mapboxsdk.Maps.Renderer.Egl.EGLConfigChooser+_1Config, Naxam.Mapbox.Droid", "", this, new java.lang.Object[] {  });
	}


	public int compareTo (java.lang.Object p0)
	{
		return n_compareTo (p0);
	}

	private native int n_compareTo (java.lang.Object p0);

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
