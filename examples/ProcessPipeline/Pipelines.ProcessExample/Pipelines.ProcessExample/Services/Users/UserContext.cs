namespace Pipelines.ProcessExample.Services.Users;

public class UserContext : IUserContext
{
    public Guid GetUser()
    {
        return Guid.NewGuid();
    }
}