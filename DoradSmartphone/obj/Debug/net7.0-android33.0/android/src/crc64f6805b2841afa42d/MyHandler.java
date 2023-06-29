package crc64f6805b2841afa42d;


public class MyHandler
	extends android.os.Handler
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_handleMessage:(Landroid/os/Message;)V:GetHandleMessage_Landroid_os_Message_Handler\n" +
			"";
		mono.android.Runtime.register ("DoradSmartphone.Helpers.MyHandler, DoradSmartphone", MyHandler.class, __md_methods);
	}


	public MyHandler ()
	{
		super ();
		if (getClass () == MyHandler.class) {
			mono.android.TypeManager.Activate ("DoradSmartphone.Helpers.MyHandler, DoradSmartphone", "", this, new java.lang.Object[] {  });
		}
	}


	public MyHandler (android.os.Handler.Callback p0)
	{
		super (p0);
		if (getClass () == MyHandler.class) {
			mono.android.TypeManager.Activate ("DoradSmartphone.Helpers.MyHandler, DoradSmartphone", "Android.OS.Handler+ICallback, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public MyHandler (android.os.Looper p0)
	{
		super (p0);
		if (getClass () == MyHandler.class) {
			mono.android.TypeManager.Activate ("DoradSmartphone.Helpers.MyHandler, DoradSmartphone", "Android.OS.Looper, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public MyHandler (android.os.Looper p0, android.os.Handler.Callback p1)
	{
		super (p0, p1);
		if (getClass () == MyHandler.class) {
			mono.android.TypeManager.Activate ("DoradSmartphone.Helpers.MyHandler, DoradSmartphone", "Android.OS.Looper, Mono.Android:Android.OS.Handler+ICallback, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public void handleMessage (android.os.Message p0)
	{
		n_handleMessage (p0);
	}

	private native void n_handleMessage (android.os.Message p0);

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
