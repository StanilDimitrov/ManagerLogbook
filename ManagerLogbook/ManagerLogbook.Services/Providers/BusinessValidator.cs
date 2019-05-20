using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Services.Providers
{
    public class BusinessValidator: IBusinessValidator
    {
        public void IsNull (string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException(ServicesConstants.InputCanNotBeNullOrEmpty);
            }
        }

        public void IsNameInRange(string name)
        {
            if (name.Length > 50)
            {
                throw new ArgumentException(ServicesConstants.NameNotInRange);
            }
        }

        public void IsDescriptionInRange(string description)
        {
            if (description.Length > 500)
            {
                throw new ArgumentException(ServicesConstants.DescriptionNotInRange);
            }
        }

        public void IsAddressInRange(string address)
        {
            if (address.Length > 200)
            {
                throw new ArgumentException(ServicesConstants.AddressNotInRange);
            }
        }
    }
}
