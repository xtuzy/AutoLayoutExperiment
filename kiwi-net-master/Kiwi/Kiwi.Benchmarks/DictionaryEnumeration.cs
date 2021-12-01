using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Kiwi.Benchmarks
{
    public class DictionaryEnumeration
    {
        private const int N = 1000000;
        private readonly Dictionary<long, string> _dictionary = new Dictionary<long, string>();

        public DictionaryEnumeration()
        {
            for (int i = 0; i < N; i++)
            {
                _dictionary.Add(i, null);
            }
        }

        private static void DoNothing(long key, string value)
        {
        }

        [Benchmark]
        public void ForLoop1()
        {
            for (int i = 0; i < N; i++)
            {
                DoNothing(i, null);
            }
        }

        [Benchmark]
        public void ForLoop2()
        {
            for (int i = 0; i < N; i++)
            {
                DoNothing(i, null);
            }
        }

        [Benchmark]
        public void ForeachKey()
        {
            foreach (var key in _dictionary.Keys)
            {
                DoNothing(key, null);
            }
        }

        [Benchmark]
        public void ForeachKeyValueDeconstruct1()
        {
            foreach (var (key, value) in _dictionary)
            {
                DoNothing(key, value);
            }
        }

        [Benchmark]
        public void ForeachKeyValueDeconstruct2()
        {
            foreach ((var key, var value) in _dictionary)
            {
                DoNothing(key, value);
            }
        }

        [Benchmark]
        public void ForeachPair()
        {
            foreach (var entry in _dictionary)
            {
                DoNothing(entry.Key, entry.Value);
            }
        }

        [Benchmark]
        public void ForeachKeyGetValue()
        {
            foreach (var key in _dictionary.Keys)
            {
                DoNothing(key, _dictionary[key]);
            }
        }
    }

    internal static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            return source.TryGetValue(key, out var value) ? value : default(TValue);
        }

        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> source, out TKey key, out TValue value)
        {
            key = source.Key;
            value = source.Value;
        }
    }
}
