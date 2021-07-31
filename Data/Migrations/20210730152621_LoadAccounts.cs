using System.Globalization;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

using CsvHelper;

using Data.Model;
using System;

namespace Data.Migrations
{
    public partial class LoadAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var dbContext = new MeterDBContext(new DbContextOptionsBuilder<MeterDBContext>().UseSqlite("Data Source=../MeterReadings.db;").Options);

            using (var reader = new CsvReader(new StreamReader("..\\SoftwareEngineerExercise\\Test_Accounts.csv"), CultureInfo.InvariantCulture))
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
