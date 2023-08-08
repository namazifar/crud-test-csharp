using MC2.CurdTest.Application.Interfaces;
using MC2.CurdTest.Domain.MC2Entities;
using MC2.CurdTest.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;


namespace MC2.CurdTest.Persistence
{
    public class MC2DbConetxt : DbContext, IMC2DbConetxt
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public MC2DbConetxt()
        {
        }
        public MC2DbConetxt(DbContextOptions<MC2DbConetxt> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }

    }
}
