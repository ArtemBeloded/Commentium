namespace Commentium.Domain.Users
{
    public class User
    {
        public Guid Id { get; private set; }

        public string UserName { get; private set; }

        public string Email { get; private set; }
    }
}
