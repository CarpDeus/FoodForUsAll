using System;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName {
            get
            {
                return (FirstName + ((string.IsNullOrEmpty(LastName)) ? string.Empty : " " + LastName)).Trim();
            }
        }
    }
}
