using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class Statistics
    {
        /// <summary>
        /// Variable Declaration
        /// </summary>
        [Display(Name = "Contagiados")]
        public int Infected { get; set; }
        [Display(Name = "Sospechosos")]
        public int Suspicious { get; set; }
        [Display(Name = "Porcentage de Sospechosos Positivos")]
        public double PositivePercentage { get; set; }
        [Display(Name = "Cantidad de Egresados")]
        public int Recovered { get; set; }

        /// <summary>
        /// Constructor, stablishes the values as 0
        /// </summary>
        public Statistics()
        {
            Infected = 0;
            Suspicious = 0;
            PositivePercentage = 0;
            Recovered = 0;
        }

        /// <summary>
        /// Changes the positive percentage value according to the suspicious and infected value.
        /// </summary>
        public void GetPercentage()
        {
            if (Suspicious > 0)
            {
                PositivePercentage = Math.Round((double)Infected / Suspicious, 3);
            }
        }
    }
}