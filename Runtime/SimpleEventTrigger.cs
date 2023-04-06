using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public class SimpleEventTrigger : MonoBehaviour, IHierarchyIconDrawable
{
    public Object IconTarget => this;
    public Texture2D OverrideIcon { get; }
    public Color IconTint => Color.white;
    
    public bool useMultipleEvent;
    [HideIf("useMultipleEvent")] public EventTiming timing;
    [HideIf("useMultipleEvent")] public float delayTime;
    [HideIf("useMultipleEvent")] public UnityEvent e;
    [ShowIf("useMultipleEvent"), TableList]public EventKeyPair[] events;

    public Action onEnable;
    public Action onStart;
    public Action onDisable;
    public Action<Collider> onTriggerEnter;

    void OnValidate()
    {
        if (delayTime < 0) delayTime = 0;
        if(events == null) return;
        foreach (var eventKeyPair in events)
        {
            if (eventKeyPair.delayTime < 0) eventKeyPair.delayTime = 0;
        }
    }

    void Awake()
    {
        InvokeByTiming(EventTiming.Awake);
    }

    void OnEnable()
    {
        InvokeByTiming(EventTiming.OnEnable);
        onEnable?.Invoke();
    }

    void OnDisable()
    {
        InvokeByTiming(EventTiming.OnDisable);
        onDisable?.Invoke();
    }

    void Start()
    {
        InvokeByTiming(EventTiming.Start);
        onStart?.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        InvokeByTiming(EventTiming.TriggerEnter);
        onTriggerEnter?.Invoke(other);
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
        if (delayTime > 0) wait.time(delayTime, e.Invoke);
        else e.Invoke();
    }
        
    public void InvokeByKey(string key)
    {
        var pair = events.FirstOrDefault(x => string.CompareOrdinal(x.key, key) == 0);
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
        [TableColumnWidth(200, false)]
        [VerticalGroup("Key")]public EventTiming timing;
        [VerticalGroup("Key")]public string key;
        [VerticalGroup("Key")] public float delayTime;
        [VerticalGroup("E")]public UnityEvent e;

        [VerticalGroup("Key"), Button]
        public void Invoke()
        {
            if (delayTime > 0) wait.time(delayTime, e.Invoke);
            else e.Invoke();
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