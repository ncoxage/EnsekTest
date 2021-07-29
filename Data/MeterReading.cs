using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleToAttribute("Data.Tests")]
namespace Data
{
    public class MeterReading
    {
        public string AccountId { get; set; }
        public string ReadAt { get; set; }
        public string ReadValue { get; set; }

        internal bool AccountIdIsValid(IMeterDBContext context)
        {
            var isValid = false;

            try
            {
                int accountId = 0;

                if(Int32.TryParse(AccountId, NumberStyles.Number, null, out accountId) 
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
                int value = 0;

                isValid = ReadValue.Length == 5 
                            && ReadValue.All(char.IsDigit);
            }
            catch
            {
                // should add debug logging
            }

            return isValid;

        }
    }
}
