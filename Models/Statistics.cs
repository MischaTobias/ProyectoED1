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
        public Nullable<int> Suspicious { get; set; }
        [Display(Name = "Porcentaje de sospechosos positivos")]
        public double PositivePercentage { get; set; }
        [Display(Name = "Cantidad de Egresados")]
        public int Recovered { get; set; }
    }
}