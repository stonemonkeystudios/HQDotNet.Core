using System.IO;
using UnityEditor;
using UnityEngine;

namespace HQDotNet.Unity.Editor {
    public class EditorTools {
        [MenuItem("HQDotNet/Export Package")]
        public static void ExportPackage() {
            string buildDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Release");

            if (!Directory.Exists(buildDir)) {
                Directory.CreateDirectory(buildDir);
            }

            string filePath =  Path.Combine(buildDir, "HQDotNet-Core.unitypackage");
            Debug.Log("Exporting package to " + filePath);

            AssetDatabase.ExportPackage("Assets/HQDotNet.Unity", filePath , ExportPackageOptions.Recurse);
        }
    }
}
