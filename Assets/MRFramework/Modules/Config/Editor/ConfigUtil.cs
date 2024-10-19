using System.Data;
using System.Diagnostics;
using System.IO;

namespace MRFramework
{
    public class ConfigUtil
    {
        /// <summary>
        /// 获取Enum文件名称
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public static string GetEnumFileName(string excelFilePath, string tableName)
        {
            // // 生成JSON文件名，包含路径前缀（如果有的话）
            // string jsonFileName;
            // string relativePath = Path.GetDirectoryName(excelFilePath);
            //
            // if (relativePath == ConfigSettings.EnumExcelPath)
            // {
            //     // 文件在根目录时，使用表名作为文件名
            //     jsonFileName = $"Enum_{tableName}";
            // }
            // else
            // {
            //     // 文件在子目录时，使用相对路径作为前缀
            //     relativePath = relativePath?.Substring(ConfigSettings.EnumExcelPath.Length + 1)
            //         .Replace(Path.DirectorySeparatorChar, '_').Replace(Path.AltDirectorySeparatorChar, '_');
            //     jsonFileName = $"Enum_{relativePath}_{tableName}";
            // }
            //
            // return jsonFileName;

            return tableName;
        }
        
        /// <summary>
        /// 获取枚举类文件存储路径
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        public static string GetEnumOutputPath(string excelFilePath)
        {
            string relativePath = Path.GetDirectoryName(excelFilePath);
            string enumExcelPath = ConfigSettings.EnumExcelPath;
            enumExcelPath = enumExcelPath.Replace("/", "\\");

            string path = ConfigSettings.EnumOutputPath;
            if (relativePath != enumExcelPath)
            {
                // 文件在子目录时
                relativePath = relativePath?.Substring(enumExcelPath.Length + 1)
                    .Replace(Path.DirectorySeparatorChar, '_').Replace(Path.AltDirectorySeparatorChar, '_');
                path = Path.Combine(ConfigSettings.EnumOutputPath, relativePath);
            }

            return path;
        }
        
        /// <summary>
        /// 获取Josn文件名称
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public static string GetJsonFileName(string excelFilePath, string tableName)
        {
            // 生成JSON文件名，包含路径前缀（如果有的话）
            string jsonFileName;
            string relativePath = Path.GetDirectoryName(excelFilePath);
            string excelPath = ConfigSettings.GlobalExcelPath;
            excelPath = excelPath.Replace("/", "\\");

            if (relativePath == excelPath)
            {
                // 文件在根目录时，使用表名作为文件名
                jsonFileName = $"Config_{tableName}";
            }
            else
            {
                // 文件在子目录时，使用相对路径作为前缀
                relativePath = relativePath?.Substring(excelPath.Length + 1)
                    .Replace(Path.DirectorySeparatorChar, '_').Replace(Path.AltDirectorySeparatorChar, '_');
                jsonFileName = $"Config_{relativePath}_{tableName}";
            }

            return jsonFileName;
        }

        /// <summary>
        /// 获取容器类文件存储路径
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        public static string GetContainerOutputPath(string excelFilePath)
        {
            string relativePath = Path.GetDirectoryName(excelFilePath);
            string excelPath = ConfigSettings.GlobalExcelPath;
            excelPath = excelPath.Replace("/", "\\");

            string path = ConfigSettings.ContainerOutputPath;
            if (relativePath != excelPath)
            {
                // 文件在子目录时
                relativePath = relativePath?.Substring(excelPath.Length + 1)
                    .Replace(Path.DirectorySeparatorChar, '_').Replace(Path.AltDirectorySeparatorChar, '_');
                path = Path.Combine(ConfigSettings.ContainerOutputPath, relativePath);
            }

            return path;
        }

        /// <summary>
        /// 获取Josn文件存储路径
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        public static string GetJsonOutputPath(string excelFilePath)
        {
            string relativePath = Path.GetDirectoryName(excelFilePath);
            string excelPath = ConfigSettings.GlobalExcelPath;
            excelPath = excelPath.Replace("/", "\\");

            string path = ConfigSettings.JsonOutputPath;
            if (relativePath != excelPath)
            {
                // 文件在子目录时
                relativePath = relativePath?.Substring(excelPath.Length + 1)
                    .Replace(Path.DirectorySeparatorChar, '_').Replace(Path.AltDirectorySeparatorChar, '_');
                path = Path.Combine(ConfigSettings.JsonOutputPath, relativePath);
            }

            return path;
        }

        /// <summary>
        /// 获取变量名所在行
        /// </summary>
        public static DataRow GetVariableNameRow(DataTable table)
        {
            return table.Rows[0];
        }

        /// <summary>
        /// 获取变量类型所在行
        /// </summary>
        public static DataRow GetVariableTypeRow(DataTable table)
        {
            return table.Rows[1];
        }

        /// <summary>
        /// 获取主键索引
        /// </summary>
        public static int GetKeyIndex(DataTable table)
        {
            DataRow row = table.Rows[2];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (row[i].ToString() == "key")
                    return i;
            }

            return 0;
        }

        /// <summary>
        /// 获取枚举字段类型行
        /// </summary>
        public static DataRow GetEnumTypeRow(DataTable table)
        {
            return table.Rows[0];
        }
    }
}