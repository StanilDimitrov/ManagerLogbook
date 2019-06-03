using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLogbook.Services.CustomExeptions
{
    public class NotAuthorizedException: Exception
    {
        public NotAuthorizedException()
        {
        }

        public NotAuthorizedException(string message)
            : base(message)
        {
        }

        public NotAuthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
