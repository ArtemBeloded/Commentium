namespace Commentium.Domain.Users
{
    public interface IUserRepository
    {
        Task Add(User user);

        Task<User?> GetByEmail(string email);

        Task<User?> GetById(Guid Id);
    }
}
