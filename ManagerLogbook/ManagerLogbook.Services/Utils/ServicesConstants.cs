namespace ManagerLogbook.Services.Utils
{
    public static class ServicesConstants
    {
        public const string InputCanNotBeNullOrEmpty = "Input can not be null or empty string!";
        public const string NameNotInRange = "Name can not be more than 50 symbols long!";
        public const string DescriptionNotInRange = "Description can not be more than 500 symbols long!";
        public const string AddressNotInRange = "Address can not be more than 200 symbols long!";
        public const string UserNotFound = "User does not exist!";
        public const string UserNotManagerOfLogbook = "User \"{0}\" is not manager of logbook \"{1}\".";
        public const string UserIsNotAuthorizedToEditNote = "User \"{0}\" is not authorized to edit this note!";
        public const string UserIsNotAuthorizedToViewNotes = "User \"{0}\" is not authorized to view notes from this logbook!";
        public const string DescriptionCanNotBeNull = "Description can not be null or empty string!";
        public const string EmailIsNotValid = "Email {0} is not valid!";
        public const string NoSuchUserExists = "No such user exists!";
        public const string NotNotFound = "Note does not exists!";
        public const string PhoneNumberIsNotValid = "PhoneNumber {0} is not valid!";
        public const string DateIsNotValid = "Date is not valid!"; 
        public const string NoLogboogChoosen = "You must choose a logbook to do operations in it.";
        public const string ManagerIsAlreadyInLogbook = "Manager {0} is already exists in logbook {1}!";
        public const string ModeratorIsAlreadyInBusinessUnit = "Moderator {0} is already exists in businessUnit {1}!";
        public const string CategoryIsAlreadyInBusinessUnit = "Category {0} is already exists!";
        public const string NameCanNotBeNullOrEmpty = "Name can not be null or empty string!";
        public const string RatingNotInRange = "Value for raiting must be between 0 and 5.";
        public const string UserNotFromLogbook = "User is not manager of logbook.";
        public const string NoteCategoryDoesNotExists = "Note category does not exists.";
        public const string LogbookNotFound = "Logbook does not exists.";
        public const string LogbookAlreadyExists = "Logbook name already exists.";
        public const string BusinessUnitNotFound = "BusinessUnit does not exists.";
        public const string BusinessUnitCategoryNotFound = "BusinessUnit Category does not exists.";
        public const string BusinessUnitNameAlreadyExists = "BusinessUnit name already exists.";
        public const string BusinessUnitCategoryNameAlreadyExists = "BusinessUnitCategory name already exists.";
        public const string ReviewNotFound = "Review does not exists.";
    }
}
