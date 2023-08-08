using MC2.CurdTest.Domain.MC2Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC2.CurdTest.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> customerConfig)
        {
            customerConfig.ToTable("Customers");

            customerConfig.HasKey(o => o.Id);

            customerConfig.Property(b => b.UpdatedAt).IsRequired(false);
            customerConfig.Property(b => b.Email).HasMaxLength(50).IsRequired(true);
            customerConfig.Property(b => b.FirstName).HasMaxLength(50).IsRequired(true);
            customerConfig.Property(b => b.LastName).HasMaxLength(50).IsRequired(true);
            customerConfig.Property(b => b.BankAccountNumber).HasMaxLength(20);
            customerConfig.Property<string>(b => b.PhoneNumber).HasMaxLength(14);
            customerConfig.Property(b => b.DateOfBirth);
            customerConfig.HasIndex(k => new { k.FirstName, k.LastName, k.DateOfBirth }).IsUnique();
            customerConfig.HasIndex(k => k.Email).IsUnique();
            customerConfig.Property(x => x.IsDeleted).HasDefaultValue(false);
            customerConfig.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
