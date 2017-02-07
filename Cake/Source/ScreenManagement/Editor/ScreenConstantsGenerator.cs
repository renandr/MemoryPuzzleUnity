using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GGS.CakeBox;
using UnityEditor;
using UnityEngine;

namespace GGS.ScreenManagement
{
    public class ScreenConstantsGenerator : ScriptableObject
    {
        public const string ToolBarOption = "GGS/Screen Management/";

        private static string DialogPrefabFolder = @"Assets/Prefabs/UI/Dialogs";
        private static string PanelPrefabFolder = @"Assets/Prefabs/UI/Panels";
        private static string ComponentsPrefabFolder = @"Assets/Prefabs/UI/Components_Dynamic";

        private static string ClassPackage = "GGS.ScreenManagement";
        private static string ConstantsOutputPath = @"Assets/Source/Generated/";


        [MenuItem(ToolBarOption+"Generate all", false, 1)]
        public static void GenerateAllConstantClasses()
        {
            GenerateDialogConstants();
            GeneratePanelConstants();
            GenerateComponentsConstants();
        }

        [MenuItem(ToolBarOption + "Generate Dialog Info", false, 21)]
        private static void GenerateDialogConstants()
        {
            GenerateScreenConstants("DialogConstants", DialogPrefabFolder);
        }

        

        [MenuItem(ToolBarOption +"Generate Panel Info", false, 22)]
        private static void GeneratePanelConstants()
        {
            GenerateScreenConstants("PanelConstants", PanelPrefabFolder);
        }

        [MenuItem(ToolBarOption + "Generate Screen Components Info", false, 23)]
        private static void GenerateComponentsConstants()
        {
            GenerateScreenConstants("ScreenComponentConstants", ComponentsPrefabFolder);
        }

        private static List<string> GetConstantStrings(Type type)
        {
            var res = new List<string>();
            List<FieldInfo> fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
            foreach (FieldInfo field in fields)
            {
                res.Add((string)field.GetRawConstantValue());
            }
            return res;
        }

        private static void GenerateScreenConstants(string className, string pathName)
        {
            var ccu = new CodeCompileUnit();
            var codeNamespace = new CodeNamespace(ClassPackage);

            ccu.Namespaces.Add(codeNamespace);

            var codeTypeDeclaration = new CodeTypeDeclaration(className)
            {
                Attributes = MemberAttributes.Public,
                //IsPartial = true
            };
            codeTypeDeclaration.Comments.Add(new CodeCommentStatement(@"Contains constants for valid panels"));

            codeNamespace.Types.Add(codeTypeDeclaration);

            List<string> prefabs = GetPrefabList(pathName, "prefab", true);
            var prefabNames = new List<string>();
            foreach (string prefabPath in prefabs)
            {
                string pfPath = prefabPath.Replace('\\', '/').Replace("./", "");
                string prefabName = GetElementName(pfPath).Replace(".prefab", "");
                string constantName = GetConstantName(prefabName, false, false);
                if (prefabNames.Contains(prefabName))
                {
                    Debug.LogError("Duplicated prefab named " + prefabName);
                }

                var asset = AssetImporter.GetAtPath(pfPath);
                asset.assetBundleName = prefabName;
                prefabNames.Add(prefabName);
                codeTypeDeclaration.Members.Add(CodeGeneratorHelper.GenerateStringConstant(constantName, prefabName));
            }
            CodeGeneratorHelper.SaveToFile(ccu, ConstantsOutputPath,  className + ".cs");
        }

        private static string GetElementName(string resPath, string separator = "/")
        {
            string s = separator;
            int lastSlashIdx = resPath.LastIndexOf(s);
            return resPath.Substring(lastSlashIdx + 1);
        }

        private static string GetConstantName(string prefabName, bool convertToConstantConvetion, bool cutOfScenePrefix)
        {
            if (cutOfScenePrefix)
            {
                prefabName = prefabName.Substring(6, prefabName.Length - 6);
            }

            var result = new StringBuilder("" + char.ToUpper(prefabName[0]));
            for (var cIdx = 1; cIdx < prefabName.Length; ++cIdx)
            {
                var c = prefabName[cIdx];
                if (!char.IsLetterOrDigit(c))
                {
                    result.Append('_');
                }
                else
                {
                    if (convertToConstantConvetion)
                    {
                        if (char.IsLower(c))
                        {
                            result.Append(char.ToUpper(c));
                        }
                        else
                        {
                            result.Append(c);
                        }
                    }
                    else
                    {
                        result.Append(c);
                    }
                }
            }
            return result.ToString();
        }

        public static List<string> GetAllPrefabsInDirectory(string basePath)
        {
            List<string> prefabs = GetPrefabList(basePath, "prefab", false);
            for (int i = 0; i < prefabs.Count; i++)
            {
                prefabs[i] = prefabs[i].Replace('\\', '/');
            }
            return prefabs;
        }

        private static List<string> GetPrefabList(string basePath, string fileExtension, bool searchSubtrees)
        {
            var result = new List<string>();
            GetRecursivePrefabPaths(basePath, result, fileExtension, searchSubtrees);
            return result;
        }

        private static void GetRecursivePrefabPaths(string path, List<string> collectedPaths, string fileExtension,
                                                    bool serachRecursive)
        {
            if (Directory.Exists(path))
            {
                var filePaths = Directory.GetFiles(path, "*." + fileExtension);
                collectedPaths.AddRange(filePaths);

                if (serachRecursive)
                {
                    var directories = Directory.GetDirectories(path);
                    foreach (var p in directories)
                    {
                        GetRecursivePrefabPaths(p, collectedPaths, fileExtension, serachRecursive);
                    }
                }
            }
        }
    }
}