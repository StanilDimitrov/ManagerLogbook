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
        public const string UserDoesNotExist = "User does not exist!";
        public const string UserNotManagerOfLogbook = "User \"{0} {1}\" is not manager of logbook \"{2}\".";
        public const string UserIsNotAuthorizedToEditTask = "User is not authorized to edit this task!";
        public const string DescriptionCanNotBeNull = "Description can not be null or empty string!";
    }
}
