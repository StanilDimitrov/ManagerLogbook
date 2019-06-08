using ManagerLogbook.Data.Models;
using ManagerLogbook.Services.Contracts.Providers;
using ManagerLogbook.Services.Utils;
using System;
using System.Text.RegularExpressions;

namespace ManagerLogbook.Services.Providers
{
    public class BusinessValidator : IBusinessValidator
    {

        public void DoesUserExists(User user)
        {
            if (user == null)
            {
                throw new ArgumentException(ServicesConstants.UserNotFound);
            }
        }

        public void IsNameInRange(string name)
        {
            if (name.Length > 50)
            {
                throw new ArgumentException(ServicesConstants.NameNotInRange);
            }
        }

        public void IsDescriptionIsNullOrEmpty(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException(ServicesConstants.DescriptionCanNotBeNull);
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

        public void IsEmailValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                throw new ArgumentException(ServicesConstants.EmailIsNotValid, email);
            }
        }

        public void IsPhoneNumberValid(string phoneNumber)
        {
            var checkPhoneNumber = Regex.Match(phoneNumber, @"^([0-9]{10})$").Success;

            if (!checkPhoneNumber)
            {
                throw new ArgumentException(ServicesConstants.PhoneNumberIsNotValid, phoneNumber);
            }
        }

        public void IsRatingInRange(int raiting)
        {
            if (raiting > 0 && raiting <= 5)
            {
                throw new ArgumentException(ServicesConstants.RatingNotInRange);
            }
        }

        public void IsDateValid(DateTime date)
        {
            DateTime checkDate;

            if (DateTime.TryParse(Convert.ToString(date), out checkDate))
            {

            }
            else
            {
                throw new ArgumentException(ServicesConstants.DateIsNotValid);
            }
        }
    }
}
