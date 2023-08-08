using MC2.CurdTest.Domain.MC2Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MC2.CurdTest.Application.Interfaces
{
    public interface IMC2DbConetxt
    {
        DbSet<Customer> Customers { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
