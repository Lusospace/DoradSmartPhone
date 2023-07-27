using DoradSmartphone.Data;
using DoradSmartphone.Models;

namespace DoradSmartphone.Services
{
    public class UserService
    {
        public readonly IRepository repository;
        
        public UserService(IRepository repository)
        {
            this.repository = repository;            
        }

        public async Task<bool> SaveUser(string name, string email, string password, string phoneNumber)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                Password = password,
                PhoneNumber = phoneNumber
            };

            var exists = await repository.RecoverUserByEmail(email);
            if(exists is null)
            {
                await repository.SaveItensAsync(user);
                return true;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Email is already registered!", "Ok");
                return false;
            }
        }
    }
}
