using System.Collections.Generic;
using System.Linq;
using Jank.Feedback;
using Jank.Observables.MouseEvents;
using Jank.Observables.Observables;
using Jank.Observables.Operators;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Props.Utilities.ComponentParents
{
    public class ColliderGroup : AComponentGroup<Collider>
    {
        public IObservableAsync<EStandardFeedbackEvent> MouseEventObservations()
        {
            if (!Components.Any())
                return EmptyObservableAsync<EStandardFeedbackEvent>.Instance;
            
            IEnumerable<IObservableAsync<EStandardFeedbackEvent>> observables = Components
                .Select(c => c.gameObject.GetOrAddComponent<MouseEventObserver>().Observations);

            IEnumerable<IObservableAsync<EStandardFeedbackEvent>> observableAsyncs = observables as IObservableAsync<EStandardFeedbackEvent>[] ?? observables.ToArray();
            
            if (observableAsyncs.Count() == 1)
                return observableAsyncs.First();

            return observableAsyncs.Merge();
        }

        public void EnableColliders() => Components.ForEach(t => t.enabled = true);
        public void DisableColliders() => Components.ForEach(t => t.enabled = false);
    }
}