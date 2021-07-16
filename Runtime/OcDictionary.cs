using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OcUtility
{
    [Serializable]
    public class OcDictionary<TKey, TValue> : IEnumerable<OcKVPair<TKey, TValue>>
    {
        [TableList(AlwaysExpanded = true)]public List<OcKVPair<TKey, TValue>> pairs;

        public OcDictionary()
        {
            pairs = new List<OcKVPair<TKey, TValue>>();
        }

        public TValue this[TKey key]
        {
            get => FindPair(key).Value;
            set
            {
                var exist = FindPair(key);
                if (exist == null)
                {
                    var pair = new OcKVPair<TKey, TValue>(key, value);
                    pairs.Add(pair);
                }
                else exist.Value = value;

            }
        }

        public bool ContainsKey(TKey key)
        {
            var exist = FindPair(key);
            return exist != null;
        }

        public bool ContainsValue(TValue value)
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                if (pairs[i].Value.Equals(value)) return true;
            }

            return false;
        }

        OcKVPair<TKey, TValue> FindPair(TKey key)
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                if (pairs[i].Key.Equals(key)) return pairs[i];
            }

            return null;
        }

        public IEnumerator<OcKVPair<TKey, TValue>> GetEnumerator()
        {
            return pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    [Serializable]
    public class OcKVPair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public OcKVPair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
