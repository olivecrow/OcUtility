using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcUtility
{
    public class OcPool<T> : IDisposable, IDisposableDebugTarget where T : MonoBehaviour, IPoolMember<T>
    {
        static Dictionary<IPoolMember<T>, OcPool<T>> GlobalPool;
        
        static OcPool()
        {
#if UNITY_EDITOR
            Application.quitting += () =>
            {
                GlobalPool = new Dictionary<IPoolMember<T>, OcPool<T>>();
            };   
#endif
            GlobalPool = new Dictionary<IPoolMember<T>, OcPool<T>>();
        }

        public static OcPool<T> MakePool(T source, int count, Transform folder = null)
        {
            OcPool<T> targetPool;
            if (GlobalPool.ContainsKey(source))
            {
                targetPool = GlobalPool[source];
                targetPool.AddMember(count);
            }
            else targetPool = new OcPool<T>(source, count, folder);

            return targetPool;
        }

        /// <summary> 글로벌 풀에서 해당 소스 기반의 풀에서 Call을 반환함. 해당 소스의 풀이 없을 경우 null을 반환함. </summary>
        public static T Call(T source, in Vector3 position, in Quaternion rotation)
        {
            if (GlobalPool.ContainsKey(source))
            {
                var targetPool = GlobalPool[source];
                
                // 씬이 변경되는 등의 이유로 참조가 Missing 상태일 경우 다시 풀을 재생성함.
                if (targetPool.Folder == null)
                {
                    GlobalPool[source] = MakePool(source, targetPool.InitialCount);
                    targetPool = GlobalPool[source];
                }
                return targetPool.Call(in position, in rotation);
            }

            return null;
        }

        public static T Call(T source, in Vector3 position)
        {
            return Call(source, in position, Quaternion.identity);
        }

        public static OcPool<T> FindPool(T source)
        {
            if (GlobalPool.ContainsKey(source)) return GlobalPool[source];

            return null;
        }

        public static void DisposeAll()
        {
            PoolDisposer.DisposeAll();
            GlobalPool = new Dictionary<IPoolMember<T>, OcPool<T>>();
        }
        
        public static void Print()
        {
            Debug.Log($"||=========  OcPool<{typeof(T).Name.Rich(Color.cyan)}> DEBUG  ==========".Rich(Color.magenta));
            Debug.Log($"||{"Global Pool".Rich(Color.white)} | Count : {GlobalPool.Count}".Rich(Color.magenta));
            foreach (var ocPool in GlobalPool)
            {
                Debug.Log($"||{($"Pool : {ocPool.Key.gameObject.name.Rich(Color.cyan)}".Rich(Color.white))} | Count : {ocPool.Value.TotalCount} | Folder : {ocPool.Value.Folder.name}".Rich(Color.magenta));    
            }
            Debug.Log("||=========================================".Rich(Color.magenta));
        }
        
        
        // Non-Static. =======

        OcPool(T source, int count, Transform folder)
        {
            _isOverridenFolder = folder != null;
            Folder = folder == null ? new GameObject().transform : folder;

            _source = source;
            GlobalPool[source] = this;
            _sleepingMembers = new Queue<T>();
            _activeMembers = new List<T>();
            InitialCount = count;
            AddMember(count);
            
            PoolDisposer.RegisterPool(this);
        }
        
        public Transform Folder { get; }
        public int TotalCount => _sleepingMembers.Count + _activeMembers.Count;
        public int InitialCount { get; }

        bool _isOverridenFolder;
        T _source;
        Queue<T> _sleepingMembers;
        List<T> _activeMembers;
        
        public void AddMember(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var gao = Object.Instantiate(_source, Folder, true);
                gao.Pool = this;
                _sleepingMembers.Enqueue(gao);
                gao.gameObject.SetActive(false);
            }

            if(!_isOverridenFolder) Folder.name = $"{_source.name}_Pool [{TotalCount:###,###}]";
        }

        public T Call(in Vector3 position, in Quaternion rotation)
        {
            if(_sleepingMembers.Count == 0) AddMember(InitialCount);

            var member = _sleepingMembers.Dequeue();
            member.transform.SetPositionAndRotation(position, rotation);
            member.WakeUp();
            _activeMembers.Add(member);
            return member;
        }

        public T Call(in Vector3 position)
        {
            return Call(in position, Quaternion.identity);
        }

        public T FindActiveMember(Predicate<T> predicate)
        {
            for (int i = 0; i < _activeMembers.Count; i++)
            {
                var m = _activeMembers[i];
                if (predicate.Invoke(m)) return m;
            }

            return null;
        }

        public T FindSleepingMember(Predicate<T> predicate)
        {
            var toArray = _sleepingMembers.ToArray();
            for (int i = 0; i < _activeMembers.Count; i++)
            {
                var m = toArray[i];
                if (predicate.Invoke(m)) return m;
            }

            return null;
        }

        public void Foreach(Action<T> action)
        {
            for (int i = 0; i < _activeMembers.Count; i++)
            {
                var m = _activeMembers[i];
                action.Invoke(m);
            }

            var toArray = _sleepingMembers.ToArray();
            for (int i = 0; i < _sleepingMembers.Count; i++)
            {
                var m = toArray[i];
                action.Invoke(m);
            }
        }

        public void Return(T member)
        {
            _sleepingMembers.Enqueue(member);
            _activeMembers.Remove(member);
        }

        public void Dispose()
        {
            _source = null;
            _sleepingMembers = null;
            _activeMembers = null;
            if(!_isOverridenFolder && Folder != null) Object.Destroy(Folder.gameObject);
        }

        public void DebugType()
        {
            Print();
        }
        public void DebugLog()
        {
            Printer.Print($"OcPool<{typeof(T).Name.Rich(Color.cyan)}> | [{_source.gameObject.name}] | Folder : {Folder.name} \n"
                              .Rich(new Color(1f, 0.5f,1f)) +
                          $"sleep : {_sleepingMembers.Count} | active : {_activeMembers.Count} | total : {_sleepingMembers.Count + _activeMembers.Count}"
                              .Rich(new Color(1f, 0.5f,1f)));
        }

        public Type GetGenericType()
        {
            return typeof(T);
        }
    }
}
