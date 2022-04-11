using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OcUtility;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MathExtension
{
    #region Vector

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
    
    public static Vector3 NewXY(this Vector3 source, float x, float y)
    {
        source.x = x;
        source.y = y;
        return source;
    }
    public static Vector3 NewXZ(this Vector3 source, float x, float z)
    {
        source.x = x;
        source.z = z;
        return source;
    }
    public static Vector3 NewYZ(this Vector3 source, float y, float z)
    {
        source.y = y;
        source.z = z;
        return source;
    }
    
    ///<summary> Vector2의 XY를 Vector3의 XZ로 바꿈.</summary>
    public static Vector3 ToXZ (this Vector2 source)
    {
        var v3 = new Vector3(source.x,0f,source.y);
        return v3;
    }

    /// <summary> 벡터의 각 요소를 더함.</summary>
    public static float Sum(this Vector2 source)
        => source.x + source.y;
    /// <summary> 벡터의 각 요소를 더함.</summary>
    public static float Sum(this Vector3 source)
        => source.x + source.y + source.z;
    /// <summary> 벡터의 각 요소를 더함.</summary>
    public static float Sum(this Vector4 source)
        => source.x + source.y + source.z + source.w;
    
    /// <summary> 벡터의 각 요소를 곱함.</summary>
    public static float SelfMultiply(this Vector2 source)
        => source.x * source.y;
    /// <summary> 벡터의 각 요소를 곱함.</summary>
    public static float SelfMultiply(this Vector3 source)
        => source.x * source.y * source.z;
    /// <summary> 벡터의 각 요소를 곱함.</summary>
    public static float SelfMultiply(this Vector4 source)
        => source.x * source.y * source.z * source.w;

    public static Vector2 Multiply(this Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }
    public static Vector3 Multiply(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static Vector4 Multiply(this Vector4 a, Vector4 b)
    {
        return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
    }
    
    public static float Avg(this Vector2 a) => a.Sum() / 2f;
    public static float Avg(this Vector3 a) => a.Sum() / 3f;
    public static float Avg(this Vector4 a) => a.Sum() / 4f;

    public static float min(this Vector2 a)
    {
        return a.x < a.y ? a.x : a.y;
    }
    public static float min(this Vector3 a)
    {
        var min = a.x;
        if (a.y < min) min = a.y;
        if (a.z < min) min = a.z;
        return min;
    }
    public static float min(this Vector4 a)
    {
        var min = a.x;
        if (a.y < min) min = a.y;
        if (a.z < min) min = a.z;
        if (a.w < min) min = a.w;
        return min;
    }
    
    public static float minXY(this Vector3 a)
    {
        var min = a.x;
        if (a.y < min) min = a.y;
        return min;
    }
    public static float minXZ(this Vector3 a)
    {
        var min = a.x;
        if (a.z < min) min = a.z;
        return min;
    }
    public static float minYZ(this Vector3 a)
    {
        var min = a.y;
        if (a.z < min) min = a.z;
        return min;
    }
    
    public static float max(this Vector2 a)
    {
        return a.x > a.y ? a.x : a.y;
    }
    public static float max(this Vector3 a)
    {
        var max = a.x;
        if (a.y > max) max = a.y;
        if (a.z > max) max = a.z;
        return max;
    }
    public static float max(this Vector4 a)
    {
        var max = a.x;
        if (a.y > max) max = a.y;
        if (a.z > max) max = a.z;
        if (a.w > max) max = a.w;
        return max;
    }
    public static float maxXY(this Vector3 a)
    {
        var max = a.x;
        if (a.y > max) max = a.y;
        return max;
    }
    public static float maxXZ(this Vector3 a)
    {
        var max = a.x;
        if (a.z > max) max = a.z;
        return max;
    }
    public static float maxYZ(this Vector3 a)
    {
        var max = a.y;
        if (a.z > max) max = a.z;
        return max;
    }
    #endregion

    #region VectorInt

    public static Vector3Int NewX(this Vector3Int source, int value)
    {
        source.x = value;
        return source;
    }

    public static Vector3Int NewY(this Vector3Int source, int value)
    {
        source.y = value;
        return source;
    }

    public static Vector3Int NewZ(this Vector3Int source, int value)
    {
        source.z = value;
        return source;
    }
    
    public static Vector3Int NewXY(this Vector3Int source, int x, int y)
    {
        source.x = x;
        source.y = y;
        return source;
    }
    public static Vector3Int NewXZ(this Vector3Int source, int x, int z)
    {
        source.x = x;
        source.z = z;
        return source;
    }
    public static Vector3Int NewYZ(this Vector3Int source, int y, int z)
    {
        source.y = y;
        source.z = z;
        return source;
    }
    
    ///<summary> Vector2의 XY를 Vector3의 XZ로 바꿈.</summary>
    public static Vector3Int ToXZ (this Vector2Int source)
    {
        var v3 = new Vector3Int(source.x,0,source.y);
        return v3;
    }

    /// <summary> 벡터의 각 요소를 더함.</summary>
    public static int Sum(this Vector2Int source)
        => source.x + source.y;
    /// <summary> 벡터의 각 요소를 더함.</summary>
    public static int Sum(this Vector3Int source)
        => source.x + source.y + source.z;
    
    /// <summary> 벡터의 각 요소를 곱함.</summary>
    public static int SelfMultiply(this Vector2Int source)
        => source.x * source.y;
    /// <summary> 벡터의 각 요소를 곱함.</summary>
    public static int SelfMultiply(this Vector3Int source)
        => source.x * source.y * source.z;
    
    public static Vector2Int Multiply(this Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x * b.x, a.y * b.y);
    }
    public static Vector3Int Multiply(this Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    
    public static float Avg(this Vector2Int a) => a.Sum() / 2f;
    public static float Avg(this Vector3Int a) => a.Sum() / 3f;
    
    public static int min(this Vector2Int a)
    {
        return a.x < a.y ? a.x : a.y;
    }
    public static int min(this Vector3Int a)
    {
        var min = a.x;
        if (a.y < min) min = a.y;
        if (a.z < min) min = a.z;
        return min;
    }
    public static int minXY(this Vector3Int a)
    {
        var min = a.x;
        if (a.y < min) min = a.y;
        return min;
    }
    public static int minXZ(this Vector3Int a)
    {
        var min = a.x;
        if (a.z < min) min = a.z;
        return min;
    }
    public static int minYZ(this Vector3Int a)
    {
        var min = a.y;
        if (a.z < min) min = a.z;
        return min;
    }
    public static int max(this Vector2Int a)
    {
        return a.x > a.y ? a.x : a.y;
    }
    public static int max(this Vector3Int a)
    {
        var max = a.x;
        if (a.y > max) max = a.y;
        if (a.z > max) max = a.z;
        return max;
    }
    public static int maxXY(this Vector3Int a)
    {
        var max = a.x;
        if (a.y > max) max = a.y;
        return max;
    }
    public static int maxXZ(this Vector3Int a)
    {
        var max = a.x;
        if (a.z > max) max = a.z;
        return max;
    }
    public static int maxYZ(this Vector3Int a)
    {
        var max = a.y;
        if (a.z > max) max = a.z;
        return max;
    }
    #endregion



    #region (Float) IsInRange, Remap

    /// <summary> float 확장. 이 float이 range에 포함되어있으면 true. (include) </summary>
    public static bool IsInRange(this float source, in Vector2 range)
        => range.x <= source && source <= range.y;
    
    /// <summary> float 확장. 이 float이 range에 포함되어있으면 true. (include) </summary>
    public static bool IsInRange(this float source, float min, float max)
        => min <= source && source <= max;
    

    /// <summary> value가 beforeRange에서 갖던 비율 만큼 targetRange 범위에서 정의되는 값을 반환함.</summary>
    public static float Remap(this float value, in Vector2 beforeRange, in Vector2 targetRange, bool useClamp = true)
    {
        var denominator = beforeRange.y - beforeRange.x;
        if (denominator == 0) return targetRange.x;
        
        var ratio = (value - beforeRange.x) / denominator;
        var result = (targetRange.y - targetRange.x) * ratio + targetRange.x;
        return useClamp ? Mathf.Clamp(result, targetRange.x, targetRange.y) : result;
    }
    public static float Remap(this float value, float beforeRangeMin, float beforeRangeMax, in Vector2 targetRange, bool useClamp = true)
    {
        var denominator = beforeRangeMax - beforeRangeMin;
        if (denominator == 0) return targetRange.x;
        
        var ratio = (value - beforeRangeMin) / denominator;
        var result = (targetRange.y - targetRange.x) * ratio + targetRange.x;
        return useClamp ? Mathf.Clamp(result, targetRange.x, targetRange.y) : result;
    }
    public static float Remap(this float value, in Vector2 beforeRange, float targetRangeMin, float targetRangeMax, bool useClamp = true)
    {
        var denominator = beforeRange.y - beforeRange.x;
        if (denominator == 0) return targetRangeMin;
        
        var ratio = (value - beforeRange.x) / denominator;
        var result = (targetRangeMax - targetRangeMin) * ratio + targetRangeMin;
        return useClamp ? Mathf.Clamp(result, targetRangeMin, targetRangeMax) : result;
    }
    
    /// <summary> value가 beforeRange에서 갖던 비율 만큼 targetRange 범위에서 정의되는 값을 반환함.</summary>
    public static float Remap(this float value, float beforeRangeMin, float beforeRangeMax, float targetRangeMin, float targetRangeMax, bool useClamp = true)
    {
        var denominator = beforeRangeMax - beforeRangeMin;
        if (denominator == 0) return targetRangeMin;
        
        var ratio = (value - beforeRangeMin) / denominator;
        var result = (targetRangeMax - targetRangeMin) * ratio + targetRangeMin;
        return useClamp ? Mathf.Clamp(result, targetRangeMin, targetRangeMax) : result;
    }
    /// <summary> value가 beforeRange에서 갖던 비율 만큼 0..1 범위에서 정의되는 값을 반환함.</summary>
    public static float RemapTo01(this float value, in Vector2 beforeRange, bool useClamp = true)
        => Remap(value, beforeRange, Vector2.up, useClamp);
    
    public static float RemapTo01(this float value, float beforeRangeMin, float beforeRangeMax, bool useClamp = true)
        => Remap(value, beforeRangeMin, beforeRangeMax, Vector2.up, useClamp);
    
        
    /// <summary> 0..1에서 정의된 value를 targetRange의 범위로 정의함</summary>
    public static float RemapFrom01(this float value, in Vector2 targetRange, bool useClamp = true)
        => Remap(value, Vector2.up, targetRange, useClamp);
    
    /// <summary> 0..1에서 정의된 value를 targetRange의 범위로 정의함</summary>
    public static float RemapFrom01(this float value, float min, float max, bool useClamp = true)
        => Remap(value, Vector2.up, min, max, useClamp);

    #endregion



    #region (Int) IsInRange, Remap

    /// <summary> int 확장. range에 포함되어있으면 true. (include) </summary>
    public static bool IsInRange(this int source, in Vector2 range)
        => range.x <= source && source <= range.y;
    
    /// <summary> int 확장. 이 float이 range에 포함되어있으면 true. (include) </summary>
    public static bool IsInRange(this int source, float min, float max)
        => min <= source && source <= max;
    

    /// <summary> value가 beforeRange에서 갖던 비율 만큼 targetRange 범위에서 정의되는 값을 반환함.</summary>
    public static float Remap(this int value, in Vector2 beforeRange, in Vector2 targetRange, bool useClamp = true) 
        => Remap((float)value, beforeRange, targetRange, useClamp);

    public static float Remap(this int value, float beforeRangeMin, float beforeRangeMax, in Vector2 targetRange, bool useClamp = true)
        => Remap((float)value, beforeRangeMin, beforeRangeMax, targetRange, useClamp);

    public static float Remap(this int value, in Vector2 beforeRange, float targetRangeMin, float targetRangeMax, bool useClamp = true)
        => Remap((float)value, beforeRange, targetRangeMin, targetRangeMax, useClamp);

    /// <summary> value가 beforeRange에서 갖던 비율 만큼 targetRange 범위에서 정의되는 값을 반환함.</summary>
    public static float Remap(this int value, float beforeRangeMin, float beforeRangeMax, float targetRangeMin, float targetRangeMax, bool useClamp = true)
        => Remap((float)value, beforeRangeMin, beforeRangeMax, targetRangeMin, targetRangeMax, useClamp);
    /// <summary> value가 beforeRange에서 갖던 비율 만큼 0..1 범위에서 정의되는 값을 반환함.</summary>
    public static float RemapTo01(this int value, in Vector2 beforeRange, bool useClamp = true)
        => Remap(value, beforeRange, Vector2.up, useClamp);
    
    public static float RemapTo01(this int value, float beforeRangeMin, float beforeRangeMax, bool useClamp = true)
        => Remap(value, beforeRangeMin, beforeRangeMax, Vector2.up, useClamp);
    
        
    /// <summary> 0..1에서 정의된 value를 targetRange의 범위로 정의함</summary>
    public static float RemapFrom01(this int value, in Vector2 targetRange, bool useClamp = true)
        => Remap(value, Vector2.up, targetRange, useClamp);
    
    /// <summary> 0..1에서 정의된 value를 targetRange의 범위로 정의함</summary>
    public static float RemapFrom01(this int value, float min, float max, bool useClamp = true)
        => Remap(value, Vector2.up, min, max, useClamp);

    #endregion



    #region (Double) IsInRange, Remap

    /// <summary> double 확장. range에 포함되어있으면 true. (include) </summary>
    public static bool IsInRange(this double source, in Vector2 range)
        => range.x <= source && source <= range.y;
    
    /// <summary> double 확장. 이 float이 range에 포함되어있으면 true. (include) </summary>
    public static bool IsInRange(this double source, float min, float max)
        => min <= source && source <= max;
    

    /// <summary> value가 beforeRange에서 갖던 비율 만큼 targetRange 범위에서 정의되는 값을 반환함.</summary>
    public static float Remap(this double value, in Vector2 beforeRange, in Vector2 targetRange, bool useClamp = true) 
        => Remap((float)value, beforeRange, targetRange, useClamp);

    public static float Remap(this double value, float beforeRangeMin, float beforeRangeMax, in Vector2 targetRange, bool useClamp = true)
        => Remap((float)value, beforeRangeMin, beforeRangeMax, targetRange, useClamp);

    public static float Remap(this double value, in Vector2 beforeRange, float targetRangeMin, float targetRangeMax, bool useClamp = true)
        => Remap((float)value, beforeRange, targetRangeMin, targetRangeMax, useClamp);

    /// <summary> value가 beforeRange에서 갖던 비율 만큼 targetRange 범위에서 정의되는 값을 반환함.</summary>
    public static float Remap(this double value, float beforeRangeMin, float beforeRangeMax, float targetRangeMin, float targetRangeMax, bool useClamp = true)
        => Remap((float)value, beforeRangeMin, beforeRangeMax, targetRangeMin, targetRangeMax, useClamp);
    /// <summary> value가 beforeRange에서 갖던 비율 만큼 0..1 범위에서 정의되는 값을 반환함.</summary>
    public static float RemapTo01(this double value, in Vector2 beforeRange, bool useClamp = true)
        => Remap(value, beforeRange, Vector2.up, useClamp);
    
    public static float RemapTo01(this double value, float beforeRangeMin, float beforeRangeMax, bool useClamp = true)
        => Remap(value, beforeRangeMin, beforeRangeMax, Vector2.up, useClamp);
    
        
    /// <summary> 0..1에서 정의된 value를 targetRange의 범위로 정의함</summary>
    public static float RemapFrom01(this double value, in Vector2 targetRange, bool useClamp = true)
        => Remap(value, Vector2.up, targetRange, useClamp);
    
    /// <summary> 0..1에서 정의된 value를 targetRange의 범위로 정의함</summary>
    public static float RemapFrom01(this double value, float min, float max, bool useClamp = true)
        => Remap(value, Vector2.up, min, max, useClamp);

    #endregion


    #region IEnumerable

    /// <summary>
    /// 열거형의 요소 중에서 특정 값이 가장 작은 것의 인덱스를 반환함.
    /// </summary>
    public static int GetMinElementIndex<T>(this IEnumerable<T> enumerable, Func<T, float> calculate)
    {
        return GetMinElementIndex(enumerable, calculate, enumerable.Count());
    }
    public static int GetMinElementIndex<T>(this IEnumerable<T> enumerable, Func<T, float> calculate, int length)
    {
        var min = float.MaxValue;
        var idx = -1;
        for (var i = 0; i < length; i++)
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
        return GetMinElement(enumerable, calculate, enumerable.Count());
    }
    public static T GetMinElement<T>(this IEnumerable<T> enumerable, Func<T, float> calculate, int length)
    {
        var min = float.MaxValue;
        var idx = -1;
        for (var i = 0; i < length; i++)
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
        return GetMaxElementIndex(enumerable, calculate, enumerable.Count());
    }
    public static int GetMaxElementIndex<T>(this IEnumerable<T> enumerable, Func<T, float> calculate, int length)
    {
        var max = float.MinValue;
        var idx = -1;
        for (var i = 0; i < length; i++)
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
        return GetMaxElement(enumerable, calculate, enumerable.Count());
    }
    public static T GetMaxElement<T>(this IEnumerable<T> enumerable, Func<T, float> calculate, int length)
    { ;
        var max = float.MinValue;
        var idx = -1;
        for (var i = 0; i < length; i++)
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
    public static Vector2 Sum(this IEnumerable<Vector2> enumerable)
    {
        var count = enumerable.Count();
        Vector2 result = default;
        for (int i = 0; i < count; i++)
        {
            result += enumerable.ElementAt(i);
        }

        return result;
    }
    public static Vector3 Sum(this IEnumerable<Vector3> enumerable)
    {
        var count = enumerable.Count();
        Vector3 result = default;
        for (int i = 0; i < count; i++)
        {
            result += enumerable.ElementAt(i);
        }

        return result;
    }
    public static Vector4 Sum(this IEnumerable<Vector4> enumerable)
    {
        var count = enumerable.Count();
        Vector4 result = default;
        for (int i = 0; i < count; i++)
        {
            result += enumerable.ElementAt(i);
        }

        return result;
    }
    
    public static Vector2 Sum<T>(this IEnumerable<T> enumerable, Func<T, Vector2> getter)
    {
        var count = enumerable.Count();
        Vector2 result = default;
        for (int i = 0; i < count; i++)
        {
            result += getter.Invoke(enumerable.ElementAt(i));
        }

        return result;
    }
    public static Vector3 Sum<T>(this IEnumerable<T> enumerable, Func<T, Vector3> getter)
    {
        var count = enumerable.Count();
        Vector3 result = default;
        for (int i = 0; i < count; i++)
        {
            result += getter.Invoke(enumerable.ElementAt(i));
        }

        return result;
    }
    public static Vector4 Sum<T>(this IEnumerable<T> enumerable, Func<T, Vector4> getter)
    {
        var count = enumerable.Count();
        Vector4 result = default;
        for (int i = 0; i < count; i++)
        {
            result += getter.Invoke(enumerable.ElementAt(i));
        }

        return result;
    }
    
    public static TResult Sum<TResult, TSource>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> getter, Func<TResult, TResult, TResult> add)
    {
        var count = enumerable.Count();
        TResult result = default;
        for (int i = 0; i < count; i++)
        {
            result = add(result, getter(enumerable.ElementAt(i)));
        }

        return result;
    }

    public static float Sum<T>(this IEnumerable<T> enumerable, Func<T, float> getter)
    {
        var count = enumerable.Count();
        var result = 0f;
        for (int i = 0; i < count; i++)
        {
            result += getter.Invoke(enumerable.ElementAt(i));
        }

        return result;
    }
    
    public static int Sum<T>(this IEnumerable<T> enumerable, Func<T, int> getter)
    {
        var count = enumerable.Count();
        var result = 0;
        for (int i = 0; i < count; i++)
        {
            result += getter.Invoke(enumerable.ElementAt(i));
        }

        return result;
    }

    public static float Multiply<T>(this IEnumerable<T> enumerable, Func<T, float> getter)
    {
        var count = enumerable.Count();
        var result = 0f;
        for (int i = 0; i < count; i++)
        {
            result *= getter.Invoke(enumerable.ElementAt(i));
        }

        return result;
    }
    
    public static int Multiply<T>(this IEnumerable<T> enumerable, Func<T, int> getter)
    {
        var count = enumerable.Count();
        var result = 0;
        for (int i = 0; i < count; i++)
        {
            result *= getter.Invoke(enumerable.ElementAt(i));
        }

        return result;
    }

    public static int IndexOf<T>(this IEnumerable<T> enumerable, T member)
    {
        return IndexOf(enumerable, member, enumerable.Count());
    }
    public static int IndexOf<T>(this IEnumerable<T> enumerable, T member, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (enumerable.ElementAt(i).Equals(member)) return i;
        }

        return -1;
    }
    public static int IndexOf<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
    {
        return IndexOf(enumerable, predicate, enumerable.Count());
    }
    public static int IndexOf<T>(this IEnumerable<T> enumerable, Predicate<T> predicate, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (predicate.Invoke(enumerable.ElementAt(i))) return i;
        }

        return -1;
    }

    public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action, int length)
    {
        for (int i = 0; i < length; i++)
        {
            action?.Invoke(enumerable.ElementAt(i));
        }
    }

    public static bool Any<T>(this IEnumerable<T> enumerable, Predicate<T> predicate, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (predicate.Invoke(enumerable.ElementAt(i))) return true;
        }

        return false;
    }
    public static bool All<T>(this IEnumerable<T> enumerable, Predicate<T> predicate, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (!predicate.Invoke(enumerable.ElementAt(i))) return false;
        }

        return true;
    }

    #endregion

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


    /// <summary> 해당 숫자를 decimalCount번째 자리 소수로 반올림함. decimalCount = 2면 x.xx가 되는 식. </summary>
    public static float Round(this float source, int decimalCount = 0)
    {
        if (decimalCount == 0) return Mathf.Round(source);
        return Mathf.Round(source * Pow10(decimalCount)) / Pow10(decimalCount);
    }

    /// <summary> 벡터의 각 요소를 decimalCount번째 자리 소수로 반올림함. decimalCount = 2면 x.xx가 되는 식. </summary>
    public static Vector2 Round(this Vector2 source, int decimalCount = 0)
    {
        return new Vector2(source.x.Round(decimalCount), source.y.Round(decimalCount));
    }
    /// <summary> 벡터의 각 요소를 decimalCount번째 자리 소수로 반올림함. decimalCount = 2면 x.xx가 되는 식. </summary>
    public static Vector3 Round(this Vector3 source, int decimalCount = 0)
    {
        return new Vector3(source.x.Round(decimalCount), source.y.Round(decimalCount), source.z.Round(decimalCount));
    }
    /// <summary> 벡터의 각 요소를 decimalCount번째 자리 소수로 반올림함. decimalCount = 2면 x.xx가 되는 식. </summary>
    public static Vector4 Round(this Vector4 source, int decimalCount = 0)
    {
        return new Vector4(source.x.Round(decimalCount), source.y.Round(decimalCount), 
            source.z.Round(decimalCount), source.w.Round(decimalCount));
    }
    
    /// <summary> 매개 변수값에 따른 10의 거듭제곱을 반환함. Mathf.Pow(10, pow)를 더욱 빨리 쓰도록 한 것.
    /// 정확도 및 int의 최댓값 제한으로, -5 ~ 19 까지의 값만 매개변수로 사용할 수 있다.</summary>
    public static float Pow10(int pow)
    {
        if (pow < -5)
        {
            Printer.Print($"유효하지 않은 매개변수 : {pow}");
            pow = -5;
        }
        else if (pow > 19)
        {
            Printer.Print($"유효하지 않은 매개변수 : {pow}");
            pow = 19;
        }
        return pow switch
        {
            -5 => 0.00001f,
            -4 => 0.0001f,
            -3 => 0.001f,
            -2 => 0.01f,
            -1 => 0.1f,
            1  => 10,
            2  => 100,
            3  => 1000,
            4  => 10000,
            5  => 100000,
            6  => 1000000,
            7  => 10000000,
            8  => 100000000,
            9  => 1000000000,
            10 => 10000000000,
            11 => 100000000000,
            12 => 1000000000000,
            13 => 10000000000000,
            14 => 100000000000000,
            15 => 1000000000000000,
            16 => 10000000000000000,
            17 => 100000000000000000,
            18 => 1000000000000000000,
            19 => 10000000000000000000,
        _ => 1
        };
    }

    public static int Sign(this int source)
    {
        return source > 0 ? 1 : -1;
    }

    public static int Sign(this float source)
    {
        return source > 0 ? 1 : -1;
    }

    public static int Sign(this double source)
    {
        return source > 0 ? 1 : -1;
    }
    public static Vector2 Sign(this Vector2 source)
    {
        return new Vector2(source.x.Sign(), source.y.Sign());
    }
    public static Vector3 Sign(this Vector3 source)
    {
        return new Vector3(source.x.Sign(), source.y.Sign(), source.z.Sign());
    }
    public static Vector4 Sign(this Vector4 source)
    {
        return new Vector4(source.x.Sign(), source.y.Sign(), source.z.Sign(), source.w.Sign());
    }

    public static int abs(this int source) => source > 0 ? source : -1 * source;
    public static float abs(this float source) => source > 0 ? source : -1 * source;
    public static double abs(this double source) => source > 0 ? source : -1 * source;
    public static Vector2 abs(this Vector2 source) => new Vector2(source.x.abs(), source.y.abs());
    public static Vector3 abs(this Vector3 source) => new Vector3(source.x.abs(), source.y.abs(), source.z.abs());
    public static Vector4 abs(this Vector4 source) => new Vector4(source.x.abs(), source.y.abs(), source.z.abs(), source.w.abs());

    public static float sqr(this float source) => source * source;
    public static int sqr(this int source) => source * source;
    public static double sqr(this double source) => source * source;

    #region EnumFlags

    public static bool Has<T>(this Enum type, T value) {
        try {
            return ((int)(object)type & (int)(object)value) == (int)(object)value;
        }
        catch {
            return false;
        }
    }

    public static bool HasOneOf<T>(this Enum type, T value)
    {
        try {
            return ((int)(object)type & (int)(object)value) != 0;
        }
        catch {
            return false;
        }
    }

    public static bool Is<T>(this Enum type, T value) {
        try {
            return (int)(object)type == (int)(object)value;
        }
        catch {
            return false;
        }
    }


    public static T Add<T>(this Enum type, T value) {
        try {
            return (T)(object)((int)(object)type | (int)(object)value);
        }
        catch(Exception ex) {
            throw new ArgumentException(
                string.Format(
                    "Could not append value from enumerated type '{0}'.",
                    typeof(T).Name
                ), ex);
        }
    }

    public static T Remove<T>(this Enum type, T value) {
        try {
            return (T)(object)((int)(object)type & ~(int)(object)value);
        }
        catch (Exception ex) {
            throw new ArgumentException(
                string.Format(
                    "Could not remove value from enumerated type '{0}'.",
                    typeof(T).Name
                ), ex);
        }
    }
    
    #endregion


    #region Random

    public static float RandomSign(this float source)
    {
        return Random.Range(0, 2) == 0 ? source : source * -1;
    }
    public static int RandomSign(this int source)
    {
        return Random.Range(0, 2) == 0 ? source : source * -1;
    }

    public static Vector3 GetRandomPosition(this Bounds bounds)
    {
        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Random.Range(bounds.min.y, bounds.max.y);
        var z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }

    public static float random(this Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }
    public static int random(this Vector2Int range)
    {
        return Random.Range(range.x, range.y);
    }

    public static T random<T>(this IEnumerable<T> enumerable)
    {
        var idx = Random.Range(0, enumerable.Count());
        return enumerable.ElementAt(idx);
    }
    #endregion
    
    
    public static Bounds GetVoxelBounds(Vector3 position, Vector3 size)
    {
        return new Bounds(position.Snapped(size), size);
    }
    public static Vector2 Snapped(this Vector2 v, Vector2 unit)
    {
        return new Vector2(v.x.Snapped(unit.x), v.y.Snapped(unit.y));
    }
    public static Vector3 Snapped(this Vector3 v, Vector3 unit)
    {
        return new Vector3(v.x.Snapped(unit.x), v.y.Snapped(unit.y), v.z.Snapped(unit.z));
    }
    public static Vector4 Snapped(this Vector4 v, Vector4 unit)
    {
        return new Vector4(v.x.Snapped(unit.x), v.y.Snapped(unit.y), v.z.Snapped(unit.z), v.w.Snapped(unit.w));
    }
    public static float Snapped(this float f, float unit)
    {
        var absUnit = unit.abs();
        var count = Mathf.RoundToInt(f / absUnit);

        return unit * count;
    }

    public static float CalcHypotenuseOfRightAngledTriangle(float x, float y)
    {
        return Mathf.Sqrt(x.sqr() + y.sqr());
    }
}