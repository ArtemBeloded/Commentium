using Commentium.Domain.Shared;

namespace Commentium.Domain.Errors
{
    public static class DomainErrors
    {
        public static class EmailErrors 
        {
            public static Error Empty => Error.Validation(
                "Email.Empty",
                "Email is empty");

            public static Error TooLong => Error.Validation(
                "Email.TooLong",
                "Email is too long");

            public static Error InvalidFormat => Error.Validation(
                "Email.InvalidFormat",
                "Email format is invalid");
        }

        public static class UserNameErorrs 
        {
            public static Error Empty => Error.Validation(
                "FirstName.Empty",
                "FirstName is empty");

            public static Error TooLong => Error.Validation(
                "FirstName.TooLong",
                "FirstName is too long");
        }
    }
}
