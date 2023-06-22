package crc641931fc92dbf2edee;


public class DraggableFrameRenderer
	extends crc648afdc667cfb0dccb.FrameRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("DoradSmartphone.Controls.DraggableFrameRenderer, DoradSmartphone", DraggableFrameRenderer.class, __md_methods);
	}


	public DraggableFrameRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == DraggableFrameRenderer.class) {
			mono.android.TypeManager.Activate ("DoradSmartphone.Controls.DraggableFrameRenderer, DoradSmartphone", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public DraggableFrameRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == DraggableFrameRenderer.class) {
			mono.android.TypeManager.Activate ("DoradSmartphone.Controls.DraggableFrameRenderer, DoradSmartphone", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public DraggableFrameRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == DraggableFrameRenderer.class) {
			mono.android.TypeManager.Activate ("DoradSmartphone.Controls.DraggableFrameRenderer, DoradSmartphone", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, System.Private.CoreLib", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);

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
