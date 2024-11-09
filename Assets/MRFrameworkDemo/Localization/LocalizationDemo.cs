using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationDemo : MonoBehaviour
{
    //public List<string> ScanLocalizationTextFromDataTables(Action<string, int, int> onProgressUpdate = null)
    //{
    //    List<string> keyList = new List<string>();

    //    var tbFullFiles = GameDataGenerator.GetGameDataExcelWithABFiles(GameDataType.DataTable, mainTbFullFiles);//同时扫描AB测试表
    //    for (int i = 0; i < tbFullFiles.Length; i++)
    //    {
    //        var excelFile = tbFullFiles[i];
    //        var fileInfo = new FileInfo(excelFile);
    //        if (!fileInfo.Exists) continue;

    //        onProgressUpdate?.Invoke(excelFile, tbFullFiles.Length, i);
    //        string tmpExcelFile = UtilityBuiltin.ResPath.GetCombinePath(fileInfo.Directory.FullName, GameFramework.Utility.Text.Format("{0}.temp", fileInfo.Name));
    //        try
    //        {
    //            File.Copy(excelFile, tmpExcelFile, true);
    //            using (var excelPackage = new ExcelPackage(tmpExcelFile))
    //            {
    //                var excelSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
    //                if (excelSheet.Dimension.End.Row >= 4)
    //                {
    //                    for (int colIndex = excelSheet.Dimension.Start.Column; colIndex <= excelSheet.Dimension.End.Column; colIndex++)
    //                    {
    //                        if (excelSheet.GetValue<string>(4, colIndex)?.ToLower() != EXCEL_I18N_TAG)
    //                        {
    //                            continue;
    //                        }
    //                        for (int rowIndex = 5; rowIndex <= excelSheet.Dimension.End.Row; rowIndex++)
    //                        {
    //                            string langKey = excelSheet.GetValue<string>(rowIndex, colIndex);
    //                            if (string.IsNullOrWhiteSpace(langKey) || keyList.Contains(langKey)) continue;
    //                            keyList.Add(langKey);
    //                        }
    //                    }

    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.LogError($"扫描数据表本地化文本失败!文件:{excelFile}, Error:{e.Message}");
    //        }

    //        if (File.Exists(tmpExcelFile))
    //        {
    //            File.Delete(tmpExcelFile);
    //        }
    //    }
    //    return keyList;
    //}
}
