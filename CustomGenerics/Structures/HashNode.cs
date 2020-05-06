using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class HashNode<T> where T : IComparable
    {
        public HashNode<T> Previous { get; set; }
        public HashNode<T> Next { get; set; }
        public T Value { get; set; }
        public string Key { get; set; }

    }
}
