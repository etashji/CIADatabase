using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIADatabase.Areas.GWOT.Models
{
    public class GWOTProfile
    {
        [Key]  // Marks the UserId as the primary key
        public int ProfileId { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[] ProfilePic { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; } = new DateTime(1753, 1, 1);

        [MinLength(2)]  // Minimum length of 2 for First Name
        [MaxLength(25)]  // Maximum length of 25 for First Name
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name must only contain letters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MinLength(2)]  // Minimum length of 2 for Last Name
        [MaxLength(25)]  // Maximum length of 25 for Last Name
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name must only contain letters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MinLength(2)]  // Minimum length of 2 for Last Name
        [MaxLength(25)]  // Maximum length of 25 for Last Name
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Alias must only contain letters.")]
        public string Alias { get; set; }

        [AllowHtml]
        [Required]
        [Column(TypeName = "ntext")]
        [MinLength(10, ErrorMessage = "The profile must be at least 10 characters long.")]// Adjust column type if using SQL Server
        public string Profile { get; set; }

        public int ProfileSectionId { get; set; }
        public virtual GWOTProfileSections ProfileSection { get; set; }
    }
}