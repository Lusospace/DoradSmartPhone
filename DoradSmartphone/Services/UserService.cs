using DoradSmartphone.Data;
using DoradSmartphone.Models;

namespace DoradSmartphone.Services
{
    public class UserService
    {
        public readonly IRepository _repository;
        public readonly DatabaseConn _databaseConn;
        public UserService(IRepository repository, DatabaseConn databaseConn = null)
        {
            _repository = repository;
            _databaseConn = databaseConn;
        }

        public async Task SaveUser(string name, string email, string password, string phoneNumber)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                Password = password,
                PhoneNumber = phoneNumber
            };

            var exists = await _databaseConn.GetUserByEmail(email);
            if(exists is null)
            {
                await _repository.SaveItensAsync(user);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Email is already registered!", "Ok");
            }
        }
    }
}
