using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcUtility
{
    [Serializable]
    public class OcDictionary<TKey, TValue> : IEnumerable<OcKVPair<TKey, TValue>>
    {
        [TableList(AlwaysExpanded = true)]public List<OcKVPair<TKey, TValue>> pairs;
        public int Count => pairs.Count;
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

        public bool Remove(TKey key)
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                if (pairs[i].Key.Equals(key))
                {
                    pairs.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public int RemoveAll(TKey key)
        {
            var count = 0;
            for (int i = pairs.Count - 1; i > 0 ; i--)
            {
                if (pairs[i].Key.Equals(key))
                {
                    pairs.RemoveAt(i);
                    count++;
                }
            }

            return count;
        }

        public int RemoveAll(TValue value)
        {
            var count = 0;
            for (int i = pairs.Count - 1; i > 0 ; i--)
            {
                if (pairs[i].Value.Equals(value))
                {
                    pairs.RemoveAt(i);
                    count++;
                }
            }

            return count;
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

        public Dictionary<TKey, TValue> ToDictionary()
        {
            var dict = new Dictionary<TKey, TValue>();
            foreach (var kvPair in this)
            {
                dict[kvPair.Key] = kvPair.Value;
            }
            return dict;
        }
    }
    
    [Serializable]
    public class OcKVPair<TKey, TValue>
    {
        public TKey Key;
        [HideLabel][VerticalGroup("Value")][HideIf("@Value is UnityEngine.Object")]public TValue Value;

        [VerticalGroup("Value")][ShowInInspector][ShowIf("@Value is UnityEngine.Object")][PreviewField(ObjectFieldAlignment.Left)]
        [HideLabel][SuffixLabel("@Preview == null ? \"null\" : Preview.name")]
        Object Preview
        {
            get => Value as Object;
            set
            {
                if(value is TValue value1) Value = value1;
            }
        }
        public OcKVPair(){}

        public OcKVPair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
