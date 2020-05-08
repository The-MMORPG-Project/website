using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EF
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=users.db");
    }

    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
    }
}