using System;
using System.Data;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Data.Model;


namespace Data
{
    public class MeterDBContext : DbContext, IMeterDBContext
    {
        public DbSet<AccountModel> Accounts { get => this.Set<AccountModel>(); }
        public DbSet<ReadingModel> Readings { get => this.Set<ReadingModel>(); }

        public MeterDBContext(DbContextOptions dbOptions) : base(dbOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // get list of all DbSet properties
            foreach (var setProperty in GetType().GetProperties()
                                                 .Where(p => p.PropertyType.IsGenericType
                                                                && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)))

            {
                // find Type of set 
                var setType = setProperty.PropertyType.GenericTypeArguments[0];

                var entityMethod = typeof(ModelBuilder).GetMethods()
                                                       .Where(m => m.Name == "ApplyConfiguration"  //ideally this should be read from config
                                                                && m.IsGenericMethod)
                                                       .First();

                var entity = entityMethod.MakeGenericMethod(new Type[] { setType });
                entity.Invoke(modelBuilder, new object[] { Activator.CreateInstance(setType) });
            };
        }
    }
}