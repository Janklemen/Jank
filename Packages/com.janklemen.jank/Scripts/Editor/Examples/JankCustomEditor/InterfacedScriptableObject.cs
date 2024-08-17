using UnityEngine;

namespace Jank.Editor.Examples
{
    public interface IExample
    {
        
    }
    
    [CreateAssetMenu(fileName = "InterfacedScriptableObject",
        menuName = "Jank/Examples/JankCustomEditor/InterfacedScriptableObject")]
    public class InterfacedScriptableObject : ScriptableObject, IExample
    {
    }
}