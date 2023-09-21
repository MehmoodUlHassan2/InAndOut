using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InAndOut.Data
{
    public class ApplicationDbContext :IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
                                                : base(options) 
        { 
            
        }
        public DbSet<Item> Items { get; set; }

        public DbSet<Expense> Expenses { get; set; }

    }
}
