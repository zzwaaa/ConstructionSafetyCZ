using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
   public partial class Imagelist
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string ImageName { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
