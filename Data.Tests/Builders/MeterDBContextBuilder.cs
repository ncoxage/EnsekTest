using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Data;
using System.Data.Common;

namespace Data.Tests.Builders
{
    internal class MeterDBContextBuilder : IDisposable
    {
        private bool disposedValue;

        private MeterDBContext _context { get; set; }

        internal MeterDBContextBuilder()
        {
            using (var context = new MeterDBContext(new DbContextOptionsBuilder<MeterDBContext>().UseSqlite(CreateInMemoryDatabase()).Options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

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
