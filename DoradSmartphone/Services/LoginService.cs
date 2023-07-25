using DoradSmartphone.Data;
using DoradSmartphone.Models;
using DoradSmartphone.Views;
using Newtonsoft.Json;

namespace DoradSmartphone.Services
{
    public class LoginService
    {
        private readonly IRepository repository;
        public LoginService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task VerifyLogin(string username, string password)
        {
            try
            {
                var user = await repository.RecoverUserByEmail(username);
                if (user is null)
                {
                    await DisplayErrorMessage("No user were found!", "OK");
                }
                if(user.Password != password)
                {
                    await DisplayErrorMessage("Wrong password!", "OK");
                }
                else
                {
                    // Create the user session
                    var userSession = new UserSession
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email
                        // Set other relevant user properties
                    };

                    // Store the user session using Preferences
                    Preferences.Set("UserSession", JsonConvert.SerializeObject(userSession));

                    Preferences.Set("UserLoggedIn", true);
                    await Shell.Current.GoToAsync($"//{nameof(GlassPage)}");
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
