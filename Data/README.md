# Data

DAL/BL layer

## Database Creation

Ensure the correct locations are specified in the [migration settings](#migrationsettings) file, then, from this (Data) folder, run:

`dotnet ef database update` (.NET Core CLI)

or

`Update-Database` (Visual Studio)

This will result in the creation of the database and loading of the Accounts contained in the [Test Accounts](#accountsdata) file.

### <a name="migrationsettings" /> [Migration Settings](migrationsettings.json)

#### <a name="meterreadingsdb" />MeterReadingsDb

File path of the MeterReadings ([Sqlite](https://www.sqlite.org/index.html)) database file. This should reference the same file specified in the [Api settings](../Data/README.md#meterreadingsdb).

#### <a name="aacountsdata" />AccountsDataFile

File path of the Accounts sample data.

## References

- [CSV Helper](https://joshclose.github.io/CsvHelper/)