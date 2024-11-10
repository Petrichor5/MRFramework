/*******************************
 * Desc: 以下代码为导表工具自动化生成，请不要手动更改。
 * Date: 2024-11-10 20:08:15
 *******************************/ 

using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace Config
{
	public class Config_Unlock_Unlock
	{
		public EUnlockID EUnlockID;
		public EUnlockType EUnlockType;
		public string LockDesc;
		public int ValueInt;
		public float ValueFloat;
		public bool ValueBool;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("EUnlockID: " + EUnlockID.ToString());
			sb.AppendLine("EUnlockType: " + EUnlockType.ToString());
			sb.AppendLine("LockDesc: " + LockDesc.ToString());
			sb.AppendLine("ValueInt: " + ValueInt.ToString());
			sb.AppendLine("ValueFloat: " + ValueFloat.ToString());
			sb.AppendLine("ValueBool: " + ValueBool.ToString());
			return sb.ToString();
		}
	}
}
