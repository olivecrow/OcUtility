using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class SimpleEventTrigger : MonoBehaviour
{
    public bool useMultipleEvent;
    [HideIf("useMultipleEvent")] public EventTiming timing;
    [HideIf("useMultipleEvent")] public UnityEvent e;
    [ShowIf("useMultipleEvent"), TableList]public EventKeyPair[] events;

    void Awake()
    {
        InvokeByTiming(EventTiming.Awake);
    }

    void OnEnable()
    {
        InvokeByTiming(EventTiming.OnEnable);
    }

    void OnDisable()
    {
        InvokeByTiming(EventTiming.OnDisable);
    }

    void Start()
    {
        InvokeByTiming(EventTiming.Start);
    }

    void OnTriggerEnter(Collider other)
    {
        InvokeByTiming(EventTiming.TriggerEnter);
    }

    void InvokeByTiming(EventTiming t)
    {
        if (useMultipleEvent)
        {
            foreach (var pair in events)
            {
                if(pair.timing == t) pair.Invoke();
            }
        }
        else
        {
            if(timing == t) Invoke();
        }
    }

    [HideIf("useMultipleEvent"), Button]
    public void Invoke()
    {
        e.Invoke();
    }
        
    public void InvokeByKey(string key)
    {
        var pair = events.FirstOrDefault(x => x.key == key);
        if (pair == null)
        {
            Debug.Log($"해당 Key값의 이벤트가 없음 | key : {key}");
            return;
        }
            
        pair.Invoke();
    }
        
    public void InvokeByIndex(int index)
    {
        if (index > events.Length - 1)
        {
            Debug.Log($"유효하지 않은 인덱스 | index : {index}");
            return;
        }
            
        events[index].Invoke();
    }

    [Serializable]
    public class EventKeyPair
    {
        public EventTiming timing;
        [TableColumnWidth(100, false)]public string key;
        [VerticalGroup("E")]public UnityEvent e;

        [VerticalGroup("E"), Button]
        public void Invoke()
        {
            e.Invoke();
        }
    }

    public enum EventTiming
    {
        Manual,
        Awake,
        Start,
        OnEnable,
        OnDisable,
        TriggerEnter
    }
}