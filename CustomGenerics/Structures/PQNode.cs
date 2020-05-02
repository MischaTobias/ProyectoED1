using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class PQNode<T>
    {
        public PQNode<T> Father;
        public PQNode<T> RightSon;
        public PQNode<T> LeftSon;
        public int Key;
        public int Priority;
        public DateTime DatePriority;

        public PQNode(int key, DateTime Date, int priority)
        {
            Key = key;
            DatePriority = Date;
            Priority = priority;
        }
    }
}
