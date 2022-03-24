using System;
using System.Collections.Generic;
using UnityEngine;

namespace OcUtility
{
    public static class PhysicsExtension
    {
        static bool _initialized;
        static Queue<RaycastHit[]> _rayHitBuffer;
        static Queue<Collider[]> _overlapBuffer;
        
        static int raycastBudget;
        static int overlapBudget;
        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            if(_initialized) return;
            _initialized = true;
            
            raycastBudget = OcUtilitySettings.Instance == null ? 32 : OcUtilitySettings.Instance.RaycastBudget;
            overlapBudget = OcUtilitySettings.Instance == null ? 32 : OcUtilitySettings.Instance.OverlapBudget;

            _rayHitBuffer = new Queue<RaycastHit[]>();

            
            for (int i = 0; i < 20; i++)
            {
                _rayHitBuffer.Enqueue(new RaycastHit[raycastBudget]);
            }
            
            _overlapBuffer = new Queue<Collider[]>();
            for (int i = 0; i < 20; i++)
            {
                _overlapBuffer.Enqueue(new Collider[overlapBudget]);
            }

#if UNITY_EDITOR
            Application.quitting += Release;
#endif
        }

#if UNITY_EDITOR
        static void Release()
        {
            _initialized = false;
            _rayHitBuffer = null;
            Application.quitting -= Release;
        }  
#endif

        public static RaycastHit[] GetRaycastHitBuffer()
        {
            if (_rayHitBuffer.Count == 0) return new RaycastHit[raycastBudget];

            return _rayHitBuffer.Dequeue();
        }
        public static Collider[] GetOverlapBuffer()
        {
            if (_overlapBuffer.Count == 0) return new Collider[overlapBudget];

            return _overlapBuffer.Dequeue();
        }

        public static void ReturnBuffer(RaycastHit[] buffer)
        {
            if(buffer.Length < raycastBudget) Array.Resize(ref buffer, raycastBudget);
            _rayHitBuffer.Enqueue(buffer);
        }
        public static void ReturnBuffer(Collider[] buffer)
        {
            if(buffer.Length < overlapBudget) Array.Resize(ref buffer, overlapBudget);
            _overlapBuffer.Enqueue(buffer);
        }

        public static Vector3[] CalcTrajectoryPoints(
            this Rigidbody body,
            Vector3 force, float yLimit, int resolution)
        {
            return CalcTrajectoryPoints(body.position, force, Physics.gravity.y, yLimit, resolution);
        }

        /// <summary>
        /// 중력과 속도에 따른 물체의 이동 궤적을 그림
        /// </summary>
        /// <param name="start">물체의 위치</param>
        /// <param name="force">물체에 가해지는 힘</param>
        /// <param name="gravity">중력(일반적으로 -9.8f)</param>
        /// <param name="yLimit">궤적의 마지막 지점의 y값의 근사치. 0일 경우, 시작지점의 높이와 비슷한 지점까지 궤적이 그려짐.</param>
        /// <param name="resolution">궤적의 해상도</param>
        /// <returns></returns>
        public static Vector3[] CalcTrajectoryPoints(
            Vector3 start, Vector3 force, float gravity,
            float yLimit, int resolution)
        {
            if (resolution < 1) resolution = 1;
            var points = new Vector3[resolution + 2];
            
            var g = -gravity;

            var lowestTimeValue = max_time_y() / (resolution + 2);
            for (int i = 0; i < points.Length; i++)
            {
                var t = lowestTimeValue * i;

                var x = force.x * t;
                var y = (force.y * t) - (g * t.sqr() * 0.5f);
                var z = force.z * t;

                points[i] = start + new Vector3(x, y, z);
            }

            return points;

            float max_time_y()
            {
                var v = force.y;
                var vv = v.sqr();

                var t = (v + Mathf.Sqrt(vv + 2 * g * -yLimit)) / g;
                return t;
            }
        }
    }
}