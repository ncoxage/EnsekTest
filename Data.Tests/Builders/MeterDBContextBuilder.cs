using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Data;
using Data.Model;

namespace Data.Tests.Builders
{
    internal class MeterDBContextBuilder : IDisposable
    {
        private bool disposedValue;

        private DbContextOptions<MeterDBContext> _contextOptions { get; }
        private MeterDBContext _context { get; set; }

        private List<AccountModel> _accountsSeed { get; set; }

        private List<ReadingModel> _readingsSeed { get; set; }

        internal MeterDBContextBuilder()
        {
            _contextOptions = new DbContextOptionsBuilder<MeterDBContext>()
                                   .UseSqlite(CreateInMemoryDatabase()).Options;

            using (var context = new MeterDBContext(dbOptions: _contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        internal MeterDBContext Build()
        {
            _context = new MeterDBContext(dbOptions: _contextOptions);

            if (_accountsSeed != null)
            {
                foreach (var account in _accountsSeed)
                {
                    _context.Add(account);
                }

            }

            if (_readingsSeed != null)
            {
                foreach (var reading in _readingsSeed)
                {
                    _context.Add(reading);
                }

            }

            _context.SaveChanges();
            return _context;
        }

        internal MeterDBContextBuilder AccountSeeds(IEnumerable<AccountModel> seedAccounts)
        {
            _accountsSeed = new List<AccountModel>(seedAccounts);
            return this;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MeterDBContextBuilder()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }


    }
}
