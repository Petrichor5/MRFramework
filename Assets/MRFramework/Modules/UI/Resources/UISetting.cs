using UnityEngine;

namespace MRFramework
{
    [CreateAssetMenu(menuName = "MRFramework/UISetting")]
    public class UISetting : ScriptableObject
    {
        private static UISetting instance;
        public static UISetting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<UISetting>("UISetting");
                }

                return instance;
            }
        }

        [Tooltip("是否启用单遮模式")]
        public bool SINGMASK_SYSTEM = true;
    }
}