using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class PatientModel : IComparable
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Municipality { get; set; }
        public string Symptoms { get; set; }
        public string InfectionDescription { get; set; }
        public int CUI { get; set; }
        public int Age { get; set; }
        public int Priority { get; set; }
        public char Status { get; set; }
        public DateTime ArrivalDate { get; set; }

        public int CompareTo(object obj)
        {
            return this.Age.CompareTo(((PatientModel)obj).Age);
        }

        public PatientModel()
        {
            PriorityAssignment();
        }
        public void PriorityAssignment()
        {
            if (Age > 60)
            {
                if (IsInfected)
                {
                    Priority = 1;
                }
                else
                {
                    Priority = 4;
                }
            }
            else if (17 < Age && Age <= 60)
            {
                if (IsInfected)
                {
                    Priority = 3;
                }
                else
                {
                    Priority = 7;
                }
            }
            else if (1 < Age && Age <= 17)
            {
                if (IsInfected)
                {
                    Priority = 5;
                }
                else
                {
                    Priority = 8;
                }
            }
            else
            {
                if (IsInfected)
                {
                    Priority = 2;
                }
                else
                {
                    Priority = 6;
                }
            }
        }
    }
}