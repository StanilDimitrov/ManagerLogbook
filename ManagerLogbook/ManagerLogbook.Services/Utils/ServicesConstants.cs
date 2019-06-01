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
        public const string UserIsNotAuthorizedToEditNote = "User is not authorized to edit this note!";
        public const string UserIsNotAuthorizedToViewNotes = "User is not authorized to view notes from this logbook!";
        public const string DescriptionCanNotBeNull = "Description can not be null or empty string!";
        public const string EmailIsNotValid = "Email {0} is not valid!";
        public const string NoSuchUserExists = "No such user exists!";
        public const string NoteDoesNotExists = "Note does not exists!";
        public const string PhoneNumberIsNotValid = "PhoneNumber {0} is not valid!";
        public const string NoLogboogChoosen = "You must choose a logbook to do operations in it.";
        public const string ManagerIsAlreadyInLogbook = "Manager {0} is already exists in logbook {1}!";
        public const string ModeratorIsAlreadyInBusinessUnit = "Moderator {0} is already exists in businessUnit {1}!";
        public const string CategoryIsAlreadyInBusinessUnit = "Category {0} is already exists!";
        public const string NameCanNotBeNullOrEmpty = "Name can not be null or empty string!";
        public const string RatingNotInRange = "Value for raiting must be between 0 and 5.";
        public const string UserNotFromLogbook = "User is not manager of logbook.";
    }
}
