//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Kiwi_Ts
//{
//    public delegate T2 Factory<T2>();

//    public interface IKeyId
//    {
//        public int id();
//    }

//    public class IMap<T1, T2> : IndexedMap<T1, T2>
//        where T1 : class,IKeyId
//        where T2 :  new()
//    {
//        public static IMap<T1, T2> createMap()
//        {
//            return new IMap<T1, T2>();
//        }
//    }

//    public class IndexedMap<T1, T2>
//        where T1 : class, IKeyId
//        where T2 : new()
//    {
//        public static readonly int DefaultIndexValue = -1;
//        //public int[] index = Enumerable.Repeat<int>(DefaultIndexValue, 100).ToArray();
//        //public List<int> index = new List<int>();
//        public Dictionary<int, int> index = new Dictionary<int, int>();
//        //public ArrayList index = new ArrayList();
        
//        public List<Pair<T1, T2>> array = new List<Pair<T1, T2>>();

        

//        /**
//        * Returns the number of items in the array.
//        */
//        public int size()
//        {
//            return this.array.Count;
//        }

//        /**
//         * Returns true if the array is empty.
//         */
//        public bool empty()
//        {
//            return this.array.Count == 0;
//        }

//        /**
//         * Returns the item at the given array index.
//         *
//         * @param index The integer index of the desired item.
//         */
//        public Pair<T1, T2> itemAt(int index)
//        {
//            return this.array[index];
//        }

//        /**
//         * Returns true if the key is in the array, false otherwise.
//         *
//         * @param key The key to locate in the array.
//         */
//        public bool contains(T1 key)
//        {
//            if(!this.index.ContainsKey(key.id()))//不存在
//                return false;
//            return this.index[key.id()]  != DefaultIndexValue;
//        }

//        /**
//         * Returns the pair associated with the given key, or undefined.
//         *
//         * @param key The key to locate in the array.
//         */
//        public Pair<T1, T2> find(T1 key)
//        {
//            if (!this.index.ContainsKey(key.id()))//不存在
//                return null;
//            var i = this.index[key.id()];
//            return i == DefaultIndexValue ? null : this.array[i];
//        }

        
//        /**
//        * Returns the pair associated with the key if it exists.
//        *
//        * If the key does not exist, a new pair will be created and
//        * inserted using the value created by the given factory.
//        * 如果不存在,会创建
//        * @param key The key to locate in the array.
//        * @param factory The function which creates the default value.
//        */
//        public Pair<T1, T2> setDefault(T1 key, Factory<T2>? factory)
//        {
//            if(!this.index.ContainsKey(key.id()))//不存在
//            { 
//                var pair = new Pair<T1, T2>(key, factory.Invoke());
//                this.index.Add(key.id(), this.array.Count);
//                this.array.Add(pair);
//                return pair;
//            }
//            else//存在
//            {
//                var i = this.index[key.id()];
//                if(i == DefaultIndexValue)//但为-1
//                {
//                    var pair = new Pair<T1, T2>(key, factory.Invoke());
//                    this.index[key.id()] = this.array.Count;
//                    this.array.Add(pair);
//                    return pair;
//                }
//                return this.array[i];
//            }
//        }

//        /**
//        * Insert the pair into the array and return the pair.
//        * 插入
//        * This will overwrite any existing entry in the array.
//        * 这会覆盖已存在的
//        * @param key The key portion of the pair.
//        * @param value The value portion of the pair.
//        */
//        public Pair<T1, T2> Insert(T1 key, T2 value)
//        {
            
//            var pair = new Pair<T1, T2>(key, value);
            
//            if(this.index.ContainsKey(key.id()))//如果存在
//            {
//                var i = this.index[key.id()];
//                if (i == DefaultIndexValue)//但为-1
//                {
//                    this.index[key.id()] = this.array.Count;
//                    this.array.Add(pair);
//                }
//                else
//                {
//                    this.array[i] = pair;
//                }
//            }
//            else//不存在
//            {
//                this.index.Add(key.id(),this.array.Count);
//                this.array.Add(pair);
//            }
            
           
//            return pair;
//        }

//        /**
//        * Removes and returns the pair for the given key, or undefined.
//        *
//        * @param key The key to remove from the map.
//        */
//        public Pair<T1, T2> Remove(T1 key)
//        {
//            var i = this.index[key.id()];
//            if (i == DefaultIndexValue)
//            {
//                return null;
//            }
//            this.index[key.id()] = DefaultIndexValue;
//            var pair = this.array[i];
//            var last = this.array[array.Count - 1];
//            this.array.RemoveAt(array.Count - 1);//因为需要存储的index,只能移除最后一个,不然index会乱
//            if (pair!=last)//如果要移除的和最后的不一样,那就挪动最后一个到要移除的位置
//            {
//                this.array[i] = last;
//                this.index[last.Key.id()] = i;
//            }
//            return pair;
//        }

//        /**
//        * Create a copy of this associative array.
//        */
//        public IndexedMap<T1, T2> copy()
//        {
//            var copy = new IndexedMap<T1, T2>();
            
//            for (var i = 0; i < this.array.Count; i++)
//            {
//                var pair = this.array[i].copy();
//                copy.array.Add(pair);
//                copy.index[pair.Key.id()] = i;
//            }
//            return copy;
//        }

        

//        public IEnumerator<Pair<T1, T2>> GetEnumerator()
//        {
//            foreach(var pair in this.array)
//            {
//                yield return pair;
//            }
//        }
//    }

//    public class Pair<T, U>
//    {
//        public T Key;
//        public U Value;
//        /**
//         * Construct a new Pair object.
//         *
//         * @param first The first item of the pair.
//         * @param second The second item of the pair.
//         */
//        public Pair(T key, U value)
//        {
//            this.Key = key;
//            Value = value;
//        }

//        /**
//         * Create a copy of the pair.
//         */
//        public Pair<T, U> copy()
//        {
//            return new Pair<T, U>(this.Key, this.Value);
//        }

        

//    }
//}
