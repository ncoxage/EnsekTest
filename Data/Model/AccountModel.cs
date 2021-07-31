using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Data.Model
{
    public class AccountModel
    {
        public int AccountId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public virtual List<ReadingModel> Readings { get; private set; }

        public AccountModel( int accountId, string firstName = null, string lastName = null)
        {
            AccountId = accountId;
            FirstName = firstName;
            LastName = lastName;
        }

        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<AccountModel>(
                e =>
                {
                    e.HasKey(acc => acc.AccountId);
                });
        }
    }
}
