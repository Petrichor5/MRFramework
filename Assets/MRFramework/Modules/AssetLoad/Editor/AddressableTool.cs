using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// Addressables 工具类
    /// </summary>
    public static class AddressableTool
    {
        /// <summary>
        /// 将资源标记为为可寻址
        /// </summary>
        /// <param name="groupName">Addressables 组名称</param>
        /// <param name="addressPath">资源路径</param>
        public static void MarkAssetAsAddressable(string groupName, string addressPath)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            // 找到指定的 Addressables 组
            var group = settings.FindGroup(groupName);
            if (group == null)
            {
                Debug.LogError($"未找到名为 {groupName} 的 Addressables 组");
                return;
            }

            // 创建或移动条目
            var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(addressPath), group);
            entry.SetAddress(addressPath);

            Debug.Log($"资源被标记为可寻址，地址为 {addressPath}");
        }
    }
}