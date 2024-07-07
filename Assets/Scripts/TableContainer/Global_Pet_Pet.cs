/*******************************
 * Desc: 以下代码为导表工具自动化生成，请不要手动更改。
 * Date: 2024-07-07 20:43:36
 *******************************/ 

using System.Text;
using System.Collections.Generic;

namespace MRFramework.Config
{
	public class Global_Pet_Pet_Container
	{
		public Dictionary<int, Global_Pet_Pet> DataDic = new Dictionary<int, Global_Pet_Pet>();

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Global_Pet_Pet: ");
			foreach (var item in DataDic)
			{
				sb.AppendLine(item.Key.ToString() + ": " + item.Value.ToString());
			}
			return sb.ToString();
		}
	}
}

public class Global_Pet_Pet
{
	public int Id;
	public string Name;
	public int Level;
	public int HP;
	public int Atk;
	public int Def;
	public int SpAtk;
	public int SpDef;
	public int Speed;

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("Id: " + Id.ToString());
		sb.AppendLine("Name: " + Name.ToString());
		sb.AppendLine("Level: " + Level.ToString());
		sb.AppendLine("HP: " + HP.ToString());
		sb.AppendLine("Atk: " + Atk.ToString());
		sb.AppendLine("Def: " + Def.ToString());
		sb.AppendLine("SpAtk: " + SpAtk.ToString());
		sb.AppendLine("SpDef: " + SpDef.ToString());
		sb.AppendLine("Speed: " + Speed.ToString());
		return sb.ToString();
	}
}
