using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class AccountModel
    {
        public int AccountId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public AccountModel( int id, string first = null, string last = null)
        {

        }
    }
}
