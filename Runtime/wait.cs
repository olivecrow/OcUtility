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
    /// <summary> 지정된 시간 이전에 shouldExit == true가 될 경우, 이벤트를 호출하지 않고 종료함 </summary>
    public static Coroutine frame(int frame, Action e, Func<bool> shouldExit)
    {
        return _instance.StartCoroutine(WaitFrameOrExit(frame, e, shouldExit));
    }
    public static Coroutine endOfFrame(int frame, Action e)
    {
        return _instance.StartCoroutine(WaitEndOfFrame(frame, e));
    }
    /// <summary> 지정된 시간 이전에 shouldExit == true가 될 경우, 이벤트를 호출하지 않고 종료함 </summary>
    public static Coroutine endOfFrame(int frame, Action e, Func<bool> shouldExit)
    {
        return _instance.StartCoroutine(WaitEndOfFrameOrExit(frame, e, shouldExit));
    }
    public static Coroutine fixedFrame(int frame, Action e)
    {
        return _instance.StartCoroutine(WaitFixedFrame(frame, e));
    }
    /// <summary> 지정된 시간 이전에 shouldExit == true가 될 경우, 이벤트를 호출하지 않고 종료함 </summary>
    public static Coroutine fixedFrame(int frame, Action e, Func<bool> shouldExit)
    {
        return _instance.StartCoroutine(WaitFixedFrameOrExit(frame, e, shouldExit));
    }
    public static Coroutine time(float sec, Action e, bool ignoreTimescale = false)
    {
        return _instance.StartCoroutine(WaitTime(sec, e, ignoreTimescale));
    }
    /// <summary> 지정된 시간 이전에 shouldExit == true가 될 경우, 이벤트를 호출하지 않고 종료함 </summary>
    public static Coroutine time(float sec, Action e, Func<bool> shouldExit, bool ignoreTimescale = false)
    {
        return _instance.StartCoroutine(WaitTimeOrExit(sec, e, shouldExit, ignoreTimescale));
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
    
    static IEnumerator WaitFixedFrameOrExit(int frame, Action e, Func<bool> shouldExit)
    {
        for (int i = 0; i < frame; i++)
        {
            if(shouldExit.Invoke()) yield break;
            yield return WAIT_FOR_FIXEDUPDATE;
        }
        e?.Invoke();
    }
    
    static IEnumerator WaitFrame(int frame, Action e)
    {
        for (int i = 0; i < frame; i++)
            yield return null;
        e?.Invoke();
    }
    static IEnumerator WaitFrameOrExit(int frame, Action e, Func<bool> shouldExit)
    {
        for (int i = 0; i < frame; i++)
        {
            if(shouldExit.Invoke()) yield break;
            yield return null;
        }
        e?.Invoke();
    }
    static IEnumerator WaitEndOfFrame(int frame, Action e)
    {
        for (int i = 0; i < frame; i++)
            yield return WAIT_FOR_ENDOFFRAME;
        e?.Invoke();
    }
    static IEnumerator WaitEndOfFrameOrExit(int frame, Action e, Func<bool> shouldExit)
    {
        for (int i = 0; i < frame; i++)
        {
            if(shouldExit.Invoke()) yield break;
            yield return WAIT_FOR_ENDOFFRAME;
        }
        e?.Invoke();
    }

    static IEnumerator WaitTime(float sec, Action e, bool ignoreTimeScale)
    {
        for (var f = 0f; f < sec; f += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime)
            yield return null;
        e?.Invoke();
    }
    static IEnumerator WaitTimeOrExit(float sec, Action e, Func<bool> shouldExit, bool ignoreTimeScale)
    {
        for (var f = 0f; f < sec; f += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime)
        {
            if(shouldExit.Invoke()) yield break;
            yield return null;
        }
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