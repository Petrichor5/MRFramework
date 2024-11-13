using System.IO;
using UnityEngine;

namespace MRFramework.UGUIPro
{
    public static class LocalizationSettings
    {
        /// <summary>
        /// Excel文件读取路径
        /// </summary>
        public static string ExcelLaodPath = Path.Combine(Directory.GetParent(Application.dataPath)!.FullName, "Excel/");

        /// <summary>
        /// Excel文件名称
        /// </summary>
        public static string ExcelFileName = "Localization.xlsx";
        
        /// <summary>
        /// 多语言配置文件输出路径
        /// </summary>
        public static string OutPutPath = Application.dataPath + "/AssetPackage/Config/Localization/";
        
        /// <summary>
        /// 多语言配置文件相对项目路径
        /// </summary>
        public static string ResPath = "Assets/AssetPackage/Config/Localization/";
    }
}