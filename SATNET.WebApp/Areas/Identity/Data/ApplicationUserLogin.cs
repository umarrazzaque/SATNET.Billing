﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Areas.Identity.Data
{
    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}