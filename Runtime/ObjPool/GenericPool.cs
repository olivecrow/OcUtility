using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcUtility
{
    public class GenericPool<T> : IPool where T : class
    {
        public static GenericPool<T> MakePool (T source, int count, Func<T> create, Action<T> sleep,
            Action<T> initializer = null)
        {
            if(!PoolManager.Initialized) PoolManager.Init();
            GenericPool<T> targetPool;
            if (PoolManager.HasPool(source.GetHashCode()))
            {
                targetPool = PoolManager.GetPool(source.GetHashCode()) as GenericPool<T>;
                if(targetPool._initialCount < count)targetPool.AddMember(count);
            }
            else targetPool = new GenericPool<T>(source, count, create, sleep, initializer);

            return targetPool;
        }
        
        public int TotalCount { get; private set; }
        public int ActiveCount => _activeMembers.Count;
        public int SleepCount => _sleepMembers.Count;
        public object Source => _source;
        public string SourceName => _source.ToString();
        public Transform Folder => null;
        
        T _source;
        int _initialCount;
        Queue<T> _sleepMembers;
        List<T> _activeMembers;
        Func<T> _create;
        Action<T> _sleep;
        Action<T> _initializer;
        bool _sourceAssignedToMember;
        
        GenericPool(T source, int count, Func<T> create, Action<T> sleep,
            Action<T> initializer = null)
        {
            _source = source;
            _initialCount = count;
            _create = create;
            _sleep = sleep;
            _initializer = initializer;
            
            _sleepMembers = new Queue<T>();
            _activeMembers = new List<T>();
            
            AddMember(count);
            
            PoolManager.RegisterPool(source.GetHashCode(),this);
        }

        public void MakeSourceAsMember()
        {
            if(_sourceAssignedToMember) return;
            _sleep?.Invoke(_source);
            _sleepMembers.Enqueue(_source);
        }

        public void AddMember(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var member = _create?.Invoke();
                _initializer?.Invoke(member);
                _sleepMembers.Enqueue(member);
                TotalCount++;
            }
        }

        public T Call()
        {
            if (_sleepMembers.Count == 0)
            {
                AddMember(_initialCount);
            }

            var member = _sleepMembers.Dequeue();
            _activeMembers.Add(member);
            return member;
        }

        public void Return(T member)
        {
            if(_activeMembers.Remove(member))
            {
                _sleep?.Invoke(member);
                _sleepMembers.Enqueue(member);
            }
            else Debug.LogWarning($"[GenericPool<{typeof(T).Name}>] 멤버가 아닌 인스턴스는 반환 할 수 없음"); 
        }
        
        public Type GetPoolMemberType()
        {
            return typeof(T);
        }

        public IEnumerable<object> GetAllMembers()
        {
            var list = new List<T>(_sleepMembers);
            list.AddRange(_activeMembers);
            return list;
        }

        public IEnumerable<object> GetActiveMembers()
        {
            return _activeMembers;
        }

        public IEnumerable<object> GetSleepMembers()
        {
            return _sleepMembers;
        }

        public void Dispose()
        {
            PoolManager.UnRegisterPool(_source.GetHashCode());
            _source = null;
            _activeMembers = null;
            _sleepMembers = null;

            _create = null;
            _initializer = null;
        }
    }
}