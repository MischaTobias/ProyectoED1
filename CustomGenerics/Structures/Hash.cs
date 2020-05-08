using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CustomGenerics.Structures
{
    public class Hash<T> where T : IComparable
    {
        /// <summary>
        /// Variable declaration
        /// </summary>
        public int Length;
        public HashNode<T>[] HashTable;

        /// <summary>
        /// Constructor, stablishes the length of the array in the Hash
        /// </summary>
        /// <param name="length"></param> Length wanted for the array
        public Hash(int length)
        {
            Length = length;
            HashTable = new HashNode<T>[Length];
        }

        /// <summary>
        /// Inserts a new node into the hash.
        /// </summary>
        /// <param name="InsertV"></param> The value of the node that is being inserted
        /// <param name="key"></param> The key used to add into the Hash
        public void Insert(T InsertV, string key)
        {
            HashNode<T> T1 = new HashNode<T>();
            T1.Value = InsertV;
            T1.Key = key;
            int code = GetCode(T1.Key);
            if (HashTable[code] != null)
            {
                HashNode<T> Aux = HashTable[code];
                while (Aux.Next != null)
                {
                    Aux = Aux.Next;
                }
                Aux.Next = T1;
                T1.Previous = Aux;
            }
            else
            {
                HashTable[code] = T1;
            }
        }
        /// <summary>
        /// Second type of insert
        /// </summary>
        /// <param name="InsertV"></param> Value of the new node.
        /// <param name="key"></param> Key used to insert the new node.
        /// <param name="multiplier"></param> Number used to establish the range used for the series .
        public void Insert (T InsertV, string key, int multiplier)
        {
            HashNode<T> T1 = new HashNode<T>();
            T1.Value = InsertV;
            T1.Key = key;
            int Originalcode = GetCode(T1.Key, multiplier);
            int code = Originalcode;
            if (HashTable[code] != null)
            {
                while(HashTable[code] != null)
                {
                    if(code >= (multiplier + 1) * 10)
                    {
                        code = multiplier * 10;
                    }
                    else
                    {
                        code += 1;
                        //if (code == Originalcode)
                        //{
                        //    avoid enqueue
                        //}
                    }
                }
                if (HashTable[code] == null)
                {
                    HashTable[code] = T1;
                }
            }
            else
            {
                HashTable[code] = T1;
            }
        }

        /// <summary>
        /// Function that searches an object in the hash
        /// </summary>
        /// <param name="searchedKey"></param> The key that it is needed to search the element.
        /// <param name="multiplier"></param> Number used to establish the range used for the series.
        /// <returns></returns>
        public HashNode<T> Search(string searchedKey, int multiplier)
        {
            int Originalcode = GetCode(searchedKey, multiplier);
            int code = Originalcode;
            bool Isfound = false;
            while (!Isfound)
            {
                if (HashTable[code] != null)
                {
                    if(searchedKey != HashTable[code].Key)
                    {
                        if (code >= (multiplier + 1) * 10)
                        {
                            code = multiplier * 10;
                        }
                        else
                        {
                            code += 1;
                        }
                        if (code == Originalcode)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        Isfound = true;
                    }
                        
                }
                else
                {
                    code += 1;
                    if (code == Originalcode)
                    {
                        return null;
                    }
                }
            }
            return HashTable[code];
            
        }

        /// <summary>
        /// Second type to search
        /// </summary>
        /// <param name="searchedKey"></param> The key that it is needed to search.
        /// <returns></returns>
        public HashNode<T> Search(string searchedKey)
        {
            int code = GetCode(searchedKey);

            if (HashTable[code] != null)
            {

                if (HashTable[code].Key != searchedKey)
                {
                    HashNode<T> Aux = HashTable[code];
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
                    return HashTable[code];
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Delete function
        /// </summary>
        /// <param name="searchedKey"></param> The key that it is needed to search the element that it will be erased.
        /// <param name="multiplier"></param> Number used to establish the range used for the series
        public void Delete(T value, string searchedKey, int multiplier)
        {
            int code = GetCode(value, searchedKey, multiplier);

            while (HashTable[code] == null)
            {
                code++;
            }

            if (HashTable[code] != null)
            {
                if (HashTable[code].Key != searchedKey)
                {
                    while (HashTable[code].Key != searchedKey)
                    {
                        if (code >= (multiplier + 1) * 10)
                        {
                            code = multiplier * 10;
                        }
                        else
                        {
                            code += 1;
                        }
                    }
                    if (HashTable[code].Key == searchedKey)
                    {
                        HashTable[code] = null;
                    }
                }
                else
                {
                    if (HashTable[code].Next != null)
                    {
                        HashTable[code] = HashTable[code].Next;
                    }
                    else
                    {
                        HashTable[code] = null;
                    }
                }
            }
        }
        /// <summary>
        /// Get the code to insert the node in the hash
        /// </summary>
        /// <param name="Key"></param> The key that it will be used to get the code
        /// <returns></returns>
        private int GetCode(string Key)
        {
            int length = Key.Length;
            int code = 0;
            for (int i = 0; i < length; i++)
            {
               code +=  Convert.ToInt32(Key.Substring(i, 1));
            }
            code = (code * 7) % Length;
            return code;
        }

        /// <summary>
        /// Second get the code
        /// </summary>
        /// <param name="Key"></param> The key that it will be used to get the code
        /// <param name="Multiplier"></param> Number used to establish the range used for the series
        private int GetCode(string Key, int Multiplier)
        {
            int code = Key.Length * 11 % (Multiplier*10);
            while(code < Multiplier * 10)
            {
                if(code >= (Multiplier * 10))
                {
                    code -= 10;
                }
                else
                {
                    if (HashTable[code] != null)
                    {
                        code += 1;
                    }
                    else
                    {
                        return code;
                    }
                }
            }
            return code;
        }

        /// <summary>
        /// Looks up for the T value received and then returns its position in the array.
        /// </summary>
        /// <param name="value"></param> represents the searched value
        /// <param name="Key"></param> represents the key that gives access to a position in the array in which the value should be.
        /// <param name="Multiplier"></param> reduces the section in which the value is searched throughout the array.
        /// <returns></returns>
        private int GetCode(T value, string Key, int Multiplier)
        {
            int code = Key.Length * 11 % (Multiplier * 10);
            while (code < Multiplier * 10)
            {
                if (code >= (Multiplier * 10))
                {
                    code -= 10;
                }
                else
                {
                    if (HashTable[code] != null)
                    {
                        if (HashTable[code].Value.CompareTo(value) == 0)
                        {
                            return code;
                        }
                        else
                        {
                            code += 1;
                        }
                    }
                    else
                    {
                        return code;
                    }
                }
            }
            return code;
        }


        /// <summary>
        /// Get a list of all nodes in the hash
        /// </summary>
        public List<HashNode<T>> GetAsNodes()
        {
            var returnList = new List<HashNode<T>>();
            var currentNode = new HashNode<T>();
            foreach (var task in HashTable)
            {
                currentNode = task;
                while (currentNode != null)
                {
                    returnList.Add(currentNode);
                    currentNode = currentNode.Next;
                }
            }
            return returnList;
        }

        /// <summary>
        /// Get a list of items that fulfill a condition
        /// </summary>
        /// <param name="predicate"></param> The condition to add to the list
        /// <returns></returns>
        public List<T> GetFilterList(Func<T,bool> predicate)
        {
            List<T> FiltedList = new List<T>();
            var currentNode = new HashNode<T>();
            foreach (var task in HashTable)
            {
                currentNode = task;
                while (currentNode != null)
                {
                    if (predicate(currentNode.Value))
                    {
                        FiltedList.Add(currentNode.Value);
                    }
                    currentNode = currentNode.Next;
                }
            }
            return FiltedList;
        }

        /// <summary>
        /// Returns the value in the position given.
        /// </summary>
        /// <param name="pos"></param> int of the key position.
        /// <param name="block"></param> int that represents the section of values which are going to be returned.
        /// <returns></returns>
        public HashNode<T> GetT(int pos, int block)
        {
            return HashTable[pos + block];
        }
    }
}
