using BudgetJar.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetJar.Api.Data
{
    public class BudgetContext : DbContext
    {
        public BudgetContext(DbContextOptions<BudgetContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}
