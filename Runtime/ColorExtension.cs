using System.Collections;
using System.Collections.Generic;
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
        /// <summary> RBG값을 시간에 따라 변경하게 해서 지속적으로 호출 시 무지개 색깔의 효과가 나오도록 함. 한 번만 호출할 경우엔 임의의 색상이 나오는 효과가 있음. </summary>
        public static Color Rainbow(float a = 1)
        {
            var r = (Mathf.Sin(Time.realtimeSinceStartup) + 1) * 0.5f;
            var g = (Mathf.Sin(Time.realtimeSinceStartup - 0.666f * Mathf.PI) + 1) * 0.5f;
            var b = (Mathf.Sin(Time.realtimeSinceStartup - 1.333f * Mathf.PI) + 1) * 0.5f;

            return new Color(r, g, b, a);
        }

        public static string ToRichText(this string source, Color target)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(target)}>{source}</color>";
        }
    }

}