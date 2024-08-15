using System.Diagnostics;
using System.IO;
using UnityEditor;

namespace Jank.Editor
{
    public class CustomEditorWindow : EditorWindow
    {
        [MenuItem("Assets/Open Source File", true)]
        public static bool TestingValidation()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return path.EndsWith(".fbx");
        }


        [MenuItem("Assets/Open Source File")]
        static void Testing()
        {
            string gDrivePath = "F:\\My Drive\\Janklemen\\Art";


            string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string fileExtension = Path.GetExtension(filePath);
            string sourceFile = Path.GetFileNameWithoutExtension(filePath);

            if (fileExtension == ".fbx")
            {
                string sourceFilePath = gDrivePath + Path.GetDirectoryName(filePath.Split("Art")[1]) + "\\" + sourceFile + ".blend";

                UnityEngine.Debug.Log(sourceFilePath);
                
                Process.Start(sourceFilePath);
            }
            else
            {
                UnityEngine.Debug.LogError("This is not an FBX, please select and FBX");
            }



        }
    }
}