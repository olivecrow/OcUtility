using System.Collections.Generic;
using UnityEngine;

namespace OcUtility
{
    public class MarkerSO : ScriptableObject
    {
#if UNITY_EDITOR
        public List<RuntimeMarker.Marker> markers;  
#endif
    }
}