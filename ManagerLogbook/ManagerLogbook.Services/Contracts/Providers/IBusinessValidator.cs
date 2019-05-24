using ManagerLogbook.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Services.Contracts.Providers
{
    public interface IBusinessValidator
    {

        void IsNameInRange(string name);

        void IsDescriptionInRange(string description);

        void IsAddressInRange(string address);

    }
}
