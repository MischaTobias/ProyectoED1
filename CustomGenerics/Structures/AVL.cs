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
        private List<AVLNode<T>> ReturningList;

        public void AddPatient(T value, Comparison<T> comparison)
        {
            var newNode = new AVLNode<T>() { Patient = value };
            Insert(Root, newNode, comparison);
        }

        private void Insert(AVLNode<T> currentNode, AVLNode<T> newNode, Comparison<T> comparison)
        {
            if (currentNode == null && currentNode == Root)
            {
                currentNode = newNode;
                Root = currentNode;
            }
            else if (comparison.Invoke(newNode.Patient, currentNode.Patient) < 0)
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
            if (comparison.Invoke(value.Patient, currentNode.Patient) == 0)
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
            else if (comparison.Invoke(value.Patient, currentNode.Patient) < 0)
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

        public List<AVLNode<T>> GetList()
        {
            ReturningList = new List<AVLNode<T>>();
            if (Root != null)
            {
                InOrder(Root);
            }
            return ReturningList;
        }

        private void InOrder(AVLNode<T> currentNode)
        {
            if (currentNode.LeftSon != null)
            {
                InOrder(currentNode.LeftSon);
            }
            ReturningList.Add(currentNode);
            if (currentNode.RightSon != null)
            {
                InOrder(currentNode.RightSon);
            }
        }

        public List<T> Search(T Patient, AVLNode<T> node, Comparison<T> comparison)
        {
            List<T> Patients = new List<T>();
            if(comparison.Invoke(Patient, node.Patient) == 0)
            {
                Patients.Add(node.Patient);
                List <T> RepeatedValues = Search(node.RightSon, Patient, comparison);
                if(RepeatedValues.Count > 0)
                {
                    foreach (var item in RepeatedValues)
                    {
                        Patients.Add(item);
                    }
                }
                RepeatedValues = Search(node.LeftSon, Patient, comparison);
                if (RepeatedValues.Count > 0)
                {
                    foreach (var item in RepeatedValues)
                    {
                        Patients.Add(item);
                    }
                }
                return Patients;
            }
            else if(comparison.Invoke(Patient, node.Patient) > 0)
            {
                return Search(Patient, node.RightSon, comparison);
            }
            else
            {
                return Search(Patient, node.LeftSon, comparison);
            }
        }
        private List<T> Search(AVLNode<T> node, T Patient, Comparison<T> comparison)
        {
            List<T> Patients = new List<T>();
            List<T> RepeatedValues = new List<T>();
            if (node != null)
            {  
                if (comparison.Invoke(Patient, node.Patient) == 0)
                {
                    Patients.Add(node.Patient);
                    RepeatedValues = Search(node.RightSon, Patient, comparison);
                    if (RepeatedValues.Count > 0)
                    {
                        foreach (var item in RepeatedValues)
                        {
                            Patients.Add(item);
                        }
                    }
                    RepeatedValues = Search(node.LeftSon, Patient, comparison);
                    if (RepeatedValues.Count > 0)
                    {
                        foreach (var item in RepeatedValues)
                        {
                            Patients.Add(item);
                        }
                    }
                    return Patients;
                }
                else if (comparison.Invoke(Patient, node.Patient)>0)
                {
                    RepeatedValues = Search(node.RightSon, Patient, comparison);
                    if (RepeatedValues.Count > 0)
                    {
                        foreach (var item in RepeatedValues)
                        {
                            Patients.Add(item);
                        }
                    }
                    return Patients;
                }
                else
                {
                    RepeatedValues = Search(node.LeftSon, Patient, comparison);
                    if (RepeatedValues.Count > 0)
                    {
                        foreach (var item in RepeatedValues)
                        {
                            Patients.Add(item);
                        }
                    }
                    return Patients;
                }
            }
            else
            {
                return Patients;
            }

        } 
    }
}
