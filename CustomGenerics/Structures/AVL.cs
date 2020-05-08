using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class AVL<T> where T : IComparable
    {
        /// <summary>
        /// Variable declaration
        /// </summary>
        public AVLNode<T> Root;
        private List<AVLNode<T>> ReturningList;
       
        /// <summary>
        ///Creates the node and then sends it to another procedure to insert it in the AVL.
        /// </summary>
        /// <param name="value"></param> Represents the value of the new node.
        /// <param name="comparison"></param> Represents the method that is being used to compare.
        public void AddPatient(T value, Comparison<T> comparison)
        {
            var newNode = new AVLNode<T>() { Patient = value };
            Insert(Root, newNode, comparison);
            
        }
        /// <summary>
        ///It goes looking for the position in the AVL by looking if its greater or lower, then add its.
        ///if the node being inserted is equal to the current node, it is added in the rightson
        ///after being added, the node is send it to a procedure to balance it.
        /// </summary>
        /// <param name="currentNode"></param> Represents the node that is being evaluated
        /// <param name="newNode"></param> Represents the node that is being added.
        /// <param name="comparison"></param> Represents the method that is being used to compare.
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
        /// <summary>
        ///The procedure searches recursively to the node that is going to delete
        ///Then it is deleted, an send it to balance node by node recursively till the root
        /// </summary>
        /// <param name="currentNode"></param> Represents the node that is being evaluated
        /// <param name="value"></param> Represents the searched value
        /// <param name="comparison"></param> Represents the method that is being used to compare.
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
        /// <summary>
        /// Find the leftmost node on the right side
        /// </summary>
        /// <param name="currentNode"></param> Represents the node being evaluated.
        /// <returns></returns>
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
        /// <summary>
        /// Find the rightmost node on the left side
        /// </summary>
        /// <param name="currentNode"></param> Represents the node being evaluated.
        /// <returns></returns>
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
        /// <summary>
        /// Calculate the balance factor, and decide the necessary rotation
        /// </summary>
        /// <param name="node"></param> Represents the node being evaluated.
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
        ///<summary>
        ///The node is exposed to a simple clockwise rotation
        ///</summary>
        /// <param name="node"></param> Represents the root of the three that it will be rotated
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
        /// <summary>
        ///The node is exposed to counterclockwise rotation
        /// </summary>
        /// <param name="node"></param> Represents the root of the three that it will be rotated
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
        ///<summary>
        ///An InOrder route is performed to obtain a list with the items in alphabetical order
        ///</summary>
        public List<AVLNode<T>> GetList()
        {
            ReturningList = new List<AVLNode<T>>();
            if (Root != null)
            {
                InOrder(Root);
            }
            return ReturningList;
        }
        /// <summary>
        /// The InOrder route, evaluated by the leftson, the root and then the rightson.
        /// </summary>
        /// <param name="currentNode"></param> Represents the node being evaluated.
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
        /// <summary>
        /// Find all nodes that are equal to the searched parameter, and return a list of them
        /// </summary>
        /// <param name="Patient"></param> Represents the searched key.
        /// <param name="node"></param> Represents the node being evaluated.
        /// <param name="comparison"></param> Represents the method that is being used to compare.
        /// <returns></returns>
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
                if (node.RightSon != null)
                {
                    return Search(Patient, node.RightSon, comparison);
                }
                else
                {
                    return new List<T>();
                }
            }
            else
            {
                if (node.LeftSon != null)
                {
                    return Search(Patient, node.LeftSon, comparison);
                }
                else
                {
                    return new List<T>();
                }
            }
        }

        /// <summary>
        /// Search recursively the nodes
        /// </summary>
        /// <param name="node"></param> Represents the node being evaluated
        /// <param name="Patient"></param> Represents the searched key.
        /// <param name="comparison"></param> Represents the method that is being used to compare.
        /// <returns></returns>
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

        public void ChangeValue(T newPatient, AVLNode<T> node, Comparison<T> comparison1, Comparison<T> comparison2)
        {
            if (node != null)
            {
                if (comparison1.Invoke(newPatient, node.Patient) < 0)
                {
                    ChangeValue(newPatient, node.LeftSon, comparison1, comparison2);
                }
                else if (comparison1.Invoke(newPatient, node.Patient) == 0)
                {
                    if (comparison2.Invoke(newPatient, node.Patient) == 0)
                    {
                        node.Patient = newPatient;
                    }
                    else
                    {
                        if (node.LeftSon != null)
                        {
                            ChangeValue(newPatient, node, comparison1, comparison2);
                        }
                        if (node.RightSon != null)
                        {
                            ChangeValue(newPatient, node, comparison1, comparison2);
                        }
                    }
                }
                else
                {
                    ChangeValue(newPatient, node.RightSon, comparison1, comparison2);
                }
            }
        }
    }
}
