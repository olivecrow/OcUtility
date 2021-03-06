#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OcUtility
{
    public class DistanceCalculator : MonoBehaviour
    {
        public List<Transform> points;

        void Reset()
        {
            points = new List<Transform>();
            points.Add(transform);
        }

        void OnDestroy()
        {
            ClearPoints();
        }

        [Button]
        public void ClearPoints()
        {
            if (points == null || points.Count == 0) return;
            foreach (var t in points)
            {
                if(t == transform) continue;
                DestroyImmediate(t.gameObject);
            }

            points = null;
        }
    }
}
#endif