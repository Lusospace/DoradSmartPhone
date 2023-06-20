namespace DoradSmartphone.Controls
{
    public class DraggableFrame : Frame
    {
        public static readonly BindableProperty IsDraggingProperty =
            BindableProperty.Create(nameof(IsDragging), typeof(bool), typeof(DraggableFrame), false);

        public bool IsDragging
        {
            get => (bool)GetValue(IsDraggingProperty);
            set => SetValue(IsDraggingProperty, value);
        }
    }
}
