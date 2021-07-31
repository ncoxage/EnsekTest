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
        /// Need to remove literals, and assumed method signatures - possibly using config/reflection combination.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var loaded = new List<string>();
            var entityMethod = typeof(ModelBuilder).GetMethods().Where(m => m.Name == "Entity" && m.IsGenericMethod).First();

            var setProperties = GetType().GetProperties().Where(p => p.PropertyType.IsGenericType
                                                                && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            setProperties.ForEach(set =>
            {
                var setType = set.PropertyType.GenericTypeArguments[0];

                var entity = entityMethod.MakeGenericMethod(new Type[] { setType });
                entity.Invoke(modelBuilder, Array.Empty<object>());

                var configure = setType.GetMethod("Configure", new Type[] { typeof(ModelBuilder) });

                if (configure != null)
                {
                    configure.Invoke(null, new object[] { modelBuilder });
                }
            });
        }
    }
}