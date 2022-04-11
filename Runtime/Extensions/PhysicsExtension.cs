using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
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
            if (_initialized) return;
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
            if (buffer.Length < raycastBudget) Array.Resize(ref buffer, raycastBudget);
            _rayHitBuffer.Enqueue(buffer);
        }

        public static void ReturnBuffer(Collider[] buffer)
        {
            if (buffer.Length < overlapBudget) Array.Resize(ref buffer, overlapBudget);
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


        static float ScaledCapsuleRadius(CapsuleCollider capsule)
        {
            var scale = capsule.transform.lossyScale;
            float max;
            switch (capsule.direction)
            {
                case 0: // X-Axis.
                    max = scale.y > scale.z ? scale.y : scale.z;
                    break;
                case 1: // Y-Axis.
                    max = scale.x > scale.z ? scale.x : scale.z;
                    break;
                case 2: // Z-Axis.
                    max = scale.x > scale.y ? scale.x : scale.y;
                    break;
                default: goto case 1;
            }

            return max * capsule.radius;
        }

        static float ScaledCapsuleHeight(CapsuleCollider capsule)
        {
            var scale = capsule.transform.lossyScale;
            float height;
            switch (capsule.direction)
            {
                case 0: // X-Axis.
                    height = capsule.height * scale.x;
                    break;
                case 1: // Y-Axis.
                    height = capsule.height * scale.y;
                    break;
                case 2: // Z-Axis.
                    height = capsule.height * scale.z;
                    break;
                default: goto case 1;
            }

            var scaledRadius = ScaledCapsuleRadius(capsule);
            if (height < scaledRadius * 2f) height = scaledRadius * 2f;
            return height;
        }

        public static void ToWorldSpaceCapsule(this CapsuleCollider capsule, out Vector3 point0, out Vector3 point1,
            out float radius)
        {
            var center = capsule.transform.TransformPoint(capsule.center);
            radius = 0f;
            float height = 0f;
            Vector3 lossyScale = capsule.transform.lossyScale.abs();
            Vector3 dir = Vector3.zero;

            switch (capsule.direction)
            {
                case 0: // x
                    radius = Mathf.Max(lossyScale.y, lossyScale.z) * capsule.radius;
                    height = lossyScale.x * capsule.height;
                    dir = capsule.transform.TransformDirection(Vector3.right);
                    break;
                case 1: // y
                    radius = Mathf.Max(lossyScale.x, lossyScale.z) * capsule.radius;
                    height = lossyScale.y * capsule.height;
                    dir = capsule.transform.TransformDirection(Vector3.up);
                    break;
                case 2: // z
                    radius = Mathf.Max(lossyScale.x, lossyScale.y) * capsule.radius;
                    height = lossyScale.z * capsule.height;
                    dir = capsule.transform.TransformDirection(Vector3.forward);
                    break;
            }

            if (height < radius * 2f)
            {
                dir = Vector3.zero;
            }

            point0 = center + dir * (height * 0.5f - radius);
            point1 = center - dir * (height * 0.5f - radius);
        }

        public static Vector3 DirectionAxis(this CapsuleCollider capsule)
        {
            return capsule.direction switch
            {
                0 => Vector3.right,
                1 => Vector3.up,
                2 => Vector3.forward,
                _ => Vector3.up
            };
        }

#if UNITY_EDITOR
        [MenuItem("CONTEXT/BoxCollider/바운드에 맞게 확장")]
        static void ExtendBox(MenuCommand command)
        {
            var c = command.context as BoxCollider;
            Undo.RecordObject(c, "바운드에 맞게 확장");
            var childRenderer = c.GetComponentsInChildren<Renderer>();

            var bounds = new Bounds(c.transform.position, Vector3.zero);
            foreach (var renderer in childRenderer)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            c.center = bounds.center - c.transform.position;
            c.size = bounds.size;
        }
        [MenuItem("CONTEXT/CapsuleCollider/바운드에 맞게 확장")]
        static void ExtendCapsule(MenuCommand command)
        {
            var c = command.context as CapsuleCollider;
            Undo.RecordObject(c, "바운드에 맞게 확장");
            var childRenderer = c.GetComponentsInChildren<Renderer>();

            var bounds = new Bounds(c.transform.position, Vector3.zero);
            foreach (var renderer in childRenderer)
            {
                bounds.Encapsulate(renderer.bounds);
            }
            
            c.center = bounds.center - c.transform.position;
            var xyz = new float[]{ bounds.extents.x, bounds.extents.y, bounds.extents.z };
            xyz = xyz.OrderByDescending(x=> x).ToArray();

            c.radius = MathExtension.CalcHypotenuseOfRightAngledTriangle(xyz[0], xyz[1]);
            c.height = bounds.size.y + c.radius;

            Debug.LogWarning($"캡슐 콜라이더 및 구체 콜라이더의 확장은 부정확할 수 있음");
        }
        
        [MenuItem("CONTEXT/SphereCollider/바운드에 맞게 확장")]
        static void ExtendSphere(MenuCommand command)
        {
            var c = command.context as SphereCollider;
            Undo.RecordObject(c, "바운드에 맞게 확장");
            var childRenderer = c.GetComponentsInChildren<Renderer>();

            var bounds = new Bounds(c.transform.position, Vector3.zero);
            foreach (var renderer in childRenderer)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            c.center = bounds.center - c.transform.position;

            var xyz = new float[]{ bounds.extents.x, bounds.extents.y, bounds.extents.z };
            xyz = xyz.OrderByDescending(x=> x).ToArray();

            c.radius = MathExtension.CalcHypotenuseOfRightAngledTriangle(xyz[0], xyz[1]);
            Debug.LogWarning($"캡슐 콜라이더 및 구체 콜라이더의 확장은 부정확할 수 있음");
        }
#endif
    }
}