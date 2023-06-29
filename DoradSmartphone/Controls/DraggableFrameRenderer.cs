using Android.Content;
using Android.Views;
using DoradSmartphone.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android.AppCompat;
using Microsoft.Maui.Controls.Platform;

[assembly: ExportRenderer(typeof(DraggableFrame), typeof(DraggableFrameRenderer))]
namespace DoradSmartphone.Controls
{
    public class DraggableFrameRenderer : FrameRenderer
    {
        private bool _isDragging;

        public DraggableFrameRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var frame = (DraggableFrame)e.NewElement;
                _isDragging = frame.IsDragging;
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            var frame = (DraggableFrame)Element;

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _isDragging = true;
                    frame.IsDragging = true;
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    _isDragging = false;
                    frame.IsDragging = false;
                    break;
            }

            return _isDragging || base.OnTouchEvent(e);
        }
    }
}
