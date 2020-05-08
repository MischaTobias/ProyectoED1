using CustomGenerics.Structures;
using ProyectoED1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoED1.Helpers
{
    public class Storage
    {
        /// <summary>
        /// Singleton implementation.
        /// </summary>
        public static Storage _instance = null;

        public static Storage Instance
        {
            get
            {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        /// <summary>
        /// Variable declaration
        /// </summary>
        public Hash<Bed> BedHash = new Hash<Bed>(50);
        public Hash<PatientModel> PatientsHash = new Hash<PatientModel>(100);
        public AVL<PatientStructure> PatientsByName = new AVL<PatientStructure>();
        public AVL<PatientStructure> PatientsByLastName = new AVL<PatientStructure>();
        public AVL<PatientStructure> PatientsByCUI = new AVL<PatientStructure>();
        public List<Hospital> Hospitals = new List<Hospital>();
        public Statistics CountryStatistics = new Statistics();
        public List<string> RepeatedNames = new List<string>();
        public List<string> RepeatedLastNames = new List<string>();
    }
}