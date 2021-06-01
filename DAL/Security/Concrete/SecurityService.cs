using System;
using System.Threading.Tasks;
using DAL.Security.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DAL.Security.Concrete
{
    public class SecurityService : ISecurityService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public SecurityService(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<User> GetUser(int userId)
        {
            using (var ctx = _factory.CreateDbContext())
            {
                var user = await ctx.Users.AsNoTracking().SingleOrDefaultAsync(o => o.UserId == userId);
                if (user == null)
                    throw new Exception($"User with id {userId} not found");
                return user;
            }
        }

        public async Task<User[]> GetUsers()
        {
            using (var ctx = _factory.CreateDbContext())
            {
                var users = await ctx.Users.AsNoTracking().ToArrayAsync();
                return users;
            }
        }

        public async Task<bool> CreateUser(User user)
        {
            using (var ctx = _factory.CreateDbContext())
            {
                await ctx.Users.AddAsync(new User
                {
                    Name = user.Name,
                    Address = user.Address,
                    Contact = user.Contact
                });
                return await ctx.SaveChangesAsync() > 0;
            }
        }
        public async Task<bool> EditUser(User user)
        {
            using (var ctx = _factory.CreateDbContext())
            {
                var userToEdit = await ctx.Users.SingleOrDefaultAsync(o => o.UserId == user.UserId);
                if (userToEdit == null)
                    throw new Exception($"User {user.Name} ({user.UserId}) not found");

                userToEdit.Address = user.Address;
                userToEdit.Contact = user.Contact;
                userToEdit.Name = user.Name;
                userToEdit.UserId = user.UserId;

                return await ctx.SaveChangesAsync() > 0;
            }
        }
        public async Task<bool> DeleteUser(int userId)
        {
            using (var ctx = _factory.CreateDbContext())
            {
                var user = await ctx.Users.SingleOrDefaultAsync(o => o.UserId == userId);
                if (user == null)
                    throw new Exception($"User with id {userId} not found");
                ctx.Remove(user);
                return await ctx.SaveChangesAsync() > 0;
            }
        }
    }
}