#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcUtility
{
    [ExecuteInEditMode]
    public class HiddenObjectFinder : MonoBehaviour
    {
        public enum SearchRange
        {
            ThisGameObject,
            Children,
            All,
            Specific
        }

        [EnumToggleButtons][LabelWidth(120)]
        public SearchRange searchRange;
        [EnumToggleButtons][LabelWidth(120)][Space]
        public HideFlags searchFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.HideAndDontSave;
        
        [ShowIf(nameof(searchRange), SearchRange.Specific)]public Transform[] specificSearchRoots;
        
        public List<SearchResult> SearchResults = new List<SearchResult>();
        [ShowInInspector] public bool isDirty => SearchResults.Any(x => x.isDirty);

        void Reset()
        {
            Undo();
            SearchResults.Clear();
            searchFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.HideAndDontSave;
        }

        [Button]
        void Search()
        {
            if (SearchResults.Count > 0 && isDirty)
            {
                Printer.Print($"오브젝트의 HideFlags가 변경된 상태에서는 재검색을 할 수 없음");
                return;
            }
            switch (searchRange)
            {
                case SearchRange.ThisGameObject:
                    SearchThisGameObject();
                    break;
                case SearchRange.Children:
                    SearchChildren();
                    break;
                case SearchRange.All:
                    SearchAll();                    
                    break;
                case SearchRange.Specific:
                    SearchSpecific();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Button]
        void SetFlagNoneAll()
        {
            foreach (var result in SearchResults)
            {
                result.SetFlagNone();
            }
        }

        [Button]
        void Undo()
        {
            foreach (var result in SearchResults)
            {
                result.Reset();
            }
        }

        void OnDestroy()
        {
            if(SearchResults.Count > 0 && isDirty)Undo();
            Printer.Print($"변경사항이 되돌려지지 않은 채 종료되어서 강제로 원래대로 되돌림", LogType.Warning);
        }
        void SearchThisGameObject()
        {
            IEnumerable<GameObject> result = GetComponents<Transform>().Where(x => x.hideFlags.HasOneOf(searchFlags)).Select(x => x.gameObject);
            foreach (var o in result)
            {
                SearchResults.Add(new SearchResult(o));
            }
            Printer.Print($"전체 검색 | 결과 => {result.Count()}개 발견");
        }
        void SearchAll()
        {
            SearchResults.Clear();
            var result = Resources.
                FindObjectsOfTypeAll<GameObject>()
                .Where(x =>
                {
                    if (!x.scene.IsValid()) return false;
                    return x.hideFlags.HasOneOf(searchFlags);
                });
            
            foreach (var o in result)
            {
                SearchResults.Add(new SearchResult(o));
            }
            Printer.Print($"전체 검색 | 결과 => {result.Count()}개 발견");
        }
        
        void SearchChildren()
        {
            var result = SearchChildrenOf(transform);
            
            foreach (var o in result)
            {
                SearchResults.Add(new SearchResult(o));
            }
            Printer.Print($"하위 오브젝트 검색 | 결과 => {result.Count()}개 발견");
        }

        IEnumerable<GameObject> SearchChildrenOf(Transform root)
        {
            var result = Resources.
                FindObjectsOfTypeAll<GameObject>()
                .Where(x =>
                {
                    if(!x.transform.IsChildOf(root))return false;
                    return x.hideFlags.HasOneOf(searchFlags);
                });

            return result;
        }

        void SearchSpecific()
        {
            var list = new List<GameObject>();
            foreach (var root in specificSearchRoots)
            {
                if(root == null) continue;
                var result = SearchChildrenOf(root);
                list.AddRange(result);
            }
            
            
            foreach (var o in list)
            {
                SearchResults.Add(new SearchResult(o));
            }
            Printer.Print($"특정 위치 검색 | 결과 => {list.Count()}개 발견");
        }

        [Serializable]
        public class SearchResult
        {
            [ReadOnly]public GameObject target;

            [ShowInInspector]
            public HideFlags hideFlags
            {
                get => target. hideFlags;
                set => target.hideFlags = value;
            }
            [ReadOnly]public HideFlags originalValue;
            public bool isDirty => originalValue != hideFlags;

            public SearchResult(GameObject o)
            {
                target = o;
                originalValue = o.hideFlags;
            }

            public void SetFlagNone()
            {
                target.hideFlags = HideFlags.None;
            }

            public void Reset()
            {
                target.hideFlags = originalValue;
            }
        }
    }
}
#endif