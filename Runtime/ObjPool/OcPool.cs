using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace OcUtility
{
    public class OcPool<T> : IDisposable, IPool where T : MonoBehaviour, IPoolMember<T>
    {
        public static OcPool<T> MakePool(T source, int count, Transform folder = null, Action<T> initializer = null)
        {
            if(!PoolManager.Initialized) PoolManager.Init();
            OcPool<T> targetPool;
            if (PoolManager.HasPool(source.GetHashCode()))
            {
                targetPool = PoolManager.GetPool(source.GetHashCode()) as OcPool<T>;
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
        public static T Call(T source, in Vector3 position, in Quaternion rotation, Action<T> beforeWakeUp = null)
        {
            var targetPool = GetPoolInternal(source);
            return targetPool?.Call(position, rotation, beforeWakeUp);
        }

        public static T Call(T source, in Vector3 position, Action<T> beforeWakeUp = null)
        {
            return Call(source, in position, Quaternion.identity, beforeWakeUp);
        }

        public static T Call(T source, Action<T> beforeWakeUp = null)
        {
            var targetPool = GetPoolInternal(source);
            return targetPool?.Call(beforeWakeUp);

        }

        static OcPool<T> GetPoolInternal(T source)
        {
            var key = source.GetHashCode();
            if (!PoolManager.HasPool(key)) return null;

            var targetPool = PoolManager.GetPool(key) as OcPool<T>;
                
            // 씬이 변경되는 등의 이유로 참조가 Missing 상태일 경우 다시 풀을 재생성함.
            if (targetPool.Folder == null)
            {
                targetPool.Dispose();
                targetPool = MakePool(source, targetPool.MaxInitialCount);
                PoolManager.RegisterPool(key, targetPool);
            }

            return targetPool;
        }

        public static OcPool<T> FindPool(T source)
        {
            var key = source.GetHashCode();
            if (PoolManager.HasPool(key)) return PoolManager.GetPool(key) as OcPool<T>;

            return null;
        }

        public static void DisposeAll()
        {
            PoolManager.DisposeAll();
        }

        //======================================================
        //=================== Non-Static. ======================
        //======================================================

        public Transform Folder { get; }
        public int TotalCount => _sleepingMembers.Count + _activeMembers.Count;
        public int ActiveCount => _activeMembers.Count;
        public int SleepCount => _sleepingMembers.Count;
        public int MaxInitialCount { get; private set; }
        public object Source => _source;
        public string SourceName => _source.name;

        readonly bool _isOverridenFolder;
        int _numberOfMakeRequest;
        T _source;
        Queue<T> _sleepingMembers;
        List<T> _activeMembers;
        Action<T> _initializer;
        
        OcPool(T source, int count, Transform folder, Action<T> initializer)
        {
            _isOverridenFolder = folder != null;
            Folder = folder == null ? new GameObject().transform : folder;

            _source = source;
            _sleepingMembers = new Queue<T>();
            _activeMembers = new List<T>();
            MaxInitialCount = count;
            _initializer = initializer;
            _numberOfMakeRequest++;
            AddMember(count, true);
            
            PoolManager.RegisterPool(source.GetHashCode(), this);
        }
        
        
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

            if(!_isOverridenFolder) Folder.name = $"[{_source.GetType().Name}] {_source.name}_Pool [{TotalCount:###,###}]";
        }

        public T Call(in Vector3 position, in Quaternion rotation, Action<T> beforeWakeUp = null)
        {
            var member = CallInternal(beforeWakeUp);
            if (member == null) return null;
            member.transform.SetPositionAndRotation(position, rotation);
            return member;
        }

        public T Call(in Vector3 position, Action<T> beforeWakeUp = null)
        {
            return Call(in position, Quaternion.identity, beforeWakeUp);
        }

        public T Call(Action<T> beforeWakeUp = null)
        {
            return CallInternal(beforeWakeUp);
        }

        T CallInternal(Action<T> beforeWakeUp)
        {
            if(_sleepingMembers.Count == 0) AddMember(MaxInitialCount, true);

            var member = _sleepingMembers.Dequeue();
            beforeWakeUp?.Invoke(member);
            member.WakeUp();
            _activeMembers.Add(member);
            return member;
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
            if(!PoolManager.Initialized) return;
            PoolManager.UnRegisterPool(_source.GetHashCode());
            _source = null;
            _sleepingMembers = null;
            _activeMembers = null;
            if(!_isOverridenFolder && Folder != null) Object.Destroy(Folder.gameObject);
        }

        public Type GetPoolMemberType()
        {
            return typeof(T);
        }

        public IEnumerable<object> GetAllMembers()
        {
            var list = new List<T>(_sleepingMembers);
            list.AddRange(_activeMembers);
            return list;
        }
        public IEnumerable<object> GetSleepMembers()
        {
            return _sleepingMembers;
        }
        public IEnumerable<object> GetActiveMembers()
        {
            return _activeMembers;
        }
    }
}
