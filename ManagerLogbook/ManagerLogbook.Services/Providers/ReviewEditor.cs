using Censored;
using ManagerLogbook.Data;
using ManagerLogbook.Services.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ManagerLogbook.Services.Providers
{
    public class ReviewEditor : IReviewEditor
    {
        private readonly ManagerLogbookContext context;

        public ReviewEditor(ManagerLogbookContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        public string AutomaticReviewEditor(string originalDescription)
        {
            var censoredWords = this.context.CensoredWords
                         .Select(x => x.Word)
                         .ToList();

            var censor = new Censor(censoredWords);

            var editDescription = censor.CensorText(originalDescription);

            return editDescription;
        }

        public bool CheckReviewVisibility(string editDescription)
        {
            bool isVisible = true; 

            int countReplaceChars = editDescription.Replace(" ",string.Empty).TakeWhile(c => c == '*').Count();

            double isVisibleRatio = countReplaceChars * 1.00 / editDescription.Length; 

            if(isVisibleRatio>0.25)
            {
                isVisible = false;
            }

            return isVisible;
        }
    }
}
