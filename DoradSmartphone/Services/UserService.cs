using DoradSmartphone.Data;
using DoradSmartphone.Models;

namespace DoradSmartphone.Services
{
    public class UserService
    {
        public readonly IRepository _repository;
        public UserService(IRepository repository) {
            _repository = repository;
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
            await _repository.SaveItensAsync(user);
        }
    }
}
