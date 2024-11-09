/*******************************
 * Desc: 以下代码为导表工具自动化生成，请不要手动更改。
 * Date: 2024-11-10 07:37:40
 *******************************/ 

using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace Config
{
	public class Config_Unlock_Unlock
	{
		public EUnlockType UnlockType;
		public int ValueInt;
		public float ValueFloat;
		public bool ValueBool;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("UnlockType: " + UnlockType.ToString());
			sb.AppendLine("ValueInt: " + ValueInt.ToString());
			sb.AppendLine("ValueFloat: " + ValueFloat.ToString());
			sb.AppendLine("ValueBool: " + ValueBool.ToString());
			return sb.ToString();
		}
	}
}
