using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.ModelBinding;

namespace CIADatabase.Areas.Users.Models
{
    public class User
    {
        [Key]  // Marks the UserId as the primary key
        public int UserId { get; set; }

        [Required (ErrorMessage = "First name is required.")]
        [MinLength(2)]  // Minimum length of 2 for First Name
        [MaxLength(25)]  // Maximum length of 25 for First Name
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name must only contain letters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required (ErrorMessage = "Last name is required.")]
        [MinLength(2)]  // Minimum length of 2 for Last Name
        [MaxLength (25)]  // Maximum length of 25 for Last Name
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name must only contain letters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required (ErrorMessage = "Age is required.")]
        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100.")]
        
        public int Age { get; set; }

        [Required (ErrorMessage = "Email is required.")]  // Makes the Email required
        [EmailAddress]  // Ensures the Email is in a valid format
        [Index(IsUnique = true)]  // Enforces uniqueness
        [MaxLength(256)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]  // Makes the Username required
        [StringLength(25, MinimumLength = 8)]  // Username length between 8 and 25 characters
        [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*(),.?:{}|<>-]+$",
            ErrorMessage = "Username can only include uppercase letters, lowercase letters, numbers, and special characters.")]
        [Index(IsUnique = true)]  // Enforces uniqueness
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]  // Makes the Password required
        [StringLength(25, MinimumLength = 8)]  // Password length between 8 and 25 characters
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?:{}|<> -])[a-zA-Z0-9!@#$%^&*(),.?:{}|<>-]{8,25}$",
            ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one number, and one special character, and may only contain uppercase letters, lowercase letters, numbers, and special characters.")]
        [NotMapped]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must confirm your password.")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [NotMapped]  // This will not be mapped to a column in the database
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string HashedPassword { get; set; }
    }

}