/*******************************
 * Desc: 以下代码为导表工具自动化生成，请不要手动更改。
 * Date: 2024-07-07 20:43:37
 *******************************/ 

using System;
using System.Collections.Generic;

public static class ConfigRegistry
{
	private static readonly Dictionary<string, Type> mConfigTypeMap = new Dictionary<string, Type>();

	public static Type GetConfigType(string configName)
	{
		if (mConfigTypeMap.TryGetValue(configName, out var type))
		{
			return type;
		}
		throw new ArgumentException($"Config type not found for {{configName}}", nameof(configName));
	}

	private static void RegisterConfig(string configName, Type type)
	{
		mConfigTypeMap.TryAdd(configName, type);
	}

	static ConfigRegistry()
	{
		RegisterConfig("Global_Pet_Pet", typeof(Dictionary<int, Global_Pet_Pet>));
	}
}
