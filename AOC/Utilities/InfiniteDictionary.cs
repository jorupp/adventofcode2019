using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.Utilities
{
    public class InfiniteDictionary<K, V> : IDictionary<K, V>
    {
        public InfiniteDictionary(V defaultValue)
        {
            DefaultValue = defaultValue;
        }

        private readonly Dictionary<K, V> inner = new Dictionary<K, V>();
        public V DefaultValue { get; }

        public V this[K key] { get => inner.TryGetValue(key, out var v) ? v : DefaultValue; set => inner[key] = value; }

        public bool IsReadOnly => false;

        public ICollection<K> Keys => ((IDictionary<K, V>)inner).Keys;

        public ICollection<V> Values => ((IDictionary<K, V>)inner).Values;

        public int Count => ((ICollection<KeyValuePair<K, V>>)inner).Count;

        public void Add(K key, V value)
        {
            ((IDictionary<K, V>)inner).Add(key, value);
        }

        public bool ContainsKey(K key)
        {
            return ((IDictionary<K, V>)inner).ContainsKey(key);
        }

        public bool Remove(K key)
        {
            return ((IDictionary<K, V>)inner).Remove(key);
        }

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            return ((IDictionary<K, V>)inner).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            ((ICollection<KeyValuePair<K, V>>)inner).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<K, V>>)inner).Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return ((ICollection<KeyValuePair<K, V>>)inner).Contains(item);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<K, V>>)inner).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return ((ICollection<KeyValuePair<K, V>>)inner).Remove(item);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<K, V>>)inner).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)inner).GetEnumerator();
        }
    }
}
