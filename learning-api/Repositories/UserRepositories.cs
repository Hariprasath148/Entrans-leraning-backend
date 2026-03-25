using learning_api.Data;
using learning_api.Dto;
using learning_api.Mappers;
using learning_api.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace learning_api.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        public readonly AppDbContext _context;
        public UserRepositories(AppDbContext context) { _context = context; }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmail(string Email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
        }

        public async Task<User> GetUserById(int Id)
        {
            return await _context.Users.FindAsync(Id);
        }

        public async Task AddUser(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUser(string Email) 
        {
            return await _context.Users.Where(u => u.Email != Email).ToListAsync();
        }

        public async Task<List<User>> GetUserByPage(string Email,int PageNumber,int PageSize = 10)
        {
            return await _context.Users.Where(u => u.Email != Email)
                                        .AsNoTracking()
                                        .Skip((PageNumber - 1) * PageSize)
                                        .Take(PageSize)
                                        .ToListAsync();
        }

        public async Task<int> GetCount(string Email)
        {
            return await _context.Users.Where(u => u.Email != Email)
                                     .AsNoTracking()
                                     .CountAsync();
        }

        public async Task RemoveUser(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<int> GetSearchCount(string Text, string Email)
        {
             return await _context.Users.Where(user =>
               user.Email != Email && (
               user.Id.ToString().Contains(Text) ||
               user.Name.ToLower().Contains(Text) ||
               user.Email.ToLower().Contains(Text) ||
               user.PhoneNumber.ToLower().Contains(Text))
             ).CountAsync();
        }

        public async Task<List<User>> GetUserBySearch(string Text, string Email,int PageNumber,int PageSize=10)
        {
            return await _context.Users
               .Where(user => user.Email != Email && (
               user.Id.ToString().Contains(Text) ||
               user.Name.ToLower().Contains(Text) ||
               user.Email.ToLower().Contains(Text) ||
               user.PhoneNumber.ToLower().Contains(Text))
            ).Skip((PageNumber - 1) * PageSize)
             .Take(PageSize)
             .ToListAsync();
        }

    }
}
