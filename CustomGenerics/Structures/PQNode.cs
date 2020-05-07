using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class PQNode<T> : ICloneable
    {
        public PQNode<T> Father;
        public PQNode<T> RightSon;
        public PQNode<T> LeftSon;
        public T Patient;
        public string Key;
        public int Priority;
        public DateTime DatePriority;

        public PQNode(string key, DateTime Date, T patient, int priority)
        {
            Key = key;
            DatePriority = Date;
            Patient = patient;
            Priority = priority;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
