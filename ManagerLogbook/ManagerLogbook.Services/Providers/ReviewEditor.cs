using ManagerLogbook.Services.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Services.Providers
{
    public class ReviewEditor : IReviewEditor
    {
        public string AutomaticReviewEditor(string originalDescription)
        {
            var editDescription = originalDescription.Replace("shit", "****");

            return editDescription;
        }
    }
}
