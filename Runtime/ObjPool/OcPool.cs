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

        public static OcPool<T> MakePool(T source, int count, Transform folder = null, Action<T> initializer = null)
        {
            OcPool<T> targetPool;
            if (GlobalPool.ContainsKey(source))
            {
                targetPool = GlobalPool[source];
                targetPool.AddMember(count);
                if (targetPool.MaxInitialCount < count) targetPool.MaxInitialCount = count;
                targetPool._numberOfMakeRequest++;
            }
            else targetPool = new OcPool<T>(source, count, folder, initializer);

            return targetPool;
        }
        public static OcPool<T> MakePool(T source, int count, Action<T> initializer)
        {
            return MakePool(source, count, null, initializer);
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
                    GlobalPool[source] = MakePool(source, targetPool.MaxInitialCount);
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

        OcPool(T source, int count, Transform folder, Action<T> initializer)
        {
            _isOverridenFolder = folder != null;
            Folder = folder == null ? new GameObject().transform : folder;

            _source = source;
            GlobalPool[source] = this;
            _sleepingMembers = new Queue<T>();
            _activeMembers = new List<T>();
            MaxInitialCount = count;
            _initializer = initializer;
            _numberOfMakeRequest++;
            AddMember(count, true);
            
            PoolDisposer.RegisterPool(this);
        }
        
        public Transform Folder { get; }
        public int TotalCount => _sleepingMembers.Count + _activeMembers.Count;
        public int MaxInitialCount { get; private set; }

        readonly bool _isOverridenFolder;
        int _numberOfMakeRequest;
        T _source;
        Queue<T> _sleepingMembers;
        List<T> _activeMembers;
        Action<T> _initializer;
        void AddMember(int count, bool forceAdd = false)
        {
            if (!forceAdd)
            {
                if (_numberOfMakeRequest < 5) goto ADD;
                if(_sleepingMembers.Count > count * 2) return;
                if(MaxInitialCount > count  * 5) return;
            }
            ADD:
            for (int i = 0; i < count; i++)
            {
                var gao = Object.Instantiate(_source, Folder, true);
                gao.Pool = this;
                _sleepingMembers.Enqueue(gao);
                gao.gameObject.SetActive(false);
                _initializer?.Invoke(gao);
            }

            if(!_isOverridenFolder) Folder.name = $"{_source.name}_Pool [{TotalCount:###,###}]";
        }

        public T Call(in Vector3 position, in Quaternion rotation)
        {
            if(_sleepingMembers.Count == 0) AddMember(MaxInitialCount, true);

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
            var allList = new List<T>(_activeMembers);
            allList.AddRange(_sleepingMembers);

            for (int i = 0; i < allList.Count; i++)
            {
                action.Invoke(allList[i]);
            }
        }
        
        public T FindMaxMember(Func<T, float> func)
        {
            var allList = new List<T>(_activeMembers);
            allList.AddRange(_sleepingMembers);
            return allList.GetMaxElement(func);
        }
        public T FindMinMember(Func<T, float> func)
        {
            var allList = new List<T>(_activeMembers);
            allList.AddRange(_sleepingMembers);
            return allList.GetMinElement(func);
        }
        public T FindMaxSleepingMember(Func<T, float> func)
        {
            return new List<T>(_sleepingMembers).GetMaxElement(func);
        }
        public T FindMinSleepingMember(Func<T, float> func)
        {
            return new List<T>(_sleepingMembers).GetMaxElement(func);
        }
        public T FindMaxActiveMember(Func<T, float> func)
        {
            return _activeMembers.GetMaxElement(func);
        }
        public T FindMinActiveMember(Func<T, float> func)
        {
            return _activeMembers.GetMaxElement(func);
        }

        public void SetInitializer(Action<T> action, bool applyNow)
        {
            _initializer = action;
            if(!applyNow) return;
            ApplyInitializer();
        }

        public void AppendInitializer(Action<T> action, bool applyNow)
        {
            _initializer += action;
            if(!applyNow) return;
            ApplyInitializer();
        }

        void ApplyInitializer()
        {
            var allMember = new List<T>(_sleepingMembers);
            allMember.AddRange(_activeMembers);
            foreach (var member in allMember) _initializer?.Invoke(member);
        }

        public void SleepAll()
        {
            var count = _activeMembers.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                _activeMembers[i].Sleep();
            }
        }

        public void Return(T member)
        {
            _sleepingMembers.Enqueue(member);
            _activeMembers.Remove(member);
        }

        public void Dispose()
        {
            GlobalPool.Remove(_source);
            PoolDisposer.UnRegisterPool(this);
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
