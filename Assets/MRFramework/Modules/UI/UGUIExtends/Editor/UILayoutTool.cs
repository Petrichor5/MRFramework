using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MRFramework
{
    public class UILayoutTool
    {
        [Obsolete("Obsolete")]
        public static void OptimizeBatchForMenu()
        {
            OptimizeBatch(Selection.activeTransform);
        }
        
        [Obsolete("Obsolete")]
        public static void OptimizeBatch(Transform trans)
        {
            if (trans == null)
                return;
            Dictionary<string, List<Transform>> imageGroup = new Dictionary<string, List<Transform>>();
            Dictionary<string, List<Transform>> textGroup = new Dictionary<string, List<Transform>>();
            List<List<Transform>> sortedImgageGroup = new List<List<Transform>>();
            List<List<Transform>> sortedTextGroup = new List<List<Transform>>();
            for (int i = 0; i < trans.childCount; i++)
            {
                Transform child = trans.GetChild(i);
                Texture cur_texture = null;
                Image img = child.GetComponent<Image>();
                if (img != null)
                {
                    cur_texture = img.mainTexture;
                }
                else
                {
                    RawImage rimg = child.GetComponent<RawImage>();
                    if (rimg != null)
                        cur_texture = rimg.mainTexture;
                }
                if (cur_texture != null)
                {
                    string cur_path = AssetDatabase.GetAssetPath(cur_texture);
                    TextureImporter importer = AssetImporter.GetAtPath(cur_path) as TextureImporter;
                    //Debug.Log("cur_path : " + cur_path + " importer:"+(importer!=null).ToString());
                    if (importer != null)
                    {
                        if (importer.spritePackingTag != null)
                        {
                            string atlas = importer.spritePackingTag;
                            //Debug.Log("atlas : " + atlas);
                            if (atlas != "")
                            {
                                if (!imageGroup.ContainsKey(atlas))
                                {
                                    List<Transform> list = new List<Transform>();
                                    sortedImgageGroup.Add(list);
                                    imageGroup.Add(atlas, list);
                                }
                                imageGroup[atlas].Add(child);
                            }
                        }
                    }
                }
                else
                {
                    Text text = child.GetComponent<Text>();
                    if (text != null)
                    {
                        string fontName = text.font.name;
                        //Debug.Log("fontName : " + fontName);
                        if (!textGroup.ContainsKey(fontName))
                        {
                            List<Transform> list = new List<Transform>();
                            sortedTextGroup.Add(list);
                            textGroup.Add(fontName, list);
                        }
                        textGroup[fontName].Add(child);
                    }
                }
                OptimizeBatch(child);
            }
            //同一图集的Image间层级顺序继续保留,不同图集的顺序就按每组第一张的来
            for (int i = sortedImgageGroup.Count - 1; i >= 0; i--)
            {
                List<Transform> children = sortedImgageGroup[i];
                for (int j = children.Count - 1; j >= 0; j--)
                {
                    children[j].SetAsFirstSibling();
                }

            }
            foreach (var item in sortedTextGroup)
            {
                List<Transform> children = item;
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].SetAsLastSibling();
                }
            }
        }
    }
}