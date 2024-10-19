using System.IO;
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;

namespace MRFramework
{
    public class CodeGenKit : ScriptableObject
    {
        public CodeGenTask Task;

        private static readonly Lazy<string> m_Dir =
            new Lazy<string>(() => "Assets/MRFramework/Modules/UI/".CreateDirIfNotExists());

        private const string m_FileName = "CodeGenPipeline.asset";

        private static CodeGenKit instance;

        public static CodeGenKit Instance
        {
            get
            {
                if (instance) return instance;

                var filePath = m_Dir.Value + m_FileName;

                if (File.Exists(filePath))
                {
                    return instance = AssetDatabase.LoadAssetAtPath<CodeGenKit>(filePath);
                }

                return instance = CreateInstance<CodeGenKit>();
            }
        }

        private void SaveData()
        {
            var filePath = m_Dir.Value + m_FileName;

            if (!File.Exists(filePath))
            {
                AssetDatabase.CreateAsset(this, m_Dir.Value + m_FileName);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        public void Generate(CodeGenTask task)
        {
            Task = task;
            CodeTemplate.GetTemplate(ref Task);
            SaveData();
            UIWindowEditor.ShowWindow();
        }

        /// <summary>
        /// 编译完成后系统自动调用
        /// </summary>
        [DidReloadScripts]
        private static void OnCompile()
        {
            string className = EditorPrefs.GetString("GeneratorClassName");
            if (string.IsNullOrEmpty(className))
            {
                return;
            }
            
            Debug.Log("[CodeGen] 编译生成脚本 ClassName：" + className);

            var generateClassName = Instance.Task.ScriptName;
            var generateNamespace = Instance.Task.ScriptNamespace;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly =>
                !assembly.FullName.StartsWith("Unity"));

            var typeName = generateNamespace + "." + generateClassName;

            var type = assemblies.Where(a => a.GetType(typeName) != null)
                .Select(a => a.GetType(typeName)).FirstOrDefault();

            if (type == null)
            {
                Debug.LogError("[CodeGen] 没有找到脚本 TypeName：" + typeName);
                return;
            }
            
            // 添加生成的脚本
            var scriptComponent = Instance.Task.Obj.GetComponent(type);
            if (!scriptComponent)
            {
                scriptComponent = Instance.Task.Obj.AddComponent(type);
            }

            // 为 ViewCode 引用组件
            var serializedObject = new SerializedObject(scriptComponent);
            foreach (var bindInfo in Instance.Task.Binds)
            {
                //根据InstanceID找到对应的对象
                GameObject Obj = EditorUtility.InstanceIDToObject(bindInfo.InstanceID) as GameObject;
                // 获取成员变量
                var serializedProperty = serializedObject.FindProperty(bindInfo.MemberName);
                // 为成员变量引用组件
                serializedProperty.objectReferenceValue = Obj;
            }

            // 应用属性修改
            serializedObject.ApplyModifiedProperties();
            // 更新序列化对象的表示形式
            serializedObject.UpdateIfRequiredOrScript();

            // 将指定场景标记为已修改
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());

            // 创建并保存预制体
            CreateAndSavePrefab(Instance.Task.Obj);

            EditorPrefs.DeleteKey("GeneratorClassName");
        }

        private static void CreateAndSavePrefab(GameObject obj)
        {
            // 构建资源路径
            string panelName = obj.name;
            string[] strs = StringUtil.SplitStr(panelName, 7);
            string resKey = "Assets\\AssetPackage\\UI";
            for (int i = 1; i < strs.Length - 1; i++)
            {
                resKey = Path.Combine(resKey, strs[i]);
            }
            resKey.CreateDirIfNotExists();
            resKey = Path.Combine(resKey, panelName + ".prefab");

            // 创建预制体
            PrefabUtility.SaveAsPrefabAssetAndConnect(obj, resKey, InteractionMode.UserAction);

            Debug.Log("面板预制体创建完成: " + resKey);
        }
    }
}