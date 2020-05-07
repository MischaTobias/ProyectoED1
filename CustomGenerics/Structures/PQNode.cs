using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class PQNode<T> : ICloneable
    {
        /// <summary>
        /// Variable declaration.
        /// </summary>
        public PQNode<T> Father;
        public PQNode<T> RightSon;
        public PQNode<T> LeftSon;
        public T Patient;
        public string Key;
        public int Priority;
        public DateTime DatePriority;
        /// <summary>
        /// PQNode Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="Date"></param>
        /// <param name="patient"></param>
        /// <param name="priority"></param>
        public PQNode(string key, DateTime Date, T patient, int priority)
        {
            Key = key;
            DatePriority = Date;
            Patient = patient;
            Priority = priority;
        }
        /// <summary>
        /// ICloneable implemetion clones the node
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
