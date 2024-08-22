using Microsoft.EntityFrameworkCore;
using ProgrammingTest.Models;

namespace ProgrammingTest
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }
    }
}
