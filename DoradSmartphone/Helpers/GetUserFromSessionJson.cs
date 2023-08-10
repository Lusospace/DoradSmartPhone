using DoradSmartphone.Models;
using Newtonsoft.Json;

public static class UserSessionHelper
{
    public static User GetUserFromSessionJson()
    {
        var userSessionJson = Preferences.Get("UserSession", string.Empty);
        var userSession = JsonConvert.DeserializeObject<UserSession>(userSessionJson);

        // Use the user session to access the user information
        // Create and return a User object
        var user = new User
        {
            Id = userSession.Id,
            Name = userSession.Name,
            Email = userSession.Email,
        };

        return user;
    }
}