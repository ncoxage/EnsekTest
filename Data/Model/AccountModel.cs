using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Model
{
    public class AccountModel : IEntityTypeConfiguration<AccountModel>
    {
        public int AccountId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public virtual List<ReadingModel> Readings { get; private set; }

        public AccountModel() { }

        public AccountModel(int accountId, string firstName = null, string lastName = null)
        {
            AccountId = accountId;
            FirstName = firstName;
            LastName = lastName;
        }

        public void Configure(EntityTypeBuilder<AccountModel> builder)
        {
            builder.HasKey(acc => acc.AccountId);
        }
    }
}
