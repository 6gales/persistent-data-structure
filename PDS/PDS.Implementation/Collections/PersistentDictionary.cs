﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using PDS.Collections;

namespace PDS.Implementation.Collections
{
    public class PersistentDictionary<TKey, TValue> : IPersistentDictionary<TKey, TValue> where TKey : notnull
    {
        private IPersistentList<List<KeyValuePair<TKey, TValue>>> _buckets;

        public PersistentDictionary(int count, IPersistentList<List<KeyValuePair<TKey, TValue>>> buckets)
        {
            Count = count;
            _buckets = buckets;
        }

        public int Count { get; }

        private (int index, List<KeyValuePair<TKey, TValue>> bucket) GetBucket(TKey key)
        {
            var index = key.GetHashCode() % _buckets.Count;
            return (index, _buckets[index]);
        }

        public TValue GetByKey(TKey key)
        {
            var (_, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    return v;
                }
            }

            throw new KeyNotFoundException(key.ToString());
        }

        public bool Contains(TKey key)
        {
            var (_, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }

        public PersistentDictionary<TKey, TValue> Set(TKey key, TValue value)
        {
            if (_buckets.Count < 0.67 * Count)
            {
                Reallocate(2 * _buckets.Count);
            }

            var (index, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    return Equals(v, value) ? this : Update(bucket, index, key, value);
                }
            }

            var newBucket = new List<KeyValuePair<TKey, TValue>>(bucket) {new(key, value)};
            var newBuckets = _buckets.SetItem(index, newBucket);

            return new PersistentDictionary<TKey, TValue>(Count + 1, newBuckets);
        }

        private PersistentDictionary<TKey, TValue> Update(List<KeyValuePair<TKey, TValue>> bucket, int index, TKey key,
            TValue value)
        {
            var newBucket = bucket
                .Where(kv => !kv.Key.Equals(key))
                .Append(new KeyValuePair<TKey, TValue>(key, value))
                .ToList();
            var newBuckets = _buckets.SetItem(index, newBucket);
            return new PersistentDictionary<TKey, TValue>(Count, newBuckets);
        }

        private void Reallocate(int newSize)
        {
            var array = Enumerable.Range(0, newSize).Select(i => new List<KeyValuePair<TKey, TValue>>()).ToArray();
            foreach (var keyValuePair in this)
            {
                var index = keyValuePair.Key.GetHashCode() % newSize;
                array[index].Add(keyValuePair);
            }

            //TODO: _buckets = new PersistentArray<List<KeyValuePair<TKey, TValue>>>(array);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.AddRange(
            IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Remove(TKey key)
        {
            return Remove(key);
        }

        public bool TryRemove(TKey key, out IPersistentDictionary<TKey, TValue> newVersion)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.SetItem(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.SetItems(
            IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Clear()
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Add(TKey key, TValue value) =>
            Set(key, value);

        public IPersistentDictionary<TKey, TValue> AddOrUpdate(TKey key, TValue value) => Set(key, value);

        public IPersistentDictionary<TKey, TValue> Update(TKey key, Func<TKey, TValue, TValue> valueFactory)
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(TKey key, TValue value, out IPersistentDictionary<TKey, TValue> newVersion)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            throw new NotImplementedException();
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Remove(TKey key)
        {
            return Remove(key);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys)
        {
            throw new NotImplementedException();
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItem(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItems(
            IEnumerable<KeyValuePair<TKey, TValue>> items) => AddRange(items);

        bool IImmutableDictionary<TKey, TValue>.TryGetKey(TKey equalKey, out TKey actualKey)
        {
            actualKey = equalKey;
            return true;
        }

        public PersistentDictionary<TKey, TValue> Remove(TKey key)
        {
            var (index, bucket) = GetBucket(key);

            var newBucket = bucket
                .Where(kv => !kv.Key.Equals(key))
                .ToList();

            if (newBucket.Count == bucket.Count)
            {
                throw new ArgumentException($"Key does not exist: {key}");
            }

            var newBuckets = _buckets.SetItem(index, newBucket);
            return new PersistentDictionary<TKey, TValue>(Count - 1, newBuckets);
        }


        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _buckets.SelectMany(bucket => bucket).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IPersistentDictionary<TKey, TValue> Add(KeyValuePair<TKey, TValue> pair)
        {
            return Set(pair.Key, pair.Value);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            return Set(key, value);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.AddRange(
            IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            return AddRange(pairs);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Clear()
        {
            return Clear();
        }

        public IPersistentDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var dict = this;
            foreach (var (key, value) in items)
            {
                dict = dict.Set(key, value);
            }

            return dict;
        }

        public IPersistentDictionary<TKey, TValue> AddRange(IReadOnlyCollection<KeyValuePair<TKey, TValue>> items)
        {
            return AddRange(items.AsEnumerable());
        }

        public IPersistentDictionary<TKey, TValue> Clear()
        {
            return new PersistentDictionary<TKey, TValue>(0, _buckets.Clear());
        }

        public bool IsEmpty => Count == 0;
        public bool ContainsKey(TKey key) => Contains(key);

#pragma warning disable CS8767
        public bool TryGetValue(TKey key, out TValue? value)
#pragma warning restore CS8767
        {
            var (_, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    value = v;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public TValue this[TKey key] => GetByKey(key);

        public IEnumerable<TKey> Keys => this.Select(kvp => kvp.Key);

        public IEnumerable<TValue> Values => this.Select(kvp => kvp.Value);
    }
}