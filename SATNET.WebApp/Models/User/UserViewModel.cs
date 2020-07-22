using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SATNET.WebApp.Models.User
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please enter first name")]

        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please enter last name")]
        public string LastName { get; set; }
        [DisplayName("Full Name")]
        public string FullName { get { return FirstName + " " + LastName; } }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "Please enter username")]
        public string UserName { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Please enter email address")]
        [EmailAddress]
        public string Email { get; set; }
        [DisplayName("Contact")]
        public string Contact { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password does not match")]
        public string ConfirmPassword { get; set; }
        public List<string> Roles { get; set; }
        public string RoleName { get; set; }
        [Required]
        public int UserTypeId { get; set; }
        public SelectList UserTypeSelectList { get; set; }
        public string UserTypeName { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public SelectList CustomerSelectList { get; set; }
        public string CustomerName { get; set; }
        [Required]
        public int PriceTierId { get; set; }
        public SelectList PriceTierSelectList { get; set; }
        public string PriceTierName { get; set; }

    }
}