using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Lookup
{
    public class LookUpModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LookUpTypeId { get; set; }
        public string LookupTypeName { get; set; }
    }

    public class CreateLookUpModel
    {
        public CreateLookUpModel()
        {
            LookUpModel = new LookUpModel();
            LookupType = new List<LookUpTypeModel>();
        }
        public LookUpModel LookUpModel { get; set; }
        public List<LookUpTypeModel> LookupType { get; set; }
    }
}
