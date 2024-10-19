using System;
using UnityEngine;
using UnityEngine.Events;

public class RayCastUtil
{
    #region 2D射线检测

    /// <summary>
    /// 射线检测 获取到多个对象 指定距离 指定层级
    /// </summary>
    /// <param name="origin">开始位置</param>
    /// <param name="direction">检测方向</param>
    /// <param name="maxDistance">检测距离</param>
    /// <param name="layerMask">检测层级</param>
    /// <param name="callBack">回调函数</param>
    /// <param name="maxCheckNum">最大检测数量</param>
    public static void RayCastAll2D(Vector2 origin, Vector2 direction, float maxDistance, int layerMask,
        UnityAction<GameObject> callBack,
        int maxCheckNum = 10)
    {
        RaycastHit2D[] hitInfos = new RaycastHit2D[maxCheckNum];
        int hitCount = Physics2D.RaycastNonAlloc(origin, direction, hitInfos, maxDistance, layerMask);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (hitInfos[i].collider != null && hitInfos[i].collider.gameObject != null)
                {
                    callBack.Invoke(hitInfos[i].collider.gameObject);
                }
            }
        }
    }

    #endregion

    #region 3D射线检测相关

    /// <summary>
    /// 射线检测 获取一个对象 指定距离 指定层级的
    /// </summary>
    /// <param name="ray">射线</param>
    /// <param name="callBack">回调函数（会把碰到的RayCastHit信息传递出去）</param>
    /// <param name="maxDistance">最大距离</param>
    /// <param name="layerMask">层级筛选</param>
    public static void RayCast(Ray ray, UnityAction<RaycastHit> callBack, float maxDistance, int layerMask)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, maxDistance, layerMask))
        {
            if (hitInfo.collider != null)
            {
                callBack.Invoke(hitInfo);
            }
        }
    }

    /// <summary>
    /// 射线检测 获取一个对象 指定距离 指定层级的
    /// </summary>
    /// <param name="ray">射线</param>
    /// <param name="callBack">回调函数（会把碰到的GameObject信息传递出去）</param>
    /// <param name="maxDistance">最大距离</param>
    /// <param name="layerMask">层级筛选</param>
    public static void RayCast(Ray ray, UnityAction<GameObject> callBack, float maxDistance, int layerMask)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, maxDistance, layerMask))
        {
            if (hitInfo.collider != null)
            {
                callBack.Invoke(hitInfo.collider.gameObject);
            }
        }
    }

    /// <summary>
    /// 射线检测 获取一个对象 指定距离 指定层级的
    /// </summary>
    /// <param name="ray">射线</param>
    /// <param name="callBack">回调函数（会把碰到的对象信息上挂在的指定脚本传递出去）</param>
    /// <param name="maxDistance">最大距离</param>
    /// <param name="layerMask">层级筛选</param>
    public static void RayCast<T>(Ray ray, UnityAction<T> callBack, float maxDistance, int layerMask)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, maxDistance, layerMask))
        {
            if (hitInfo.collider != null)
            {
                callBack.Invoke(hitInfo.collider.gameObject.GetComponent<T>());
            }
        }
    }

    /// <summary>
    /// 射线检测 获取到多个对象 指定距离 指定层级
    /// </summary>
    /// <param name="ray">射线</param>
    /// <param name="callBack">回调函数（会把碰到的RayCastHit信息传递出去） 每一个对象都会调用一次</param>
    /// <param name="maxDistance">最大距离</param>
    /// <param name="layerMask">层级筛选</param>
    /// <param name="maxCheckNum">最大检测数量</param>
    public static void RayCastAll3D(Ray ray, UnityAction<RaycastHit> callBack, float maxDistance, int layerMask,
        int maxCheckNum = 10)
    {
        RaycastHit[] hitInfos = new RaycastHit[maxCheckNum];
        int hitCount = Physics.RaycastNonAlloc(ray, hitInfos, maxDistance, layerMask);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (hitInfos[i].collider != null)
                {
                    callBack.Invoke(hitInfos[i]);
                }
            }
        }
    }

    /// <summary>
    /// 射线检测 获取到多个对象 指定距离 指定层级
    /// </summary>
    /// <param name="ray">射线</param>
    /// <param name="callBack">回调函数（会把碰到的GameObject信息传递出去） 每一个对象都会调用一次</param>
    /// <param name="maxDistance">最大距离</param>
    /// <param name="layerMask">层级筛选</param>
    /// <param name="maxCheckNum">最大检测数量</param>
    public static void RayCastAll(Ray ray, UnityAction<GameObject> callBack, float maxDistance, int layerMask,
        int maxCheckNum = 10)
    {
        RaycastHit[] hitInfos = new RaycastHit[maxCheckNum];
        int hitCount = Physics.RaycastNonAlloc(ray, hitInfos, maxDistance, layerMask);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (hitInfos[i].collider != null)
                {
                    callBack.Invoke(hitInfos[i].collider.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// 射线检测 获取到多个对象 指定距离 指定层级
    /// </summary>
    /// <param name="ray">射线</param>
    /// <param name="callBack">回调函数（会把碰到的对象信息上依附的脚本传递出去） 每一个对象都会调用一次</param>
    /// <param name="maxDistance">最大距离</param>
    /// <param name="layerMask">层级筛选</param>
    /// <param name="maxCheckNum">最大检测数量</param>
    public static void RayCastAll<T>(Ray ray, UnityAction<T> callBack, float maxDistance, int layerMask,
        int maxCheckNum = 10)
    {
        RaycastHit[] hitInfos = new RaycastHit[maxCheckNum];
        int hitCount = Physics.RaycastNonAlloc(ray, hitInfos, maxDistance, layerMask);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (hitInfos[i].collider != null)
                {
                    callBack.Invoke(hitInfos[i].collider.gameObject.GetComponent<T>());
                }
            }
        }
    }

    #endregion

    #region 2D范围检测相关

    /// <summary>
    /// 2D圆形范围检测
    /// </summary>
    /// <typeparam name="T">想要获取的信息类型 可以填写 Collider GameObject 以及对象上依附的组件类型</typeparam>
    /// <param name="center">球体的中心点</param>
    /// <param name="radius">球体的半径</param>
    /// <param name="layerMask">层级筛选</param>
    /// <param name="callBack">回调函数</param>
    /// <param name="maxCheckNum">最大检测数量</param>
    public static void OverlapCircle<T>(Vector3 center, float radius, LayerMask layerMask, UnityAction<T> callBack,
        int maxCheckNum = 10) where T : class
    {
        Type type = typeof(T);
        Collider2D[] colliders = new Collider2D[maxCheckNum];
        int numColliders = Physics2D.OverlapCircleNonAlloc(center, radius, colliders, layerMask);

        for (int i = 0; i < numColliders; i++)
        {
            Collider2D collider = colliders[i];
            if (collider == null) continue;

            T result = null;

            if (type == typeof(Collider2D))
                result = collider as T;
            else if (type == typeof(GameObject))
                result = collider.gameObject as T;
            else
                result = collider.gameObject.GetComponent<T>();

            if (result != null)
                callBack.Invoke(result);
        }
    }

    #endregion

    #region 3D范围检测相关

    /// <summary>
    /// 3D盒状范围检测
    /// </summary>
    /// <typeparam name="T">想要获取的信息类型 可以填写 Collider GameObject 以及对象上依附的组件类型</typeparam>
    /// <param name="center">盒装中心点</param>
    /// <param name="rotation">盒子的角度</param>
    /// <param name="halfExtents">长宽高的一半</param>
    /// <param name="layerMask">层级筛选</param>
    /// <param name="callBack">回调函数 </param>
    /// <param name="maxCheckNum">最大检测数量</param>
    public static void OverlapBox<T>(Vector3 center, Quaternion rotation, Vector3 halfExtents, int layerMask,
        UnityAction<T> callBack, int maxCheckNum = 10) where T : class
    {
        Type type = typeof(T);
        Collider[] colliders = new Collider[maxCheckNum];
        int numColliders = Physics.OverlapBoxNonAlloc(center, halfExtents, colliders, rotation, layerMask,
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = colliders[i];
            if (collider == null) continue;

            T result = null;

            if (type == typeof(Collider))
                result = collider as T;
            else if (type == typeof(GameObject))
                result = collider.gameObject as T;
            else
                result = collider.gameObject.GetComponent<T>();

            if (result != null)
                callBack.Invoke(result);
        }
    }

    /// <summary>
    /// 3D球体范围检测
    /// </summary>
    /// <typeparam name="T">想要获取的信息类型 可以填写 Collider GameObject 以及对象上依附的组件类型</typeparam>
    /// <param name="center">球体的中心点</param>
    /// <param name="radius">球体的半径</param>
    /// <param name="layerMask">层级筛选</param>
    /// <param name="callBack">回调函数</param>
    /// <param name="maxCheckNum">最大检测数量</param>
    public static void OverlapSphere<T>(Vector3 center, float radius, int layerMask, UnityAction<T> callBack,
        int maxCheckNum = 10) where T : class
    {
        Type type = typeof(T);
        Collider[] colliders = new Collider[maxCheckNum];
        int numColliders =
            Physics.OverlapSphereNonAlloc(center, radius, colliders, layerMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = colliders[i];
            if (collider == null) continue;

            T result = null;

            if (type == typeof(Collider))
                result = collider as T;
            else if (type == typeof(GameObject))
                result = collider.gameObject as T;
            else
                result = collider.gameObject.GetComponent<T>();

            if (result != null)
                callBack.Invoke(result);
        }
    }

    #endregion
}