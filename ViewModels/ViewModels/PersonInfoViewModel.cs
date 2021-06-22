using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.ViewModels
{
   public class PersonInfoViewModel
    {
        public LifeInUserInfo userInfos { get; set; }

        public string PersonId { get; set; }
    }
    public class IntegalInfoViewModel
    {
        public string IntegalName { get; set; }

        public double Percent { get; set; }
    }
}
