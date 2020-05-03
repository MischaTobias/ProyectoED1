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
        public string Patient { get; set; }

        public void EmptyBed()
        {
            Patient = null;
            Availability = true;
        }
    }
}