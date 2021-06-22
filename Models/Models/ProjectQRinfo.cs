using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
   public class ProjectQRinfo
    {
        public int ID { get; set; }
        
        public string ProjectName { get; set; }


        public string PersonName { get; set; }

        public string QrCode { get; set; }

        public string PersonId { get; set; }

        public string ProjectInfoId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
