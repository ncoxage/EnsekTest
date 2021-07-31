using System;
using System.Globalization;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

using CsvHelper;

using Data.Model;

using static Data.Globals;

namespace Data.Migrations
{
    public partial class LoadAccounts : Migration
    {
        readonly static string ACCOUNTS_FILE_KEY = "AccountsDataFile";
        readonly static string ACCOUNTS_FILE_DEFAULT = "Test_Accounts.csv";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var dbContext = new MeterDBContext(new DbContextOptionsBuilder<MeterDBContext>().UseSqlite($"Data Source={MeterDBContextFactory.Configuration.GetValue(DB_FILE_KEY, DB_FILE_DEFAULT)};").Options);

            using (var reader = new CsvReader(new StreamReader(MeterDBContextFactory.Configuration.GetValue(ACCOUNTS_FILE_KEY, ACCOUNTS_FILE_DEFAULT)), CultureInfo.InvariantCulture))
            {
                reader.Context.RegisterClassMap<ReadingMap>();
                var list = reader.GetRecords<dynamic>().GetEnumerator();

                while (list.MoveNext())
                {
                    var account = list.Current;
                    dbContext.Accounts.Add(new AccountModel(accountId: Convert.ToInt32(account.AccountId), firstName: account.FirstName, lastName: account.LastName ));
                }

                dbContext.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
