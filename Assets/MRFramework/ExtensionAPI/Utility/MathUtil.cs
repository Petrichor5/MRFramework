using UnityEngine;

public class MathUtil
{
    #region �ǶȺͻ���
    /// <summary>
    /// �Ƕ�ת���ȵķ���
    /// </summary>
    /// <param name="deg">�Ƕ�ֵ</param>
    /// <returns>����ֵ</returns>
    public static float Deg2Rad(float deg)
    {
        return deg * Mathf.Deg2Rad;
    }

    /// <summary>
    /// ����ת�Ƕȵķ���
    /// </summary>
    /// <param name="rad">����ֵ</param>
    /// <returns>�Ƕ�ֵ</returns>
    public static float Rad2Deg(float rad)
    {
        return rad * Mathf.Rad2Deg;
    }
    #endregion

    #region ���������ص�
    /// <summary>
    /// ��ȡXZƽ���� ����ľ���
    /// </summary>
    /// <param name="srcPos">��1</param>
    /// <param name="targetPos">��2</param>
    /// <returns></returns>
    public static float GetObjDistanceXZ(Vector3 srcPos, Vector3 targetPos)
    {
        srcPos.y = 0;
        targetPos.y = 0;
        return Vector3.Distance(srcPos, targetPos);
    }

    /// <summary>
    /// �ж�����֮����� �Ƿ�С�ڵ���Ŀ����� XZƽ��
    /// </summary>
    /// <param name="srcPos">��1</param>
    /// <param name="targetPos">��2</param>
    /// <param name="dis">����</param>
    /// <returns></returns>
    public static bool CheckObjDistanceXZ(Vector3 srcPos, Vector3 targetPos, float dis)
    {
        return GetObjDistanceXZ(srcPos, targetPos) <= dis;
    }

    /// <summary>
    /// ��ȡXYƽ���� ����ľ���
    /// </summary>
    /// <param name="srcPos">��1</param>
    /// <param name="targetPos">��2</param>
    /// <returns></returns>
    public static float GetObjDistanceXY(Vector3 srcPos, Vector3 targetPos)
    {
        srcPos.z = 0;
        targetPos.z = 0;
        return Vector3.Distance(srcPos, targetPos);
    }

    /// <summary>
    /// �ж�����֮����� �Ƿ�С�ڵ���Ŀ����� XYƽ��
    /// </summary>
    /// <param name="srcPos">��1</param>
    /// <param name="targetPos">��2</param>
    /// <param name="dis">����</param>
    /// <returns></returns>
    public static bool CheckObjDistanceXY(Vector3 srcPos, Vector3 targetPos, float dis)
    {
        return GetObjDistanceXY(srcPos, targetPos) <= dis;
    }

    #endregion

    #region λ���ж����
    /// <summary>
    /// �ж���������ϵ�µ�ĳһ���� �Ƿ�����Ļ�ɼ���Χ��
    /// </summary>
    /// <param name="pos">��������ϵ�µ�һ�����λ��</param>
    /// <returns>����ڿɼ���Χ�ⷵ��true�����򷵻�false</returns>
    public static bool IsWorldPosOutScreen(Vector3 pos)
    {
        //����������תΪ��Ļ����
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        //�ж��Ƿ�����Ļ��Χ��
        if (screenPos.x >= 0 && screenPos.x <= Screen.width &&
            screenPos.y >= 0 && screenPos.y <= Screen.height)
            return false;
        return true;
    }

    /// <summary>
    /// �ж�ĳһ��λ�� �Ƿ���ָ�����η�Χ�ڣ�ע�⣺��������������������ǻ���ͬһ������ϵ�µģ�
    /// </summary>
    /// <param name="pos">�������ĵ�λ��</param>
    /// <param name="forward">�Լ����泯��</param>
    /// <param name="targetPos">Ŀ�����</param>
    /// <param name="radius">�뾶</param>
    /// <param name="angle">���εĽǶ�</param>
    /// <returns></returns>
    public static bool IsInSectorRangeXZ(Vector3 pos, Vector3 forward, Vector3 targetPos, float radius, float angle)
    {
        pos.y = 0;
        forward.y = 0;
        targetPos.y = 0;
        //���� + �Ƕ�
        return Vector3.Distance(pos, targetPos) <= radius && Vector3.Angle(forward, targetPos - pos) <= angle / 2f;
    }
    #endregion
}
