using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class ReadingModel
    {
        public int Id { get; private set; }
        public int AccountId { get; private set; }
        public DateTime ReadAt { get; private set; }
        public int Value { get; private set; }

        public virtual AccountModel Account { get; private set; }

        public ReadingModel( int accountId, DateTime readAt, int value)
        {
            AccountId = accountId;
            ReadAt = readAt;
            Value = value;
        }

        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<ReadingModel>(
                e =>
                {
                    e.HasOne(read => read.Account)
                          .WithMany(acc => acc.Readings);

                    e.HasIndex(e => new { e.AccountId, e.ReadAt })
                     .IsUnique();
                }
            );
        }
    }
}
