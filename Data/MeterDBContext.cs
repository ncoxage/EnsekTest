using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Data.Extensions;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <remarks>
        /// Need to remove literal "Configure", and assumed method signature - possibly using config/reflection combination.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // get list of all DbSet properties
            var setProperties = GetType().GetProperties().Where(p => p.PropertyType.IsGenericType
                                                                && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            setProperties.ForEach(set =>
            {
                // find Type of set 
                var setType = set.PropertyType.GenericTypeArguments[0];

                // get static Configure method
                var configure = setType.GetMethod("Configure", new Type[] { typeof(ModelBuilder) });

                if (configure != null)
                {
                    configure.Invoke(null, new object[] { modelBuilder });
                }
            });
        }
    }
}