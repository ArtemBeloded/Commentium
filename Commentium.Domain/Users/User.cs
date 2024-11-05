using Commentium.Domain.Shared;
using System.Text.RegularExpressions;
using static Commentium.Domain.Errors.DomainErrors;

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
            var isEmailCorrectResult = IsEmailCorrect(email);

            if (isEmailCorrectResult.IsFailure) 
            {
                return Result.Failure<User>(isEmailCorrectResult.Error);
            }

            var isUserNameCorrectResult = IsUserNameCorrect(userName);

            if (isUserNameCorrectResult.IsFailure) 
            {
                return Result.Failure<User>(isUserNameCorrectResult.Error);
            }

            var user = new User(
                Guid.NewGuid(),
                userName,
                email);

            return user;
        }

        private static Result<bool> IsEmailCorrect(string email) 
        {
            if (string.IsNullOrWhiteSpace(email)) 
            {
                return Result.Failure<bool>(EmailErrors.Empty);
            }

            if (email.Length > MaxEmailLenght) 
            {
                return Result.Failure<bool>(EmailErrors.TooLong);
            }

            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var isValid = Regex.IsMatch(email, emailPattern);

            return isValid ?
                true
                : Result.Failure<bool>(EmailErrors.InvalidFormat);
        }

        private static Result<bool> IsUserNameCorrect(string userName) 
        {
            if (string.IsNullOrWhiteSpace(userName)) 
            {
                return Result.Failure<bool>(UserNameErrors.Empty);
            }

            if (userName.Length > MaxUserNameLength) 
            {
                return Result.Failure<bool>(UserNameErrors.TooLong);
            }

            var userNamePattern = @"^[a-zA-Z0-9]+$";
            var isValid = Regex.IsMatch(userName, userNamePattern);

            return isValid ?
                true
                : Result.Failure<bool>(UserNameErrors.InvalidFormat);
        }
    }
}
