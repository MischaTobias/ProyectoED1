using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CustomGenerics.Structures
{
    public class Hash<T> where T : IComparable
    {
        public int Length;
        public HashNode<T>[] HashTable;

        public Hash(int length)
        {
            Length = length;
            HashTable = new HashNode<T>[Length];
        }

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

        public void Delete(string searchedKey, int multiplier)
        {
            int code = GetCode(searchedKey, multiplier);

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

        private int GetCode(string Key, int Multiplier)
        {
            int code = Key.Length * 11 % (Multiplier*10);
            while(code < Multiplier * 10 || code >= (Multiplier+1)*10)
            {
                if(code >= (Multiplier + 1) * 10)
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

        public HashNode<T> GetT(int pos, int block)
        {
            return HashTable[pos + block];
        }
    }
}
