using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Services.Contracts.Providers
{
    public interface IBusinessValidator
    {
        void IsNull(string input);

        void IsNameInRange(string name);

        void IsDescriptionInRange(string description);

        void IsAddressInRange(string address);

        void IsEmailValid(string email);

        void IsPhoneNumberValid(string phoneNumber);
    }
}
