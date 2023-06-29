using Android.OS;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Helpers
{
    public class MyHandler : Handler
    {        
        DashboardViewModel viewModel;

        public MyHandler(DashboardViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void HandleMessage(Message msg)
        {
            switch (msg.What)
            {
                case Constants.MESSAGE_STATE_CHANGE:
                    int newState = msg.Arg1;
                    viewModel.HandleStateChange(newState);
                    break;
                case Constants.MESSAGE_DEVICE_NAME:
                    string deviceName = msg.Data.GetString(Constants.GLASSES_NAME);
                    viewModel.HandleDeviceName(deviceName);
                    break;
                case Constants.MESSAGE_TOAST:
                    string toastMessage = msg.Data.GetString(Constants.TOAST);
                    viewModel.HandleToastMessage(toastMessage);
                    break;
                case Constants.MESSAGE_READ:
                    byte[] readBytes = (byte[])msg.Obj;
                    viewModel.HandleReceivedData(readBytes);
                    break;
                case Constants.MESSAGE_WRITE:
                    byte[] writeBytes = (byte[])msg.Obj;
                    viewModel.HandleSentData(writeBytes);
                    break;
            }
        }
    }
}
