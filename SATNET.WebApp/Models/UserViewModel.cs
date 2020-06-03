using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SATNET.WebApp.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            FirstName = "";
            customList = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem {Text = "Google", Value = "Go"},
                new SelectListItem {Text = "Other", Value = "Ot"},
            }, "Value", "Text");
        }
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
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password does not match")]
        public string ConfirmPassword { get; set; }
        public SelectList customList { get; set; }
    }
}