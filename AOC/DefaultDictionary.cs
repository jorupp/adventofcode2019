using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AOC
{
    public class DefaultDictionary<K, V> : IDictionary<K,V>
    {
        private IDictionary<K, V> _inner;
        private V _defaultValue;

        public DefaultDictionary(IDictionary<K, V> inner, V defaultValue)
        {
            _inner = inner;
            _defaultValue = defaultValue;
        }


        public V this[K key]
        {
            get => _inner.TryGetValue(key, out var v) ? v : _defaultValue;
            set => _inner[key] = value;
        }

        #region Passthrough
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _inner).GetEnumerator();
        }

        public void Add(KeyValuePair<K, V> item)
        {
            _inner.Add(item);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return _inner.Contains(item);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return _inner.Remove(item);
        }

        public int Count => _inner.Count;

        public bool IsReadOnly => _inner.IsReadOnly;

        public void Add(K key, V value)
        {
            _inner.Add(key, value);
        }

        public bool ContainsKey(K key)
        {
            return _inner.ContainsKey(key);
        }

        public bool Remove(K key)
        {
            return _inner.Remove(key);
        }

        public bool TryGetValue(K key, out V value)
        {
            return _inner.TryGetValue(key, out value);
        }


        public ICollection<K> Keys => _inner.Keys;

        public ICollection<V> Values => _inner.Values;
        #endregion  
    }
}
