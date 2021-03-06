using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Data.Model;

[assembly: InternalsVisibleToAttribute("Data.Tests")]
namespace Data
{
    public class MeterReading : IMeterReading
    {
        public string AccountId { get; set; }
        public string ReadAt { get; set; }
        public string ReadValue { get; set; }

        public bool IsValid(IMeterDBContext context)
        {
            return ReadAtIsValid()
                     && ReadValueIsValid()
                     && AccountIdIsValid(context);
        }

        internal bool AccountIdIsValid(IMeterDBContext context)
        {
            var isValid = false;

            try
            {

                if (Int32.TryParse(AccountId, NumberStyles.Number, null, out int accountId)
                    && accountId > 0)
                {
                    isValid = context.Accounts.FirstOrDefault(acc => acc.AccountId == accountId) != null;
                }
            }
            catch
            {
                // should add debug logging
            }

            return isValid;
        }

        internal bool ReadValueIsValid()
        {
            bool isValid = false;

            try
            {
                isValid = ReadValue.Length == 5
                            && ReadValue.All(char.IsDigit);
            }
            catch
            {
                // should add debug logging
            }

            return isValid;

        }

        internal bool ReadAtIsValid()
        {
            bool isValid = false;

            try
            {
                DateTime.ParseExact(ReadAt, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                // should add debug logging
            }

            return isValid;
        }

        public async Task<bool> LoadReading(IMeterDBContext context)
        {
            try
            {
                if (IsValid(context))
                {
                    await context.Readings.AddAsync(new ReadingModel(accountId: Convert.ToInt32(AccountId),
                                                                     readAt: Convert.ToDateTime(ReadAt),
                                                                     value: Convert.ToInt32(ReadValue)));

                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch
            {
                // add debug logging
            }

            return false;
        }
    }
}
