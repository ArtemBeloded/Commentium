using Commentium.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Commentium.Persistence.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetById(Guid id) 
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
