using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class Bed
    {
        public bool Availability { get; set; }
        public int Bedcode { get; set; }
        public int Patient { get; set; }
    }
}