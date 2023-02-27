using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OcUtility
{
    public static class GraphicExtensions
    {
        /// <summary>
        /// 위치, 회전, 스케일이 적용되지 않은 렌더러 바운드. 콜라이더 확장 등을 할때처럼 로컬 바운드의 크기가 필요할때 사용.
        /// 이 값은 정확하지 않으며, 근사치를 구함.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Bounds LocalRendererBounds(this GameObject source)
        {
            var bounds = new Bounds();
            var boundsAssigned = false;
            foreach (var renderer in source.GetComponentsInChildren<Renderer>())
            {
                var worldB = renderer.bounds;

                var localB = renderer.localBounds;
                localB.center += renderer.transform.localPosition;
                
                var scaleMultiplier = (renderer.transform.localRotation * renderer.transform.localScale).abs();
                var transformedScale = localB.size.Multiply(scaleMultiplier);
                var targetScale = transformedScale;
                targetScale.x = Mathf.Max(targetScale.x, worldB.size.x);
                targetScale.y = Mathf.Max(targetScale.y, worldB.size.y);
                targetScale.z = Mathf.Max(targetScale.z, worldB.size.z);

                localB.size = targetScale;
            
                if (boundsAssigned)
                {
                    bounds.Encapsulate(localB);
                }
                else
                {
                    bounds = localB;
                    boundsAssigned = true;
                }   
            }
            
            return bounds;
        }
        public static Bounds RendererBounds(this Renderer[] renderers)
        {
            var bounds = new Bounds();
            var boundsAssigned = false;
            for (int i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (boundsAssigned)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
                else
                {
                    bounds = renderer.bounds;
                    boundsAssigned = true;
                }   
            }

            return bounds;
        }
        public static Bounds RendererBounds(this List<Renderer> renderers)
        {
            var bounds = new Bounds();
            var boundsAssigned = false;
            for (int i = 0; i < renderers.Count; i++)
            {
                var renderer = renderers[i];
                if (boundsAssigned)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
                else
                {
                    bounds = renderer.bounds;
                    boundsAssigned = true;
                }   
            }

            return bounds;
        }
        public static Bounds RendererBounds(this LODGroup source)
        {
            var bounds = new Bounds();
            var boundsAssigned = false;

            var lods = source.GetLODs();
            for (int i = 0; i < lods.Length; i++)
            {
                var lod = lods[i];
                for (int j = 0; j < lod.renderers.Length; j++)
                {
                    var renderer = lod.renderers[i];
                    if (boundsAssigned)
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }
                    else
                    {
                        bounds = renderer.bounds;
                        boundsAssigned = true;
                    }
                }
            }

            return bounds;
        }

        public static Bounds RendererBounds(this GameObject GO)
        {
            return RendererBounds(GO.GetComponentsInChildren<Renderer>());
        }


        public static void QueryLOD(this LODGroup lodGroup)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(lodGroup);
#endif
            var renderers = lodGroup.GetComponentsInChildren<Renderer>().ToList();
            var rendererDict = new Dictionary<int, List<Renderer>>();
            var count = renderers.Count;
            for (int i = 0; i < count; i++)
            {
                var name = renderers[0].name;
                if (!name.Contains("_LOD"))
                {
                    Debug.LogWarning($"_LOD 접미사가 없는 오브젝트 : {name}");
                    add_to_lod(0, renderers[0]);
                    continue;
                }

                if (int.TryParse(name[^1].ToString(), out var level))
                {
                    add_to_lod(level, renderers[0]);
                    continue;
                }
                Debug.LogWarning($"LOD를 파악할 수 없는 오브젝트 : {name}");
                add_to_lod(0, renderers[0]);
            }

            var lods = new List<LOD>();

            var index = 0;
            foreach (var kv in rendererDict.OrderByDescending(x => x.Key))
            {
                var lod = new LOD();
                lod.renderers = kv.Value.ToArray();
                lod.screenRelativeTransitionHeight = 0.01f + (Mathf.Pow(index, 2)).Remap(0, 25, 0, 0.9f);
                lods.Add(lod);
                index++;
            }

            lods.Reverse();
            lodGroup.SetLODs(lods.ToArray());
            lodGroup.RecalculateBounds();

            void add_to_lod(int i, Renderer ren)
            {
                renderers.Remove(ren);
                if (!rendererDict.ContainsKey(i)) rendererDict[i] = new List<Renderer>();
                rendererDict[i].Add(ren);
            }
        }
    }
}