using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class Hash<T> where T : IComparable
    {
        HashNode<T>[] TablaHash = new HashNode<T>[100];

        public void Insert(T InsertV, int key)
        {
            HashNode<T> T1 = new HashNode<T>();
            T1.Value = InsertV;

            T1.Key = key;
            int code = GetCode(T1.Key);
            if (TablaHash[code] != null)
            {
                HashNode<T> Aux = TablaHash[code];
                while (Aux.Next != null)
                {
                    Aux = Aux.Next;
                }
                Aux.Next = T1;
                T1.Previous = Aux;
            }
            else
            {
                TablaHash[code] = T1;
            }
        }

        public HashNode<T> Search(int searchedKey)
        {
            int code = GetCode(searchedKey);

            if (TablaHash[code] != null)
            {

                if (TablaHash[code].Key != searchedKey)
                {
                    HashNode<T> Aux = TablaHash[code];
                    while (Aux.Key != searchedKey && Aux.Next != null)
                    {
                        Aux = Aux.Next;
                    }
                    if (Aux.Key == searchedKey)
                    {
                        return Aux;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return TablaHash[code];
                }
            }
            else
            {
                return null;
            }
        }

        public void Delete(int searchedKey)
        {
            int code = GetCode(searchedKey);

            if (TablaHash[code] != null)
            {

                if (TablaHash[code].Key != searchedKey)
                {
                    HashNode<T> Aux = TablaHash[code];
                    while (Aux.Key != searchedKey && Aux.Next != null)
                    {
                        Aux = Aux.Next;
                    }
                    if (Aux.Key == searchedKey)
                    {
                        if (Aux.Next != null)
                        {
                            Aux.Next.Previous = Aux.Previous;
                        }
                        if (Aux.Previous != null)
                        {
                            Aux.Previous.Next = Aux.Next;
                        }
                        if (Aux.Next == null && Aux.Previous == null)
                        {
                            Aux = null;
                        }
                    }
                }
                else
                {
                    if (TablaHash[code].Next != null)
                    {
                        TablaHash[code] = TablaHash[code].Next;
                    }
                    else
                    {
                        TablaHash[code] = TablaHash[code].Next;
                    }
                }
            }
        }
        private int GetCode(int Key)
        {
            string key = Key.ToString();
            int length = key.Length;
            int code = 0;
            for (int i = 0; i < length; i++)
            {
               code +=  Convert.ToInt32(key.Substring(i, 1));
            }
            code = (code * 3) % 100;
            return code;

        }
        public List<HashNode<T>> GetTasksAsNodes()
        {
            var listOfTasks = new List<HashNode<T>>();
            var currentNode = new HashNode<T>();
            foreach (var task in TablaHash)
            {
                currentNode = task;
                while (currentNode != null)
                {
                    listOfTasks.Add(currentNode);
                    currentNode = currentNode.Next;
                }
            }
            return listOfTasks;
        }

    }
}
