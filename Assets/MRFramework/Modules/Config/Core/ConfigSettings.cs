using System.IO;
using UnityEngine;

namespace MRFramework
{
    public static class ConfigSettings
    {
        /// <summary>
        /// Global Excel文件存放的路径
        /// </summary>
        public static string GlobalExcelPath = Path.Combine(Directory.GetParent(Application.dataPath)!.FullName, "Excel/Global");
        
        /// <summary>
        /// Enum Excel文件存放的路径
        /// </summary>
        public static string EnumExcelPath = Path.Combine(Directory.GetParent(Application.dataPath)!.FullName, "Excel/Enum");
    
        /// <summary>
        /// 枚举类脚本文件输出路径
        /// </summary>
        public static string EnumOutputPath = Application.dataPath + "/Scripts/TableData/Enum/";
        
        /// <summary>
        /// 容器类脚本文件输出路径
        /// </summary>
        public static string ContainerOutputPath = Application.dataPath + "/Scripts/TableData/Global/";

        /// <summary>
        /// Json文件输出路径
        /// </summary>
        public static string JsonOutputPath = Application.dataPath + "/AssetPackage/Config/";

        /// <summary>
        /// Json数据加载路径
        /// </summary>
        public static string JsonLoadPath = "Assets/AssetPackage/Config/";

        /// <summary>
        /// 真正内容开始的行号
        /// </summary>
        public static int BeginIndex = 4;
        
        /// <summary>
        /// 枚举真正内容开始的行号
        /// </summary>
        public static int EnumBeginIndex = 2;

        /// <summary>
        /// Addressable 组名称
        /// </summary>
        public static string GroupName = "Config";
    }
}