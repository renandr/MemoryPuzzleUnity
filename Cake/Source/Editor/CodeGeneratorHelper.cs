using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using UnityEngine;

namespace GGS.CakeBox
{
    /// <summary>
    /// Helper functions to generate code
    /// </summary>
    public static class CodeGeneratorHelper
    {
        /// <summary>
        /// Generate a class
        /// </summary>
        /// <param name="ccu">CCU</param>
        /// <param name="filename">file name for the class</param>
        public static void SaveToFile(CodeCompileUnit ccu, string pathName, string fileName)
        {
            string fullName = pathName + fileName;
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions
            {
                BracingStyle = "C",
                BlankLinesBetweenMembers = false
            };

            using (var sourceWriter = new StringWriter())
            {
                provider.GenerateCodeFromCompileUnit(ccu, sourceWriter, options);
                Debug.Log("Generated '" + fullName + "':\n " + sourceWriter);
            }

            if (!Directory.Exists(pathName))
                Directory.CreateDirectory(pathName);

            using (var sourceWriter = new StreamWriter(fullName))
            {
                provider.GenerateCodeFromCompileUnit(ccu, sourceWriter, options);
            }
        }

        /// <summary>
        /// Generate a string constant and return it
        /// </summary>
        /// <param name="name">Name of the constant</param>
        /// <param name="value">Value of the constant</param>
        /// <returns>The constant</returns>
        public static CodeMemberField GenerateStringConstant(string name, string value)
        {
            CodeMemberField codeMemberField = new CodeMemberField(typeof(string), name)
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Const,
                InitExpression = new CodeSnippetExpression('"' + value + '"')
            };
            return codeMemberField;
        }
    }
}