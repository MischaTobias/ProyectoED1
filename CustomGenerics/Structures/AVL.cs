using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class AVL<T> where T : IComparable
    {
        public AVLNode<T> Root;

        public void Insert(AVLNode<T> currentNode, AVLNode<T> newNode, Comparison<T> comparison)
        {
            if (currentNode == null && currentNode == Root)
            {
                //currentNode = newNode;
                Root = currentNode;
            }
            else if (comparison.Invoke(currentNode.Patient, newNode.Patient) < 0)
            {
                if (currentNode.LeftSon == null)
                {
                    currentNode.LeftSon = newNode;
                    newNode.Father = currentNode;
                    Balance(currentNode);
                }
                else
                {
                    Insert(currentNode.LeftSon, newNode, comparison);
                }
            }
            else
            {
                if (currentNode.RightSon == null)
                {
                    currentNode.RightSon = newNode;
                    newNode.Father = currentNode;
                    Balance(currentNode);
                }
                else
                {
                    Insert(currentNode.RightSon, newNode, comparison);
                }
            }
        }

        public void Delete(AVLNode<T> currentNode, AVLNode<T> value, Comparison<T> comparison)
        {
            if (comparison.Invoke(currentNode.Patient, value.Patient) == 0)
            {
                if (currentNode.LeftSon != null)
                {
                    currentNode.Patient = GetReplacementLeft(currentNode.LeftSon).Patient;
                    Delete(currentNode.LeftSon, GetReplacementLeft(currentNode.LeftSon), comparison);
                }
                else if (currentNode.RightSon != null)
                {
                    currentNode.Patient = GetReplacementRight(currentNode.RightSon).Patient;
                    Delete(currentNode.RightSon, GetReplacementRight(currentNode.RightSon), comparison);
                }
                else
                {
                    var compareNode = currentNode;
                    if (currentNode.Father.LeftSon == compareNode)
                    {
                        currentNode.Father.LeftSon = null;
                    }
                    else
                    {
                        currentNode.Father.RightSon = null;
                    }
                    Balance(currentNode.Father);
                }
            }
            else if (comparison.Invoke(currentNode.Patient, value.Patient) < 0)
            {
                Delete(currentNode.LeftSon, value, comparison);
            }
            else
            {
                Delete(currentNode.RightSon, value, comparison);
            }
        }

        private AVLNode<T> GetReplacementLeft(AVLNode<T> currentNode)
        {
            if (currentNode.RightSon != null)
            {
                return GetReplacementLeft(currentNode.RightSon);
            }
            else
            {
                return currentNode;
            }
        }

        private AVLNode<T> GetReplacementRight(AVLNode<T> currentNode)
        {
            if (currentNode.LeftSon != null)
            {
                return GetReplacementRight(currentNode.LeftSon);
            }
            else
            {
                return currentNode;
            }
        }

        private void Balance(AVLNode<T> node)
        {
            if (node.GetBalanceIndex() == -2)
            {
                if (node.LeftSon.GetBalanceIndex() == 1)
                {
                    LeftRotation(node.LeftSon);
                    RightRotation(node);
                }
                else
                {
                    RightRotation(node);
                }
            }
            else if (node.GetBalanceIndex() == 2)
            {
                if (node.RightSon.GetBalanceIndex() == -1)
                {
                    RightRotation(node.RightSon);
                    LeftRotation(node);
                }
                else
                {
                    LeftRotation(node);
                }
            }
            if (node.Father != null)
            {
                Balance(node.Father);
            }
        }

        private void RightRotation(AVLNode<T> node)
        {
            AVLNode<T> newLeft = node.LeftSon.RightSon;
            node.LeftSon.RightSon = node;
            node.LeftSon.Father = node.Father;
            if (node.Father != null)
            {
                if (node.Father.RightSon == node)
                {
                    node.Father.RightSon = node.LeftSon;
                }
                else
                {
                    node.Father.LeftSon = node.LeftSon;
                }
            }
            node.Father = node.LeftSon;
            node.LeftSon = newLeft;
            if (newLeft != null)
            {
                newLeft.Father = node;
            }

            if (node.Father.Father == null)
            {
                Root = node.Father;
            }
        }

        private void LeftRotation(AVLNode<T> node)
        {
            AVLNode<T> newRight = node.RightSon.LeftSon;
            node.RightSon.LeftSon = node;
            node.RightSon.Father = node.Father;
            if (node.Father != null)
            {
                if (node.Father.RightSon == node)
                {
                    node.Father.RightSon = node.RightSon;
                }
                else
                {
                    node.Father.LeftSon = node.RightSon;
                }
            }
            node.Father = node.RightSon;
            node.RightSon = newRight;
            if (newRight != null)
            {
                newRight.Father = node;
            }
            if (node.Father.Father == null)
            {
                Root = node.Father;
            }
        }
    }
}
