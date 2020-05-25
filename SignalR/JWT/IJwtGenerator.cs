namespace SignalR.JWT
{
    public interface IJwtGenerator
    {
        string CreateToken(UserModel user);
    }
}