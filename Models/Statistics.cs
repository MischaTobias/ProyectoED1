using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class Statistics
    {
        [Display(Name = "Contagiados")]
        public int Infected { get; set; }
        [Display(Name = "Sospechosos")]
        public int Suspicious { get; set; }
        [Display(Name = "Sospechosos positivos")]
        public double PositivePercentage { get; set; }
        [Display(Name = "Cantidad de Egresados")]
        public int Recovered { get; set; }

        public Statistics()
        {
            Infected = 0;
            Suspicious = 0;
            PositivePercentage = 0;
            Recovered = 0;
        }
    }
}