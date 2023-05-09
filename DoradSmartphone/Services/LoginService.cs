using DoradSmartphone.Data;

namespace DoradSmartphone.Services
{
    public class LoginService
    {
        private readonly DatabaseConn _databaseConn;
        public LoginService(DatabaseConn databaseConn)
        {
            _databaseConn = databaseConn;
        }

        public async Task VerifyLogin(string username, string password)
        {
            try
            {
                var user = await _databaseConn.GetUserByEmail(username);
                if (user is null)
                {
                    await DisplayErrorMessage("No user were found!", "OK");
                }
                if(user.Password != password)
                {
                    await DisplayErrorMessage("Wrong password!", "OK");
                }               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        async Task DisplayErrorMessage(string msg, string action) => await Shell.Current.DisplayAlert("Invalid Attempt", msg, action);
    }
}
