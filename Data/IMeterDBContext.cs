using Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    interface IMeterDBContext
    {
        DbSet<AccountModel> Accounts { get; }
        DbSet<ReadingModel> Readings { get; }

        int SaveChanges();
    }
}
