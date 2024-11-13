using Commentium.Domain.Shared;

namespace Commentium.Domain.Errors
{
    public static class DomainErrors
    {
        public static class UserErrors 
        {
            public static Error InvalidUserName => Error.Validation(
                "User.InvalidUserName",
                "The provided user name is invalid");
        }

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

        public static class UserNameErrors
        {
            public static Error Empty => Error.Validation(
                "UserName.Empty",
                "User name is empty");

            public static Error TooLong => Error.Validation(
                "UserName.TooLong",
                "User name is too long");

            public static Error InvalidFormat => Error.Validation(
                "UserName.InvalidFormat",
                "User name format is invalid");
        }

        public static class CommentContentErrors 
        {
            public static Error Empty => Error.Validation(
                "Comment.Empty",
                "Comment text is empty");

            public static Error InvalidHTMLTags => Error.Validation(
                "Comment.InvalidFormat",
                "Comment contains invalid HTML tags");

            public static Error UnclosedHTMLTags => Error.Validation(
                "Commnet.InvalidFormat",
                "Comment contains unclosed or incorrectly nested tags");
        }

        public static class CommentFileErrors 
        {
            public static Error InvalidFileFormat => Error.Validation(
                "CommentFile.InvalidFileFormat",
                "Acceptable file formats: JPG, GIF, PNG and TXT");

            public static Error InvalidTextFileSize => Error.Validation(
                "CommentFile.InvalidTextFileSize",
                "Text file size exceeds 100 kB");
        }
    }
}
