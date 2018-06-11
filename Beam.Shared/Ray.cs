using System;
using System.Collections.Generic;
using System.Text;

namespace Beam.Shared
{
    public class Ray
    {
        public int RayId { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PrismCount { get; set; }
    }
}
