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
        public static Storage _instance = null;

        public static Storage Instance
        {
            get
            {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        public Hash<PatientModel> PatientsHash = new Hash<PatientModel>();
        public Hash<Bed> BedHash = new Hash<Bed>();
        public AVL<PatientStructure> PatientsByName = new AVL<PatientStructure>();
        public AVL<PatientStructure> PatientsByLastName = new AVL<PatientStructure>();
        public AVL<PatientStructure> PatientsByCUI = new AVL<PatientStructure>();
        public List<string> HCapital = new List<string>();
        public List<string> HQuetzaltenango = new List<string>();
        public List<string> HPeten = new List<string>();
        public List<string> HEscuintla = new List<string>();
        public List<string> HOriente = new List<string>();
    }
}