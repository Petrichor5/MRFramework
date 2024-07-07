using System.IO;
using UnityEngine;

namespace MRFramework
{
    public static class ConfigSettings
    {
        /// <summary>
        /// excel文件存放的路径
        /// </summary>
        public static string EXCEL_PATH = Path.Combine(Directory.GetParent(Application.dataPath)!.FullName, "Excel");

        /// <summary>
        /// 映射关系脚本存储路径
        /// </summary>
        public static string DATA_CONFIGREGISTRY_PATH = Application.dataPath + "/MRFramework/Tools/Config/";
    
        /// <summary>
        /// 容器类脚本存储路径
        /// </summary>
        public static string DATA_CONTAINER_PATH = Application.dataPath + "/Scripts/TableContainer/";

        /// <summary>
        /// JSON数据存储路径
        /// </summary>
        public static string DATA_JSON_PATH = Application.dataPath + "/Scripts/TableData/";

        /// <summary>
        /// 真正内容开始的行号
        /// </summary>
        public static int BEGIN_INDEX = 4;
    }
}