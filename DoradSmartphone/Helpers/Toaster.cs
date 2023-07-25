using Android.Widget;

namespace DoradSmartphone.Helpers
{
    public static class Toaster
    {
        public static void MakeToast(string message)
        {
            Toast.MakeText(Platform.CurrentActivity, message, ToastLength.Long).Show();            
        }
    }
}
