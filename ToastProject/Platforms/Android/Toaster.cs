using Android.Widget;

namespace ToastProject.Platforms
{

    public class Toaster : IToast
    {
        public void MakeToast(string message)
        {
            Toast.MakeText(Platform.CurrentActivity, message, ToastLength.Long).Show();
        }
    }
}
