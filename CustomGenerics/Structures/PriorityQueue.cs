using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class PriorityQueue<T> : ICloneable, IEnumerable<T>
    {
        /// <summary>
        /// Variable declaration.
        /// </summary>
        public PQNode<T> Root;
        public int PatientsNumber;
        public PriorityQueue<T> queueCopy;

        /// <summary>
        /// Constructor, stablishes patients number as 0.
        /// </summary>
        public PriorityQueue()
        {
            PatientsNumber = 0;
        }

        /// <summary>
        /// Returns true if the root is null, else returns false.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Root == null ? true : false;
        }

        /// <summary>
        /// Returns true if the queue already has 10 patients, else returns false.
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return PatientsNumber == 10 ? true : false;
        }

        /// <summary>
        /// Adds a new patient to the priorityQueue and calls OrderDowntoUp()
        /// </summary>
        /// <param name="key"></param> represents the node's key.
        /// <param name="date"></param> represents date as a secondary priority criteria.
        /// <param name="patient"></param> represents the value that's been inserted to the queue
        /// <param name="priority"></param> represents the patient's priority.
        public void AddPatient(string key, DateTime date, T patient, int priority)
        {
            var newNode = new PQNode<T>(key, date, patient, priority);
            if (IsEmpty())
            {
                Root = newNode;
                PatientsNumber = 1;
            }
            else
            {
                PatientsNumber++;
                var NewNodeFather = SearchLastNode(Root, 1);
                if (NewNodeFather.LeftSon != null)
                {
                    NewNodeFather.RightSon = newNode;
                    newNode.Father = NewNodeFather;
                    OrderDowntoUp(newNode);
                }
                else
                {
                    NewNodeFather.LeftSon = newNode;
                    newNode.Father = NewNodeFather;
                    OrderDowntoUp(newNode);
                }

            }
        }
        /// <summary>
        /// Compare priorities and change if necessary, sort from current node to root
        /// </summary>
        /// <param name="current"></param> Represents the node being evaluated
        private void OrderDowntoUp(PQNode<T> current)
        {
            if (current.Father != null)
            {
                if (current.Priority < current.Father.Priority)
                {
                    ChangeNodes(current);
                }
                else if (current.Priority == current.Father.Priority)
                {
                    if (current.DatePriority < current.Father.DatePriority)
                    {
                        ChangeNodes(current);
                    }
                }
                OrderDowntoUp(current.Father);
            }
        }
        /// <summary>
        /// Compare priorities and change if necessary, sort from root to the leaves
        /// </summary>
        /// <param name="current"></param> Represents the node being evaluated
        private void OrderUptoDown(PQNode<T> current)
        {
            if (current.RightSon != null && current.LeftSon != null)
            {
                if (current.LeftSon.Priority > current.RightSon.Priority)
                {
                    if (current.Priority > current.RightSon.Priority)
                    {
                        ChangeNodes(current.RightSon);
                        OrderUptoDown(current.RightSon);
                    }
                    else if (current.Priority == current.RightSon.Priority)
                    {
                        if (current.DatePriority > current.RightSon.DatePriority)
                        {
                            ChangeNodes(current.RightSon);
                            OrderUptoDown(current.RightSon);
                        }
                    }
                }
                else if (current.LeftSon.Priority < current.RightSon.Priority)
                {
                    if (current.Priority > current.LeftSon.Priority)
                    {
                        ChangeNodes(current.LeftSon);
                        OrderUptoDown(current.LeftSon);
                    }
                    else if (current.Priority == current.LeftSon.Priority)
                    {
                        if (current.DatePriority > current.LeftSon.DatePriority)
                        {
                            ChangeNodes(current.LeftSon);
                            OrderUptoDown(current.LeftSon);
                        }
                    }
                }
                else
                {
                    if (current.LeftSon.DatePriority > current.RightSon.DatePriority)
                    {
                        if (current.Priority > current.RightSon.Priority)
                        {
                            ChangeNodes(current.RightSon);
                            OrderUptoDown(current.RightSon);
                        }
                        else if (current.Priority == current.RightSon.Priority)
                        {
                            if (current.DatePriority > current.RightSon.DatePriority)
                            {
                                ChangeNodes(current.RightSon);
                                OrderUptoDown(current.RightSon);
                            }
                        }
                    }
                    else
                    {
                        if (current.Priority > current.LeftSon.Priority)
                        {
                            ChangeNodes(current.LeftSon);
                            OrderUptoDown(current.LeftSon);
                        }
                        else if (current.Priority == current.LeftSon.Priority)
                        {
                            if (current.DatePriority > current.LeftSon.DatePriority)
                            {
                                ChangeNodes(current.LeftSon);
                                OrderUptoDown(current.LeftSon);
                            }
                        }
                    }
                }
            }
            else if (current.RightSon != null)
            {
                if (current.Priority > current.RightSon.Priority)
                {
                    ChangeNodes(current.RightSon);
                    OrderUptoDown(current.RightSon);
                }
                else if (current.Priority == current.RightSon.Priority)
                {
                    if (current.DatePriority > current.RightSon.DatePriority)
                    {
                        ChangeNodes(current.RightSon);
                        OrderUptoDown(current.RightSon);
                    }
                }
            }
            else if (current.LeftSon != null)
            {
                if (current.Priority > current.LeftSon.Priority)
                {
                    ChangeNodes(current.LeftSon);
                    OrderUptoDown(current.LeftSon);
                }
                else if (current.Priority == current.LeftSon.Priority)
                {
                    if (current.DatePriority > current.LeftSon.DatePriority)
                    {
                        ChangeNodes(current.LeftSon);
                        OrderUptoDown(current.LeftSon);
                    }
                }
            }
        }
        /// <summary>
        /// Swaps the current node with its father
        /// </summary>
        /// <param name="node"></param> The node being exchanged
        private void ChangeNodes(PQNode<T> node)
        {
            var Priority1 = node.Priority;
            var Key1 = node.Key;
            var Date1 = node.DatePriority;
            var Patient1 = node.Patient;
            node.Priority = node.Father.Priority;
            node.Key = node.Father.Key;
            node.DatePriority = node.Father.DatePriority;
            node.Patient = node.Father.Patient;
            node.Father.Priority = Priority1;
            node.Father.Key = Key1;
            node.Father.DatePriority = Date1;
            node.Father.Patient = Patient1;

        }
        /// <summary>
        /// Remove the first node from the Priority Queue.
        /// </summary>
        /// <returns></returns>
        public PQNode<T> GetFirst()
        {
            if (Root == null)
            {
                return null;
            }
            PQNode<T> LastNode = SearchLastNode(Root, 1);
            PQNode<T> FirstNode = (PQNode<T>)Root.Clone();
            var LastNodeCopy = (PQNode<T>)LastNode.Clone();
            Root.Key = LastNodeCopy.Key;
            Root.Priority = LastNodeCopy.Priority;
            Root.Patient = LastNodeCopy.Patient;
            Root.DatePriority = LastNodeCopy.DatePriority;
            if (LastNode.Father == null)
            {
                Root = null;
                PatientsNumber--;
                return LastNode;
            }
            else
            {
                if (LastNode.Father.LeftSon == LastNode)
                {
                    LastNode.Father.LeftSon = null;
                }
                else
                {
                    LastNode.Father.RightSon = null;
                }
            }
            OrderUptoDown(Root);
            PatientsNumber--;
            return FirstNode;
        }
        /// <summary>
        /// It searches the last node added to the Priority Queue.
        /// </summary>
        /// <param name="current"></param> The current node being evaluated.
        /// <param name="number"></param> Total number of elements
        /// <returns></returns>
        private PQNode<T> SearchLastNode(PQNode<T> current, int number)
        {
            try
            {
                int previousn = PatientsNumber;
                if (previousn == number)
                {
                    return current;
                }
                else
                {
                    while (previousn / 2 != number)
                    {
                        previousn = previousn / 2;
                    }
                    if (previousn % 2 == 0)
                    {
                        if (current.LeftSon != null)
                        {
                            return SearchLastNode(current.LeftSon, previousn);
                        }
                        else
                        {
                            return current;
                        }
                    }
                    else
                    {
                        if (current.RightSon != null)
                        {
                            return SearchLastNode(current.RightSon, previousn);
                        }
                        else
                        {
                            return current;
                        }
                    }
                }
            }
            catch   
            {
                return current;
            }

        }

        /// <summary>
        /// ICloneable implemetion, clones the PriorityQueue
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// IEnumerator implementation 
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            var queueCopy = new PriorityQueue<T>() { Root = this.Root, PatientsNumber = this.PatientsNumber };
            var current = queueCopy.Root;
            while (current != null)
            {
                yield return current.Patient;
                current = queueCopy.GetFirst();
            }
        }

        /// <summary>
        /// IEnumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
