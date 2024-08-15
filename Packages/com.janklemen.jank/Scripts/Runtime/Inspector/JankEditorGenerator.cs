#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Jank.Utilities.Paths;
using Jank.Utilities.Reflection;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Jank.Inspector.CustomEditorGenerator
{
    public static class JankEditorGenerator
    {
        // [InitializeOnLoadMethod]
        // public static void RegisterCompilationCallbacks()
        // {
        //     CompilationPipeline.compilationStarted += Generate;
        //     
        //     // If you try to run GenerateCustomEditor during InitializeOnLoadMethod it'll save files in the wrong place.
        //     // It's as if the Package/ don't exist yet when this run. 
        //     // GenerateCustomEditors();
        //     
        //     static void Generate(object obj)
        //     {
        //         GenerateCustomEditors();
        //         CompilationPipeline.compilationStarted -= Generate;
        //     }
        // }
        
        [MenuItem("Janklemen/Tools/GenerateEditors")]
        static void GenerateCustomEditorsForce() => GenerateCustomEditors(true);

        
        static void GenerateCustomEditors(bool force = false)
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesWithAttribute<JankCustomEditorAttribute>();

            foreach (Type type in types)
            {
                string assemblyName = type.Assembly.GetName().Name;
                string[] assemblies = AssetDatabase.FindAssets($"t:asmdef {assemblyName}");

                // Search for the assembly and grab the correctly named one. Breaks if naming the name in the asmdef
                // isn't the same as the name of the .asmdef file.
                string assemblyPathEditor = null;
                string assemblyPath = null;
                
                foreach (string assembly in assemblies)
                {
                    string candidatePath = AssetDatabase.GUIDToAssetPath(assembly);

                    if (candidatePath.EndsWith($"{assemblyName}.Editor.asmdef"))
                        assemblyPathEditor = candidatePath;

                    if (candidatePath.EndsWith($"{assemblyName}.asmdef"))
                    {
                        assemblyPath = candidatePath;
                        
                        // It's possible that the editor and non-editor assemblies are just the editor assembly because
                        // the thing with the custom editor is in an editor assembly. This is a special case where you
                        // just need to use the editor assembly for both things
                        if (assemblyName.EndsWith($"Editor"))
                        {
                            assemblyPathEditor = candidatePath;
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(assemblyPathEditor) && !string.IsNullOrEmpty(assemblyPath))
                        break;
                }

                if (string.IsNullOrEmpty(assemblyPathEditor))
                {
                    Debug.LogWarning($"[{nameof(GenerateCustomEditors)} Generate Error] Failed to find {assemblyName}.Editor.asmdef when trying to generate files for type {type.FullName}");
                    continue;
                }
                
                if (string.IsNullOrEmpty(assemblyPath))
                {
                    Debug.LogWarning($"[{nameof(GenerateCustomEditors)} Generate Error] Failed to find {assemblyName}.asmdef when trying to generate files for type {type.FullName}");
                    continue;
                }

                // Determine the path to generated files and force the folder to exist
                PathString customEditorFolderPathEditor 
                    = new(Path.GetDirectoryName(assemblyPathEditor), "Generated", "CustomEditors");
                customEditorFolderPathEditor.CreateIfNotExistsDirectory();
                
                PathString customEditorFolderPath = 
                    Path.Combine(Path.GetDirectoryName(assemblyPath), "Generated", "CustomEditors");
                customEditorFolderPath.CreateIfNotExistsDirectory();
                
                // Determine the path to this types generated files
                PathString customEditorFilePathEditor = Path.Combine(customEditorFolderPathEditor, $"{type.Name}.jankeditor.editor.g.cs");
                PathString customEditorFilePath = Path.Combine(customEditorFolderPath, $"{type.Name}.jankeditor.runtime.g.cs");
                
                // Determine the types to process
                Stack<Type> typesToProcess = new();
                typesToProcess.Push(type);

                while (typesToProcess.Peek().BaseType != typeof(MonoBehaviour) && typesToProcess.Peek().BaseType != typeof(object))
                    typesToProcess.Push(typesToProcess.Peek().BaseType);
                
                // Extract Members into a list in order
                List<MemberInfo> membersToProcess = new();
                
                while (typesToProcess.Count > 0)
                {
                    Type toProcess = typesToProcess.Pop();

                    IEnumerable<MemberInfo> members = toProcess
                        .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(member => member.DeclaringType == toProcess);
                    
                    membersToProcess.AddRange(members);
                }

                membersToProcess = membersToProcess.OrderBy(m => m.MetadataToken).ToList();
                
                // Get the generation package for all members
                Dictionary<MemberInfo, IMemberHandler> membersToGenerate = new();

                foreach (MemberInfo member in membersToProcess)
                {
                    if (member.GetCustomAttributes().Any(a => a is JankInspectIgnoreAttribute))
                        continue;

                    if (UTMemberHandler.TryGetMemberHandler(member, out IMemberHandler handler))
                        membersToGenerate[member] = handler;
                }

                // Determine the hash of this file and check against the existing file. This will determine if
                // generation should occur at all
                bool shouldGenerate = true;

                int hash = 0;
                hash = membersToGenerate.Aggregate(hash,
                    (lst, nxt) => lst + nxt.Value.ProvideMemberHash(nxt.Key));

                if (!force && File.Exists(customEditorFilePathEditor))
                {
                    using StreamReader reader = new(customEditorFilePathEditor);
                    string firstLine = reader.ReadLine();

                    // The first line of a valid generated file will be
                    // "// [${hash}]: Hash generated by JankCustomEditor"
                    // THe hash needs extracting to test so we don't constantly regenerate

                    int hashFirstIndex = firstLine.IndexOf("[");
                    int hashLastIndex = firstLine.IndexOf("]");

                    if (hashFirstIndex != -1 || hashLastIndex != -1)
                    {
                        string hashString = firstLine.Substring(hashFirstIndex, hashLastIndex - hashFirstIndex);

                        if (int.TryParse(hashString, out int cachedHash))
                        {
                            if (hash == cachedHash)
                                shouldGenerate = false;
                        }
                    }
                }

                if (!shouldGenerate)
                    continue;

                // Generate the strings for different segements of the file
                string BuildInspectorString(IEnumerable<string> strings)
                {
                    StringBuilder sb = new();

                    foreach (string se in strings)
                    {
                        if (!string.IsNullOrEmpty(se))
                            sb.AppendLine(se);
                    }

                    return sb.ToString();
                }
                
                // Build partial members
                string partialMembers = BuildInspectorString(membersToGenerate.Select(mem => mem.Value.Generate(mem.Key, IMemberHandler.GenerationEvent.RuntimeMembers)));

                // Build OnAfterDeserialize members
                string partialOnAfterDeserialize =
                    BuildInspectorString(membersToGenerate.Select(mem => mem.Value.Generate(mem.Key, IMemberHandler.GenerationEvent.RuntimeOnAfterDeserialize)));

                // Generate the class string
                string className = type.Name;
                
                string classGeneratedSourceCode = $@"// [{hash}]: Hash generated by JankCustomEditor

using Jank.Inspector.CustomEditorGenerator;
using Jank.Objects;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace {type.Namespace} 
{{
    public partial class {className} : ISerializationCallbackReceiver
    {{
        {partialMembers}

        public void OnBeforeSerialize() {{
        
        }}

        public void OnAfterDeserialize() {{
            {partialOnAfterDeserialize}
        }}
    }}
}}
";
                
                File.WriteAllBytes(customEditorFilePath, Encoding.UTF8.GetBytes(classGeneratedSourceCode));
                
                AssetDatabase.Refresh();
            }
        }
    }
}
#endif