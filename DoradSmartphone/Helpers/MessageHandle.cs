using DoradSmartphone.Data;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using Newtonsoft.Json;

namespace DoradSmartphone.Helpers
{
    public static class MessageHandler
    {
        public static void ProcessReceivedData(string receivedData)
        {
            try
            {
                User user = UserSessionHelper.GetUserFromSessionJson();
                Exercise smartPhoneDto = JsonConvert.DeserializeObject<Exercise>(receivedData);
                smartPhoneDto.UserId = user.Id;
                // Save to the database
                IRepository repository = new DatabaseConn();
                repository.SaveItensAsync(smartPhoneDto);                
            }
            catch (Exception ex)
            {
                Toaster.MakeToast("Error when saving into the database " + ex);
            }
        }
    }
}
