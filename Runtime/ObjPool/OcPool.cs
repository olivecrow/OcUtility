using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcUtility
{
    public class OcPool : IDisposable
    {
        static Dictionary<PoolMember, OcPool> GlobalPool;
        
        static OcPool()
        {
#if UNITY_EDITOR
            Application.quitting += () => GlobalPool = new Dictionary<PoolMember, OcPool>();      
#endif
            GlobalPool = new Dictionary<PoolMember, OcPool>();
        }

        public static OcPool MakePool(PoolMember source, int count)
        {
            OcPool targetPool;
            if (GlobalPool.ContainsKey(source))
            {
                targetPool = GlobalPool[source];
                targetPool.AddMember(count);
            }
            else targetPool = new OcPool(source, count);

            return targetPool;
        }

        /// <summary> 글로벌 풀에서 해당 소스 기반의 풀에서 Call을 반환함. 해당 소스의 풀이 없을 경우 null을 반환함. </summary>
        public static PoolMember Call(PoolMember source, in Vector3 position, in Quaternion rotation)
        {
            if (GlobalPool.ContainsKey(source))
            {
                var targetPool = GlobalPool[source];
                return targetPool.Call(in position, in rotation);
            }

            return null;
        }

        public OcPool FindPool(PoolMember source)
        {
            if (GlobalPool.ContainsKey(source)) return GlobalPool[source];

            return null;
        }

        public OcPool(PoolMember source, int count)
        {
            _folder = new GameObject().transform;
            _source = source;
            GlobalPool[source] = this;
            _sleepingMembers = new Queue<PoolMember>();
            _allMembers = new List<PoolMember>();
            _initialCount = count;
            AddMember(count, true);
        }
        
        
        // Non-Static.
        readonly Transform _folder;
        PoolMember _source;
        Queue<PoolMember> _sleepingMembers;
        List<PoolMember> _allMembers;
        readonly int _initialCount;
        bool HasEnoughMember(int count)
        {
            return _allMembers.Count * 0.1f > count && _sleepingMembers.Count * 0.5f  > count;
        }
        /// <summary> 오브젝트 풀에 멤버를 추가함. 기본적으로 전체 풀의 10%미만, 대기 풀의 50% 미만이면 추가하지 않고 넘어감. </summary>
        public void AddMember(int count, bool forceToAdd = false)
        {
            if(!forceToAdd && HasEnoughMember(count)) return;
            for (int i = 0; i < count; i++)
            {
                var gao = Object.Instantiate(_source, _folder, true);
                gao.Pool = this;
                _sleepingMembers.Enqueue(gao);
                _allMembers.Add(gao);
                gao.gameObject.SetActive(false);
            }

            _folder.name = $"{_source.name}_Pool [{_allMembers.Count:###,###}]";
        }

        public PoolMember Call(in Vector3 position, in Quaternion rotation)
        {
            if(_sleepingMembers.Count == 0) AddMember(_initialCount);

            var member = _sleepingMembers.Dequeue();
            member.transform.SetPositionAndRotation(position, rotation);
            member.WakeUp();
            return member;
        }

        public void Return(PoolMember member)
        {
            _sleepingMembers.Enqueue(member);
        }

        public void Dispose()
        {
            _source = null;
            _sleepingMembers = null;
            var count = _allMembers.Count;
            for (int i = 0; i < count; i++)
            {
                Object.Destroy(_allMembers[0]);
            }

            _allMembers = null;
        }
    }
}
