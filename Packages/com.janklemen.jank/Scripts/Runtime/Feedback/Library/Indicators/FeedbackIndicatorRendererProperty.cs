using Cysharp.Threading.Tasks;
using Jank.Feedback;
using Jank.Props.Utilities.ComponentParents;
using Jank.Utilities;
using UnityEngine;

namespace Jank
{
    [CreateAssetMenu(menuName = "Jank/Feedback/Indicator/RendererProperty")]
    public class FeedbackIndicatorRendererProperty : FeedbackImplementation
    {
        public enum EMode
        {
            MainColor,
            Emission,
            Custom
        }
        
        public EMode Mode;
        public string CustomColorShaderPropertyName;
        public string CustomColorShaderPropertyStrength;
        
        public Color Color;
        public float Strength;
        
        
        public override UniTask PerformFeedback(FeedbackEvent evt, GameObject caller)
        {
            RendererGroup renderers = caller.GetComponent<RendererGroup>();

            switch (Mode)
            {
                case EMode.MainColor:
                    renderers.Materials.ForEach(m => m.color = Color);
                    break;
                case EMode.Emission:
                    renderers.Materials.ForEach(m =>
                    {
                        m.SetColor("_EmissionColor", Color);
                        m.EnableKeyword("_EMISSION");
                    });
                    break;
                case EMode.Custom:
                    renderers.Materials.ForEach(m =>
                    {
                        if(!string.IsNullOrEmpty(CustomColorShaderPropertyName))
                            m.SetColor(CustomColorShaderPropertyName, Color);
                        
                        if(!string.IsNullOrEmpty(CustomColorShaderPropertyStrength))
                            m.SetFloat(CustomColorShaderPropertyStrength, Strength);
                    });
                    break;
            }
            
            return UniTask.CompletedTask;
        }
    }
}