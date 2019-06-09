using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Services.Contracts.Providers
{
    public interface IReviewEditor
    {
        string AutomaticReviewEditor(string originalDescription);

        bool CheckReviewVisibility(string editDescription);
    }
}
