using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class Bed : IComparable
    {
        public bool Availability { get; set; }
        public int Bedcode { get; set; }
        public int Patient { get; set; }

        public int CompareTo(object obj)
        {
            return this.Bedcode.CompareTo(((Bed)obj).Bedcode);
        }

        public void EmptyBed()
        {
            Patient = 0;
            Availability = true;
        }
    }
}