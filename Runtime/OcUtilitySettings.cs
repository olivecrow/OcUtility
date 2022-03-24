using UnityEditor;
using UnityEngine;

namespace OcUtility
{
    [CreateAssetMenu(fileName = "OcUtility Settings", menuName = "OcUtility/Settings", order = 0)]
    public class OcUtilitySettings : ScriptableObject
    {
        internal static OcUtilitySettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<OcUtilitySettings>("OcUtility Settings");
#if UNITY_EDITOR
                    if (_instance == null)
                    {
                        _instance = CreateInstance<OcUtilitySettings>();
                        AssetDatabase.CreateAsset(_instance, "Assets/Resources/OcUtility Settings.asset");
                    }
#endif
                }
                return _instance;
            }
        }
        static OcUtilitySettings _instance;
        public int RaycastBudget = 32;
        public int OverlapBudget = 32;
    }
}