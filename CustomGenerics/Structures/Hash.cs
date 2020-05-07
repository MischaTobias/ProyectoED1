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
        public HashNode<T>[] TablaHash;

        public Hash(int length)
        {
            Length = length;
            TablaHash = new HashNode<T>[Length];
        }

        public void Insert(T InsertV, string key)
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

        public void Insert (T InsertV, string key, int multiplier)
        {
            HashNode<T> T1 = new HashNode<T>();
            T1.Value = InsertV;
            T1.Key = key;
            int Originalcode = GetCode(T1.Key, multiplier);
            int code = Originalcode;
            if (TablaHash[code] != null)
            {
                while(TablaHash[code] != null)
                {
                    if(code >= (multiplier + 1) * 10)
                    {
                        code = multiplier * 10;
                    }
                    else
                    {
                        code += 1;
                    }
                }
                if (TablaHash[code] == null)
                {
                    TablaHash[code] = T1;
                }
            }
            else
            {
                TablaHash[code] = T1;
            }
        }

        public HashNode<T> Search(string searchedKey, int multiplier)
        {
            int Originalcode = GetCode(searchedKey, multiplier);
            int code = Originalcode;
            bool Isfound = false;
            while (!Isfound)
            {
                if (TablaHash[code] != null)
                {
                    if(searchedKey != TablaHash[code].Key)
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
            return TablaHash[code];
            
        }

        public HashNode<T> Search(string searchedKey)
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

        public void Delete(string searchedKey, int multiplier)
        {
            int code = GetCode(searchedKey, multiplier);

            while (TablaHash[code] == null)
            {
                code++;
            }

            if (TablaHash[code] != null)
            {
                if (TablaHash[code].Key != searchedKey)
                {
                    while (TablaHash[code].Key != searchedKey)
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
                    if (TablaHash[code].Key == searchedKey)
                    {
                        TablaHash[code] = null;
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
                    code += 10;
                }
            }
            return code;
        }

        public List<HashNode<T>> GetAsNodes()
        {
            var returnList = new List<HashNode<T>>();
            var currentNode = new HashNode<T>();
            foreach (var task in TablaHash)
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
            foreach (var task in TablaHash)
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
    }
}
