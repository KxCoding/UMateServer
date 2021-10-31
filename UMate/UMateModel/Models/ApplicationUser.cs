using System;
using Microsoft.AspNetCore.Identity;

namespace UMateModel.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string NickName { get; set; }

        public string YearOfAdmission { get; set; }
        public string UniversityName { get; set; }
        public bool UniversityConfirmed { get; set; }

        public string SelectedProfileImage { get; set; }

        public DateTime JoinDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
