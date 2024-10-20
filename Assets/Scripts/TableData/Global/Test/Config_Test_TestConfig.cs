/*******************************
 * Desc: 以下代码为导表工具自动化生成，请不要手动更改。
 * Date: 2024-10-20 21:35:13
 *******************************/ 

using System.Text;
using System.Collections.Generic;

namespace Config
{
	public class Config_Test_TestConfig
	{
		public int Id;
		public string Name;
		public int StorageCount;
		public string PrefabPath;
		public bool IsCommonOre;
		public float BuildPosX;
		public float BuildPosY;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Id: " + Id.ToString());
			sb.AppendLine("Name: " + Name.ToString());
			sb.AppendLine("StorageCount: " + StorageCount.ToString());
			sb.AppendLine("PrefabPath: " + PrefabPath.ToString());
			sb.AppendLine("IsCommonOre: " + IsCommonOre.ToString());
			sb.AppendLine("BuildPosX: " + BuildPosX.ToString());
			sb.AppendLine("BuildPosY: " + BuildPosY.ToString());
			return sb.ToString();
		}
	}
}
