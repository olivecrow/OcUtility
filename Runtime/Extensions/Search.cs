#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcUtility
{
    public static class Search
    {
        public static void Load<T>(string text, Action<List<T>> complete) where T : Object
        {
            Debug.Log($"{text.Rich(new Color(0.8f,0.5f,0.2f))} | type : {typeof(T)} 검색 시작...");
            SearchService.Request(text, on_search_complete);
            
            void on_search_complete(SearchContext context, IList<SearchItem> items)
            {
                Debug.Log($"검색된 오브젝트 | 개수 : {items.Count}\n{string.Join('\n', items)}");
                var list = new List<T>();
                foreach (var item in items)
                {
                    list.Add(LoadFromItem<T>(item));
                }
                complete?.Invoke(list);
            }
        }


        public static T LoadFromItem<T>(this SearchItem item) where T : Object
        {
            if (GlobalObjectId.TryParse(item.id, out var id))
            {
                return GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id) as T;;
            }

            return null;
        }
        
    }
}
#endif