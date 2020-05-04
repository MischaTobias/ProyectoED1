﻿using CustomGenerics.Structures;
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

        public Hash<Bed> BedHash = new Hash<Bed>();
        public Hash<PatientModel> PatientsHash = new Hash<PatientModel>();
        public AVL<PatientStructure> PatientsByName = new AVL<PatientStructure>();
        public AVL<PatientStructure> PatientsByLastName = new AVL<PatientStructure>();
        public AVL<PatientStructure> PatientsByCUI = new AVL<PatientStructure>();
        public Hospital HCapital = new Hospital();
        public Hospital HQuetzaltenango = new Hospital();
        public Hospital HPeten = new Hospital();
        public Hospital HEscuintla = new Hospital();
        public Hospital HOriente = new Hospital();
        public Statistics CountryStatistics = new Statistics();
    }
}