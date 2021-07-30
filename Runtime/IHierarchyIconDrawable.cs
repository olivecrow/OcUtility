using UnityEngine;

public interface IHierarchyIconDrawable
{
    Texture2D IconTexture { get; }
    /// <summary>
    /// 아이콘의 위치. Editor Default Resources에 있는 경우, 해당 폴더 하위부터의 경로와 확장자를 적어주고,
    /// Resources에 있는 경우, 확장자를 제외한 해당 폴더의 하위 경로를 적어줄 것.
    /// 그 외에는 전체 경로와 확장자를 적어주면 된다.
    /// </summary>
    string IconPath { get; }

    /// <summary>
    /// Hierarchy 창의 왼쪽으로부터의 거리.
    /// Default = 15;
    /// </summary>
    int DistanceToText { get; }
    /// <summary>
    /// 아이콘의 컬러값.
    /// </summary>
    Color IconTint { get; }
}