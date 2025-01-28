using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIADatabase.Areas.JediArchives.Models
{
    public class JediProfile
    {
        [Key]  // Marks the ProfileId as the primary key
        public int ProfileId { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[] ProfilePic { get; set; }

        [MinLength(2)]  // Minimum length of 2 for First Name
        [MaxLength(25)]  // Maximum length of 25 for First Name
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MinLength(2)]  // Minimum length of 2 for Last Name
        [MaxLength(25)]  // Maximum length of 25 for Last Name
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MinLength(2)]  // Minimum length of 2 for Alias
        [MaxLength(25)]  // Maximum length of 25 for Alias
        public string Alias { get; set; }

        [AllowHtml]
        [Required]
        [Column(TypeName = "ntext")]
        [MinLength(10, ErrorMessage = "The profile must be at least 10 characters long.")] // Adjust column type if using SQL Server
        public string Profile { get; set; }

        public int ProfileSectionId { get; set; }
        public virtual JediProfileSection ProfileSection { get; set; }
    }
}
