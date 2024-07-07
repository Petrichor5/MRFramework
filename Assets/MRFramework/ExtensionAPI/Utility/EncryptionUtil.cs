using System;
using System.Text;
using Random = UnityEngine.Random;

/// <summary>
/// 加密工具类 主要提供加密需求
/// </summary>
public class EncryptionUtil
{
    private const int XOR_CONSTANT = 0xADAD;
    private const int SHIFT_VALUE = 5;
    
    /// <summary>
    /// 获取随机密钥
    /// </summary>
    public static int GetRandomKey()
    {
        return Random.Range(1, 10000) + 5;
    }

    #region int 类型
    
    public static int Encrypt(int value, int key)
    {
        value = value ^ (key % 9);
        value = value ^ XOR_CONSTANT;
        value = value ^ (1 << SHIFT_VALUE);
        value += key;
        return value;
    }
    
    public static int Decrypt(int value, int key)
    {
        if (value == 0)
            return value;
        value -= key;
        value = value ^ (1 << SHIFT_VALUE);
        value = value ^ XOR_CONSTANT;
        value = value ^ (key % 9);
        return value;
    }
    
    #endregion

    #region long 类型
    
    public static long Encrypt(long value, int key)
    {
        value = value ^ (key % 9);
        value = value ^ XOR_CONSTANT;
        value = value ^ (1 << SHIFT_VALUE);
        value += key;
        return value;
    }
    
    public static long Decrypt(long value, int key)
    {
        if (value == 0)
            return value;
        value -= key;
        value = value ^ (1 << SHIFT_VALUE);
        value = value ^ XOR_CONSTANT;
        value = value ^ (key % 9);
        return value;
    }

    #endregion

    #region string 类型
    
    public static string Encrypt(string value, int key)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        byte[] bytes = Encoding.UTF8.GetBytes(value);
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ (key % 9));
            bytes[i] = (byte)(bytes[i] ^ (XOR_CONSTANT & 0xFF)); // 只取低8位
            bytes[i] = (byte)(bytes[i] ^ (1 << SHIFT_VALUE));
            bytes[i] = (byte)(bytes[i] + key);
        }
        return Convert.ToBase64String(bytes);
    }
    
    public static string Decrypt(string value, int key)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        byte[] bytes = Convert.FromBase64String(value);
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] - key);
            bytes[i] = (byte)(bytes[i] ^ (1 << SHIFT_VALUE));
            bytes[i] = (byte)(bytes[i] ^ (XOR_CONSTANT & 0xFF)); // 只取低8位
            bytes[i] = (byte)(bytes[i] ^ (key % 9));
        }
        return Encoding.UTF8.GetString(bytes);
    }

    #endregion

    #region float 类型

    public static float Encrypt(float value, int key)
    {
        int intValue = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        int lockedIntValue = Encrypt(intValue, key);
        return BitConverter.ToSingle(BitConverter.GetBytes(lockedIntValue), 0);
    }

    public static float Decrypt(float value, int key)
    {
        int intValue = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        int unlockedIntValue = Decrypt(intValue, key);
        return BitConverter.ToSingle(BitConverter.GetBytes(unlockedIntValue), 0);
    }
    
    #endregion
}
