using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Menu: BaseEntity
    {
        public string DisplayName { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string MenuURL { get; set; }
        public string MenuIcon { get; set; }
        public bool IsLeftCol { get; set; }
    }
}
