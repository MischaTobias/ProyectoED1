using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    class AVLNode
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int CUI { get; set; }
        public AVLNode RightSon { get; set; }
        public AVLNode LeftSon { get; set; }
    }
}
