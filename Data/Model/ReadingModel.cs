using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Model
{
    public class ReadingModel : IEntityTypeConfiguration<ReadingModel>
    {
        public int Id { get; private set; }
        public int AccountId { get; private set; }
        public DateTime ReadAt { get; private set; }
        public int Value { get; private set; }

        public virtual AccountModel Account { get; private set; }

        public ReadingModel() { }

        public ReadingModel(int accountId, DateTime readAt, int value)
        {
            AccountId = accountId;
            ReadAt = readAt;
            Value = value;
        }

        public void Configure(EntityTypeBuilder<ReadingModel> builder)
        {
            builder.HasOne(read => read.Account)
                   .WithMany(acc => acc.Readings);

            builder.HasIndex(e => new { e.AccountId, e.ReadAt })
                   .IsUnique();
        }
    }
}
