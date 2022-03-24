using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace OcUtility
{
    public static class ColorExtension
    {
        public static Color SetR(this Color source, float value)
        {
            source.r = value;
            return source;
        }

        public static Color SetG(this Color source, float value)
        {
            source.g = value;
            return source;
        }

        public static Color SetB(this Color source, float value)
        {
            source.b = value;
            return source;
        }

        public static Color SetA(this Color source, float value)
        {
            source.a = value;
            return source;
        }
        /// <summary> RBG값을 시간에 따라 변경하게 해서 지속적으로 호출 시 무지개 색깔의 효과가 나오도록 함.</summary>
        public static Color Rainbow(float speed = 1)
        {
            var r = (Mathf.Sin(Time.realtimeSinceStartup * speed) + 1) * 0.5f;
            var g = (Mathf.Sin((Time.realtimeSinceStartup - 0.666f * Mathf.PI) * speed) + 1) * 0.5f;
            var b = (Mathf.Sin((Time.realtimeSinceStartup - 1.333f * Mathf.PI) * speed) + 1) * 0.5f;

            return new Color(r, g, b);
        }
        
        [Obsolete]public static string ToRichText(this string source, Color target)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(target)}>{source}</color>";
        }

        public static string Rich(this string source, Color target)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(target)}>{source}</color>";
        }

        /// <summary> Debug Rich Text by seed value </summary>
        public static string DRT(this string source, int seed)
        {
            return source.Rich(Random(seed, 0.4f));
        }
        
        /// <summary> Debug Rich Text </summary>
        public static string DRT(this string source, object seed)
        {
            return DRT(source, seed.GetHashCode());
        }

        public static float SumRGB(this Color source)
        {
            return source.r + source.g + source.b;
        }

        public static float SumRGBA(this Color source)
        {
            return source.r + source.g + source.b + source.a;
        }
        

        /// <summary> 색을 밝게 함. 각 채널에 더해주는 방식. 어둡게 하는 경우, 색이 강한 채널의 채도가 강하게 남는 경향이 있다. </summary>
        public static Color Brighten(this Color source, float value)
        {
            source.r += value;
            source.g += value;
            source.b += value;
            
            return source;
        }

        /// <summary> 색을 어둡게 함. 각 채널에 value를 곱하는 방식이기 때문에 밝아지는 기능으로는 이용하기 힘듦. (0인 채널은 변하지 않기 때문)
        /// 어두워지면서 채도가 고르게 낮아진다.</summary>
        public static Color Darken(this Color source, float value)
        {
            source.r *= value;
            source.g *= value;
            source.b *= value;

            return source;
        }

        public static Color Gray(float rgb, float a = 1f)
        {
            return new Color(rgb, rgb, rgb, a);
        }

        /// <summary> 무작위의 한 색깔을 출력함. </summary>
        public static Color Random() => new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        /// <summary> 무작위의 한 색깔을 출력함. </summary>
        public static Color SystemRandom()
        {
            var random = new System.Random();
            return new Color(random.Next(0, 255) * 0.0039f, random.Next(0, 255) * 0.0039f, random.Next(0, 255) * 0.0039f);
        }

        /// <summary> 전달된 seed에 맞는 색을 하나 출력함. seed가 같으면 같은 색이 출력됨.
        /// 색상의 밝기 범위를 지정하려면 remap에 범위를 추가하면 됨.</summary>
        public static Color Random(int seed, float remapMin = 0, float remapMax = 1)
        {
            var rr = Mathf.Sin(seed * Mathf.Rad2Deg - Mathf.Rad2Deg * 1.14f);
            var gg = Mathf.Sin(seed * Mathf.Rad2Deg * Mathf.Rad2Deg - Mathf.Rad2Deg + 0.666f * Mathf.PI);
            var bb = Mathf.Cos(seed * Mathf.Rad2Deg * Mathf.Rad2Deg - 1.333f * Mathf.PI);
            
            var r = Mathf.Repeat(rr, 1);
            var g = Mathf.Repeat(gg, 1);
            var b = Mathf.Repeat(bb, 1);

            return new Color(r.RemapFrom01(remapMin, remapMax), g.RemapFrom01(remapMin, remapMax), b.RemapFrom01(remapMin, remapMax));
        }

        public static Color Random(int seed, in Vector2 remapRange)
        {
            return Random(seed, remapRange.x, remapRange.y);
        }
        public static Color Invert(this Color source, bool invertAlpha = false)
        {
            return new Color(1 - source.r, 1 - source.g, 1 - source.b, invertAlpha ? 1 - source.a : source.a);
        }
    }

}