using System.Collections.Generic;
using MRFramework;
using MRFramework.UGUIPro;
using UnityEngine;

/*
 *                        _oo0oo_
 *                       o8888888o
 *                       88" . "88
 *                       (| -_- |)
 *                       0\  =  /0
 *                     ___/`---'\___
 *                   .' \\|     |// '.
 *                  / \\|||  :  |||// \
 *                 / _||||| -:- |||||- \
 *                |   | \\\  - /// |   |
 *                | \_|  ''\---/''  |_/ |
 *                \  .-\__  '-'  ___/-. /
 *              ___'. .'  /--.--\  `. .'___
 *           ."" '<  `.___\_<|>_/___.' >' "".
 *          | | :  `- \`.;`\ _ /`;.`/ - ` : | |
 *          \  \ `_.   \_ __\ /__ _/   .-` /  /
 *      =====`-.____`.___ \_____/___.-`___.-'=====
 *                        `=---='
 *
 *
 *      ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *
 *                佛祖保佑你的代码永无内存泄露
 */

public class AssetLoadDemo : MonoBehaviour
{
    public ImagePro TestImage;
    private string m_SpriteKey = "Icon/01.png";

    private string m_GameObjectKey = "Assets/MRFrameworkDemo/AssetLoad/TestGameObject.prefab";
    private List<GameObject> m_GameObjectList = new List<GameObject>();

    public void Instantiate()
    {
        GameObject go = AssetManager.Instance.Instantiate(m_GameObjectKey);
        m_GameObjectList.Add(go);
    }

    public void ReleaseInstance()
    {
        if (m_GameObjectList.Count > 0)
        {
            int index = m_GameObjectList.Count - 1;
            AssetManager.Instance.ReleaseInstance(m_GameObjectList[index]);
            m_GameObjectList.RemoveAt(index);
        }
    }

    public void ReleaseAllInstance()
    {
        foreach (GameObject go in m_GameObjectList)
        {
            AssetManager.Instance.ReleaseInstance(go);
        }
        m_GameObjectList.Clear();
    }

    public void SetSprite()
    {
        TestImage.SetSprite(m_SpriteKey);
    }

    public void ReleaseSprite()
    {
        TestImage.ReleaseSprite();
    }
}