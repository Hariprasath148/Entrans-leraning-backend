using learning_api.Models;
using Microsoft.EntityFrameworkCore;

namespace learning_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
    } 
}
