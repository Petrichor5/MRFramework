using UnityEngine;
using UnityEngine.UI;


namespace MRFramework.UGUIPro
{
    public class TextProMain : MonoBehaviour
    {
        public Sprite sprite;
        public ImagePro mImagePro;
        public Dropdown dropdown;
        public TextPro mTextPro;

        // Update is called once per frame
        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        mImagePro.sprite = sprite;
        //    }
        //    if (Input.GetKeyDown(KeyCode.S))
        //    {
        //        mTextPro.SetTextAlpha(0);
        //    }
        //    if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        mTextPro.SetOutLineColor(Color.green);
        //    }
        //}
    }
    //-----TextPro 功能介绍--------
    //  1.高性能描边OutLine 
    //  2.字符间距调整
    //  3.字符4个顶点颜色调整
    //  4.字符颜色渐变
    //  5.字符Shadow
    //	6.本地多语言
    //-----TextMeshPro 功能介绍--------
    //  1.本地多语言


    //-----ImagePro 功能介绍--------
    //  1.图片多语言支持  （Excel配置读取与转换）
    //  2.高性能Mask遮罩、裁剪

    //-----ButtonPro 功能介绍--------
    //  1.双击事件支持
    //  2.长按事件支持
}