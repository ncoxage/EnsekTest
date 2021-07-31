using Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public interface IMeterDBContext
    {
        DbSet<AccountModel> Accounts { get; }
        DbSet<ReadingModel> Readings { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
