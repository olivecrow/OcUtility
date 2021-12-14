using UnityEngine;

public interface IHierarchyIconDrawable
{
#if UNITY_EDITOR
    Object IconTarget { get; }
    Texture2D OverrideIcon { get; }
    /// <summary>
    /// 아이콘의 컬러값.
    /// </summary>
    Color IconTint { get; }
#endif
}