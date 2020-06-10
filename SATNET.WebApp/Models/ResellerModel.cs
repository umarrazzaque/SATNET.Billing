using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class ResellerModel
    {
        public ResellerModel()
        {
            RName = RType = REmail = RAddress = RContactNumber = "";
            RTypeId = ResellerId = -1;
        }
        public int ResellerId { get; set; }
        public string RName { get; set; }
        public int RTypeId { get; set; }
        public string RType { get; set; }
        public string REmail { get; set; }
        public string RAddress { get; set; }
        public string RContactNumber { get; set; }
    }

    public class CreateResellerModel
    {
        public CreateResellerModel()
        {
            ResellerModel = new ResellerModel();
            ResellerType = new List<LookUpModel>();
        }
        public ResellerModel ResellerModel { get; set; }
        public List<LookUpModel> ResellerType { get; set; }
    }
}
