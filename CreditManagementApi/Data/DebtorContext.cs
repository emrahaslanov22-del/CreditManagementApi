using CreditManagementApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditManagementApi.Context
{
    public class DebtorContext : DbContext
    {
        public DbSet<Debtor> debtors { set; get; }

        public DebtorContext(DbContextOptions<DebtorContext> options)
            : base(options) { }

    }
}
