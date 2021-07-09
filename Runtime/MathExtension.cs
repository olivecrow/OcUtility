using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MathExtension
{
    public static Vector3 NewX(this Vector3 source, float value)
    {
        source.x = value;
        return source;
    }

    public static Vector3 NewY(this Vector3 source, float value)
    {
        source.y = value;
        return source;
    }

    public static Vector3 NewZ(this Vector3 source, float value)
    {
        source.z = value;
        return source;
    }
    
    ///<summary> Vector2의 XY를 Vector3의 XZ로 바꿈.</summary>
    public static Vector3 ToXZ (this Vector2 source)
    {
        var v3 = new Vector3(source.x,0f,source.y);
        return v3;
    }
    
    /// <summary> float 확장. 이 float이 range에 포함되어있으면 true. (include) </summary>
    public static bool IsInRange(this float source, in Vector2 range)
    {
        return range.x <= source && source <= range.y;
    }

    /// <summary> value가 beforeRange에서 갖던 비율 만큼 targetRange 범위에서 정의되는 값을 반환함.</summary>
    public static float Remap(this float value, in Vector2 beforeRange, in Vector2 targetRange, bool useClamp = true)
    {
        var ratio = (value - beforeRange.x) / (beforeRange.y - beforeRange.x);
        var result = (targetRange.y - targetRange.x) * ratio + targetRange.x;
        return useClamp ? Mathf.Clamp(result, targetRange.x, targetRange.y) : result;
    }
    
    /// <summary> value가 beforeRange에서 갖던 비율 만큼 targetRange 범위에서 정의되는 값을 반환함.</summary>
    public static float Remap(this float value, float beforeRangeMin, float beforeRangeMax, float targetRangeMin, float targetRangeMax, bool useClamp = true)
    {
        var ratio = (value - beforeRangeMin) / (beforeRangeMax - beforeRangeMin);
        var result = (targetRangeMax - targetRangeMin) * ratio + targetRangeMin;
        return useClamp ? Mathf.Clamp(result, targetRangeMin, targetRangeMax) : result;
    }
        
    /// <summary> value가 beforeRange에서 갖던 비율 만큼 0..1 범위에서 정의되는 값을 반환함.</summary>
    public static float RemapTo01(this float value, in Vector2 beforeRange, bool useClamp = true)
    {
        return Remap(value, beforeRange, Vector2.up, useClamp);
    }
        
    /// <summary> 0..1에서 정의된 value를 targetRange의 범위로 정의함</summary>
    public static float RemapFrom01(this float value, in Vector2 targetRange, bool useClamp = true)
    {
        return Remap(value, Vector2.up, targetRange, useClamp);
    }
    /// <summary> f1과 f2가 근사치 이내의 차이를 갖는지 판단함. </summary>
    public static bool Approximately(float f1, float f2, float precision = 0.002f)
    {
        return -precision < f1 - f2 && f1 - f2 < precision;
    }
    /// <summary> 해당 값이 f와 근사치 이내의 차이를 갖는지 판단함. </summary>
    public static bool IsApproximately(this float source, float f, float precision = 0.002f)
    {
        return -precision < source - f && source - f < precision;
    }

    /// <summary>
    /// 열거형의 요소 중에서 특정 값이 가장 작은 것의 인덱스를 반환함.
    /// </summary>
    public static int GetMinElementIndex<T>(this IEnumerable<T> enumerable, Func<T, float> calculate)
    {
        var count = enumerable.Count();
        var min = float.MaxValue;
        var idx = -1;
        for (var i = 0; i < count; i++)
        {
            var value = calculate.Invoke(enumerable.ElementAt(i));
            if (value < min)
            {
                min = value;
                idx = i;
            }
        }

        return idx;
    }
    
    /// <summary>
    /// 열거형의 요소 중, 특정 값이 가장 작은 요소를 반환함.
    /// </summary>
    public static T GetMinElement<T>(this IEnumerable<T> enumerable, Func<T, float> calculate)
    {
        var count = enumerable.Count();
        var min = float.MaxValue;
        var idx = -1;
        for (var i = 0; i < count; i++)
        {
            var value = calculate.Invoke(enumerable.ElementAt(i));
            if (value < min)
            {
                min = value;
                idx = i;
            }
        }

        return enumerable.ElementAt(idx);
    }

    /// <summary>
    /// 두 개의 요소중 특정 값을 비교하여 작은 것을 반환함.
    /// </summary>
    public static T GetMinElement<T>(T t1, T t2, Func<T, float> compare)
    {
        var value1 = compare.Invoke(t1);
        var value2 = compare.Invoke(t2);

        return value1 < value2 ? t1 : t2;
    }
        
    /// <summary>
    /// 열거형의 요소 중에서 특정 값이 가장 큰 것의 인덱스를 반환함.
    /// </summary>
    public static int GetMaxElementIndex<T>(this IEnumerable<T> enumerable, Func<T, float> calculate)
    {
        var count = enumerable.Count();
        var max = float.MinValue;
        var idx = -1;
        for (var i = 0; i < count; i++)
        {
            var value = calculate.Invoke(enumerable.ElementAt(i));
            if (value > max)
            {
                max = value;
                idx = i;
            }
        }

        return idx;
    }
    /// <summary>
    /// 열거형의 요소 중, 특정 값이 가장 큰 요소를 반환함.
    /// </summary>
    public static T GetMaxElement<T>(this IEnumerable<T> enumerable, Func<T, float> calculate)
    {
        var count = enumerable.Count();
        var max = float.MinValue;
        var idx = -1;
        for (var i = 0; i < count; i++)
        {
            var value = calculate.Invoke(enumerable.ElementAt(i));
            if (value > max)
            {
                max = value;
                idx = i;
            }
        }

        return enumerable.ElementAt(idx);
    }

    /// <summary>
    /// 두 개의 요소중 특정 값을 비교하여 큰 것을 반환함.
    /// </summary>
    public static T GetMaxElement<T>(T t1, T t2, Func<T, float> compare)
    {
        var value1 = compare.Invoke(t1);
        var value2 = compare.Invoke(t2);

        return value1 > value2 ? t1 : t2;
    }
}