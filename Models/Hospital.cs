using CustomGenerics.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoED1.Helpers;

namespace ProyectoED1.Models
{
    public class Hospital
    {
        public PriorityQueue <PatientModel> InfectedPatients { get; set; }
        public PriorityQueue <PatientModel> SuspiciousPatients { get; set; }
        public int[] Bedcodearray = new int[10];

        public bool BedFull()
        {
            bool IsFull = false;
            foreach (var item in Bedcodearray)
            {
                //if(Search Hash the bed.Availability ==true){
                IsFull = true;
            //}            
                
            }
            return IsFull;
        } 
        public bool InfectedQueueFull()
        {
            return InfectedPatients.IsFull();
        }
        public bool SuspiciousQueueFull()
        {
            return SuspiciousPatients.IsFull();
        }
        public void InfectionTest()
        {
            //PatientModel patient=  //Hash.Search(SuspiciousPatients.Delete().Key);
            //Realizar la prueba
            if(patient.IsInfected)
            {
                if (!InfectedQueueFull)
                {
                    InfectedPatients.Insert(patient);
                }
                else
                {
                    //En caso la lista de enfermos este llena
                }
            }
        }

    }
}