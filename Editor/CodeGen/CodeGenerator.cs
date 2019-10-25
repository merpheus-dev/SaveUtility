using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Subtegral.SaveUtility.CodeGen
{
    public static class CodeGenerator
    {
        private const string SwitchMenuName = "Subtegral/SaveUtility/Auto Generate Wrappers";
        private const string AutoGenerationPrefKey = "AutoWrapperGenerationEnabled";

        private static string templatePath = null;

        private static string TemplatePath
        {
            get
            {
                if (templatePath != null) return templatePath;

                var template = Resources.Load<TextAsset>("WrapperTemplate.cs");
                templatePath = AssetDatabase.GetAssetPath(template);
                return templatePath;
            }
        }

        #region Menu Items
        [DidReloadScripts]
        private static void AutoGenerate()
        {
            if (EditorPrefs.GetBool(AutoGenerationPrefKey))
                Generate();
        }

        [InitializeOnLoadMethod]
        private static void UpdateSwitchStatus()
        {
            if (!EditorPrefs.HasKey(AutoGenerationPrefKey))
                EditorPrefs.SetBool(AutoGenerationPrefKey, false);

            Menu.SetChecked(SwitchMenuName, EditorPrefs.GetBool(AutoGenerationPrefKey));
        }

        [MenuItem(SwitchMenuName)]
        public static void AutoGenerateSwitch()
        {
            EditorPrefs.SetBool(AutoGenerationPrefKey, !EditorPrefs.GetBool(AutoGenerationPrefKey));
            Menu.SetChecked(SwitchMenuName, EditorPrefs.GetBool(AutoGenerationPrefKey));
        } 
        #endregion

        [MenuItem("Subtegral/SaveUtility/Generate")]
        public static void Generate()
        {
            if (!Directory.Exists("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            if (!Directory.Exists("Assets/Resources/SaveUtility"))
                AssetDatabase.CreateFolder("Assets/Resources", "SaveUtility");

            var wrapperDatabase = Resources.Load<WrapperDatabase>("SaveUtility/WrapperDB");
            if (wrapperDatabase == null)
            {
                wrapperDatabase = ScriptableObject.CreateInstance<WrapperDatabase>();
                wrapperDatabase.wrapperNames = new List<string>();
                AssetDatabase.CreateAsset(wrapperDatabase, "Assets/Resources/SaveUtility/WrapperDB.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            var attributeUsage = FetchAttributeUsages();
            var scriptDatas = attributeUsage.ToList();
            var newClasses = scriptDatas.Select(x => x.DataClassName).Except(wrapperDatabase.wrapperNames);

            var obsoleteClasses =
                wrapperDatabase.wrapperNames.Except(scriptDatas.Select(x => x.DataClassName)).ToArray();
            for (var i = 0; i < obsoleteClasses.Count(); i++)
            {
                if (scriptDatas.ToList().Exists(x => x.DataClassName == obsoleteClasses[i]))
                {
                    var scriptData = scriptDatas.First(x => x.DataClassName == obsoleteClasses[i]);
                    File.Delete($"Assets/Resources/SaveUtility/{scriptData.DataClassName}Wrapper.cs");
                }
                wrapperDatabase.wrapperNames.Remove(obsoleteClasses[i]);   
            }

            foreach (var newClass in newClasses)
            {
                Debug.Log("<color='green'>New Save Wrapper Class " + newClass + "</color> generated.");
                var scriptData = scriptDatas.First(x => x.DataClassName == newClass);
                File.WriteAllText($"Assets/Resources/SaveUtility/{scriptData.DataClassName}Wrapper.cs",
                    BuildScriptContents(scriptData));
                wrapperDatabase.wrapperNames.Add(newClass);
            }

            AssetDatabase.Refresh();
        }

        private static IEnumerable<ScriptData> FetchAttributeUsages()
        {
            var scriptDataList = new List<ScriptData>();
#if UNITY_2019_2_OR_NEWER
            var typeCollection = TypeCache.GetTypesWithAttribute(typeof(SaveAttribute));
            foreach (var perType in typeCollection)
            {
                scriptDataList.Add(new ScriptData()
                {
                    DataClassName = perType.Name,
                    ServiceName = perType.GetCustomAttribute<SaveAttribute>().saveService.ToString(),
                    NameSpaceName = perType.Namespace
                });
            }
#else
            var monoBehaviourObjects = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass);
            try
            {
                foreach (var monoBehaviourObject in monoBehaviourObjects)
                {
                    var typeInfo = monoBehaviourObject.GetTypeInfo();
                    if (typeInfo.IsDefined(typeof(SaveAttribute), false))
                    {
                        scriptDataList.Add(new ScriptData
                        {
                            DataClassName = typeInfo.Name,
                            ServiceName = typeInfo.GetCustomAttribute<SaveAttribute>().saveService.ToString(),
                            NameSpaceName = typeInfo.Namespace
                        });
                    }
                }
            }
            catch (ReflectionTypeLoadException exception) {}
#endif
            return scriptDataList;
        }

        private static string BuildScriptContents(ScriptData scriptData)
        {
            var wrapperContents = File.ReadAllText(TemplatePath);

            var replace = wrapperContents.Replace("#SCRIPT_NAME#", scriptData.DataClassName + "Wrapper")
                .Replace("#SERVICE_NAME#", scriptData.ServiceName)
                .Replace("#DATA_CLASS_NAME#", scriptData.DataClassName);

            var stringBuilder = new StringBuilder();
            if (scriptData.DoesNameSpaceExists)
                stringBuilder.AppendLine($"using {scriptData.NameSpaceName};");
            stringBuilder.Append(replace);
            return stringBuilder.ToString();
        }
    }
}