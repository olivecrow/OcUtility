using System;
using System.Collections;

#if UNITY_EDITOR
using System.Reflection;
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine;

public class wait : MonoBehaviour
{
    public static wait Instance => _instance;
    static wait _instance;
    static readonly WaitForFixedUpdate WAIT_FOR_FIXEDUPDATE = new WaitForFixedUpdate();
    static readonly WaitForEndOfFrame WAIT_FOR_ENDOFFRAME = new WaitForEndOfFrame();
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        _instance = new GameObject("wait instance").AddComponent<wait>();
        
        DontDestroyOnLoad(_instance);
    }

    public static Coroutine frame(int frame, Action e)
    {
        return _instance.StartCoroutine(WaitFrame(frame, e));
    }
    public static Coroutine endOfFrame(int frame, Action e)
    {
        return _instance.StartCoroutine(WaitEndOfFrame(frame, e));
    }

    public static Coroutine fixedFrame(int frame, Action e)
    {
        return _instance.StartCoroutine(WaitFixedFrame(frame, e));
    }
    public static Coroutine time(float sec, Action e, bool ignoreTimescale = false)
    {
        return _instance.StartCoroutine(WaitTime(sec, e, ignoreTimescale));
    }
    /// <summary> predicate가 true 가 되면 실행 </summary>
    public static Coroutine until(Func<bool> predicate, Action e)
    {
        return _instance.StartCoroutine(WaitUntil(predicate, e));
    }

    public static Coroutine doUntilTime(float duration, Action e)
    {
        return _instance.StartCoroutine(DoUntilTime(duration, e));
    }
    public static Coroutine doUntilFrame(int frame, Action e)
    {
        return _instance.StartCoroutine(DoUntilFrame(frame, e));
    }
    public static Coroutine doUntilTime(float duration, Action<float> e)
    {
        return _instance.StartCoroutine(DoUntilTime(duration, e));
    }
    public static Coroutine doUntilFrame(int frame, Action<int> e)
    {
        return _instance.StartCoroutine(DoUntilFrame(frame, e));
    }
    
    
    static IEnumerator WaitFixedFrame(int frame, Action e)
    {
        for (int i = 0; i < frame; i++)
            yield return WAIT_FOR_FIXEDUPDATE;
        e?.Invoke();
    }
    
    static IEnumerator WaitFrame(int frame, Action e)
    {
        for (int i = 0; i < frame; i++)
            yield return null;
        e?.Invoke();
    }
    static IEnumerator WaitEndOfFrame(int frame, Action e)
    {
        for (int i = 0; i < frame; i++)
            yield return WAIT_FOR_ENDOFFRAME;
        e?.Invoke();
    }

    static IEnumerator WaitTime(float sec, Action e, bool ignoreTimeScale)
    {
        for (var f = 0f; f < sec; f += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime)
            yield return null;
        e?.Invoke();
    }

    static IEnumerator WaitUntil(Func<bool> predicate, Action e)
    {
        while (!predicate.Invoke())
            yield return null;
        
        e?.Invoke();
    }

    static IEnumerator DoUntilTime(float duration, Action e)
    {
        for (float f = 0f; f < duration; f += Time.deltaTime)
        {
            e?.Invoke();
            yield return null;
        }
    }
    static IEnumerator DoUntilFrame(int frame, Action e)
    {
        for (int i = 0; i < frame; i++)
        {
            e?.Invoke();
            yield return null;
        }
    }
    
    static IEnumerator DoUntilTime(float duration, Action<float> e)
    {
        for (float f = 0f; f < duration; f += Time.deltaTime)
        {
            e?.Invoke(f);
            yield return null;
        }
    }
    static IEnumerator DoUntilFrame(int frame, Action<int> e)
    {
        for (int i = 0; i < frame; i++)
        {
            e?.Invoke(i);
            yield return null;
        }
    }
    
        
#if UNITY_EDITOR
    public static EditorCoroutine editorFrame(int frame, Action e)
    {
        return EditorCoroutineUtility.StartCoroutineOwnerless(WaitFrame(frame, e));
    }
    
    public static EditorCoroutine editorTime(float time, Action e)
    {
        return EditorCoroutineUtility.StartCoroutineOwnerless(WaitTime(time, e, true));
    }
    
    public static EditorCoroutine editorUntil(Func<bool> predicate, Action e)
    {
        return EditorCoroutineUtility.StartCoroutineOwnerless(WaitUntil(predicate, e));
    }
#endif

}