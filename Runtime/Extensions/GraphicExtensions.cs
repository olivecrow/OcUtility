using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OcUtility
{
    public static class GraphicExtensions
    {
        public static Bounds CalcLocalRendererBounds(this GameObject source)
        {
            var bounds = new Bounds();
            var boundsAssigned = false;
            
            foreach (var renderer in source.GetComponentsInChildren<Renderer>())
            {
                if (boundsAssigned)
                {
                    var childBound = renderer.bounds;
                    childBound.center = source.transform.InverseTransformPoint(childBound.center);
                    childBound.size = source.transform.InverseTransformVector(childBound.size);
                    bounds.Encapsulate(childBound);
                }
                else
                {
                    bounds = renderer.localBounds;
                    boundsAssigned = true;
                }
            }

            return bounds;
        }
        public static Bounds CalcRendererBounds(this Renderer[] renderers)
        {
            var bounds = new Bounds();
            var boundsAssigned = false;
            
            foreach (var renderer in renderers)
            {
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
        public static Bounds CalcRendererBounds(this List<Renderer> renderers)
        {
            var bounds = new Bounds();
            var boundsAssigned = false;
            
            foreach (var renderer in renderers)
            {
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
        public static Bounds CalcRendererBounds(this LODGroup source)
        {
            var bounds = new Bounds();
            var boundsAssigned = false;

            foreach (var lod in source.GetLODs())
            {
                foreach (var renderer in lod.renderers)
                {
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