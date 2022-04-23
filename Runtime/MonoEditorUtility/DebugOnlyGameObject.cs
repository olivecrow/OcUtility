using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OcUtility
{
    [ExecuteInEditMode]
    public class DebugOnlyGameObject : MonoBehaviour
    {
        [InfoBox("이 게임오브젝트는 릴리즈 빌드에서 제외됨.")]
        [ShowInInspector]public HideFlags HideFlags => gameObject.hideFlags;
    }
}