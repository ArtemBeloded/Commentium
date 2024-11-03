using Commentium.Domain.Shared;

namespace Commentium.Domain.Users
{
    public class User
    {
        private const int MaxUserNameLength = 50;
        private const int MaxEmailLenght = 60;

        private User(
            Guid id,
            string userName,
            string email)
        {
            Id = id;
            UserName = userName;
            Email = email;
        }

        public Guid Id { get; private set; }

        public string UserName { get; private set; }

        public string Email { get; private set; }

        public static Result<User> Create(string userName, string email) 
        {
            //email verification
            //userName verification

            var user = new User(
                Guid.NewGuid(),
                email,
                userName);

            return user;
        }
    }
}
