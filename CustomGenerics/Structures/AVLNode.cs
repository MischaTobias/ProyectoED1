using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class AVLNode<T> where T : IComparable
    {
        public AVLNode<T> LeftSon { get; set; }
        public AVLNode<T> RightSon { get; set; }
        public AVLNode<T> Father { get; set; }
        public T Patient { get; set; }

        public int GetBalanceIndex()
        {
            if (this.LeftSon != null && this.RightSon != null)
            {
                return this.RightSon.GetTreeHeight() - this.LeftSon.GetTreeHeight();
            }
            else if (this.LeftSon == null)
            {
                if (this.RightSon == null)
                {
                    return 0;
                }
                else
                {
                    return this.RightSon.GetTreeHeight();
                }
            }
            else
            {
                return this.LeftSon.GetTreeHeight() * -1;
            }
        }

        public int GetTreeHeight()
        {
            if (this.LeftSon == null && this.RightSon == null)
            {
                return 1;
            }
            else if (this.LeftSon == null || this.RightSon == null)
            {
                if (this.LeftSon == null)
                {
                    return this.RightSon.GetTreeHeight() + 1;
                }
                else
                {
                    return this.LeftSon.GetTreeHeight() + 1;
                }
            }
            else
            {
                if (this.LeftSon.GetTreeHeight() > this.RightSon.GetTreeHeight())
                {
                    return this.LeftSon.GetTreeHeight() + 1;
                }
                else
                {
                    return this.RightSon.GetTreeHeight() + 1;
                }
            }
        }
    }
}
