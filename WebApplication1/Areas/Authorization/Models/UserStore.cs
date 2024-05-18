using WebApplication1.Models;

namespace WebApplication1.Areas.Authorization.Models;
public class UserStore
{
    private readonly List<User> users = new();

    public void AddUser(User user)
    {
        if (!users.Any(u => u.DiscordId == user.DiscordId))
        {
            users.Add(user);
        }
    }

    public User GetUser(string discordId)
    {
        return users.FirstOrDefault(u => u.DiscordId == discordId);
    }

    public List<User> GetAllUsers()
    {
        return users;
    }
}