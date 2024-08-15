#if UNITY_EDITOR
using Jank.Observables.Observers;
using UnityEditor;
using UnityEngine;
#endif
using Jank.Utilities;

namespace Jank.Observables.Subject
{
    public class MonoSubjectUnit : MonoSubject<Unit>
    {
        public static MonoSubjectUnit AddComponent(GameObject target, string eventName)
        {
            MonoSubjectUnit subject = target.AddComponent<MonoSubjectUnit>();
            subject.EventName = eventName;
            return subject;
        }

        public static void MakeSubjectIfNull(ref MonoSubjectUnit field, GameObject gameObject, string label)
        {
            if (field == null)
                field = MonoSubjectUnit.AddComponent(gameObject, label);
        }
            
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MonoSubjectUnit))]
    public class MonoSubjectUnitEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MonoSubjectUnit subject = (MonoSubjectUnit)target;

            // Draw default inspector
            base.OnInspectorGUI();

            RenderObserver(subject.Observer);
            
            void RenderObserver(IObserverAsync<Unit> observer)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField("Event",subject.EventName);
                
                if (observer is DisposedObserverAsync<Unit> || observer is EmptyObserverAsync<Unit> ||
                    observer is ThrowObserverAsync<Unit>)
                {
                    EditorGUILayout.LabelField("Empty");
                    return;
                }
            
                if (subject.OutObserver is ICompositeObserverAsync<Unit> observerAsync)
                {
                    ImmutableList<IObserverAsync<Unit>> observers = observerAsync.Observers;

                    foreach (IObserverAsync<Unit> innerObserver in observers.Data)
                        RenderObserver(innerObserver);
                    
                    return;
                }

                if (observer is IComponentReferencer igr)
                {
                    EditorGUILayout.ObjectField("Observer", igr.ComponentReference, igr.ComponentReference.GetType(), false);
                }
                EditorGUI.EndDisabledGroup();
            }
        }
    }
#endif
}