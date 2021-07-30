using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Data.Model;

[assembly: InternalsVisibleToAttribute("Data.Tests")]
namespace Data
{
    public class ReadingsLoadResults
    {
        public List<ReadingModel> LoadedReadings { get; } = new List<ReadingModel>();
        public List<ReadingModel> RejectedReadings { get; } = new List<ReadingModel>();

        public int Loaded { get => LoadedReadings.Count; }

        public int Rejected { get => RejectedReadings.Count; }
    }

    public class MeterReading
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

        public bool LoadReading(IMeterDBContext context)
        {
            try
            {
                if (IsValid(context))
                {
                    context.Readings.Add(new ReadingModel(accountId: Convert.ToInt32(AccountId),
                                                          readAt: Convert.ToDateTime(ReadAt),
                                                          value: Convert.ToInt32(ReadValue)));

                    context.SaveChanges();

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
