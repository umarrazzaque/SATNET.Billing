using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SATNET.WebApp.Models
{
    public class DistributorUserViewModel
    {
        public DistributorUserViewModel()
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
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DistributorId { get; set; }
        public SelectList customList { get; set; }
    }
}
