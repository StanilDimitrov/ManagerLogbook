using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Services.Utils
{
    public static class ServicesConstants
    {
        public const string InputCanNotBeNullOrEmpty = "Input can not be null or empty string!";
        public const string NameNotInRange = "Name can not be more than 50 symbols long!";
        public const string DescriptionNotInRange = "Description can not be more than 500 symbols long!";
        public const string AddressNotInRange = "Address can not be more than 200 symbols long!";
        public const string EmailIsNotValid = "Email {0} is not valid!";
        public const string PhoneNumberIsNotValid = "PhoneNumber {0} is not valid!";        
    }
}
