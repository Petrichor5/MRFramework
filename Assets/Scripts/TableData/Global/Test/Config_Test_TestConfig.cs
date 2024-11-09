/*******************************
 * Desc: 以下代码为导表工具自动化生成，请不要手动更改。
 * Date: 2024-11-10 06:37:34
 *******************************/ 

using System.Text;
using UnityEngine;
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
		public int[] IntArrayTest;
		public string[] StringArrayTest;
		public float[] FloatArrayTest;
		public List<int> ListInt;
		public Vector2 BuildPos1;
		public Vector3 BuildPos2;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Id: " + Id.ToString());
			sb.AppendLine("Name: " + Name.ToString());
			sb.AppendLine("StorageCount: " + StorageCount.ToString());
			sb.AppendLine("PrefabPath: " + PrefabPath.ToString());
			sb.AppendLine("IsCommonOre: " + IsCommonOre.ToString());
			sb.AppendLine("IntArrayTest: " + IntArrayTest.ToString());
			sb.AppendLine("StringArrayTest: " + StringArrayTest.ToString());
			sb.AppendLine("FloatArrayTest: " + FloatArrayTest.ToString());
			sb.AppendLine("ListInt: " + ListInt.ToString());
			sb.AppendLine("BuildPos1: " + BuildPos1.ToString());
			sb.AppendLine("BuildPos2: " + BuildPos2.ToString());
			return sb.ToString();
		}
	}
}
