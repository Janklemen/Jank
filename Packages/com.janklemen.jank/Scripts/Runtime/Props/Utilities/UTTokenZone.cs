using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.Props.Architecture;
using Jank.Props.Architecture.Token;
using Jank.Props.Architecture.Zone;
using Jank.Utilities;
using Jank.Utilities.DataStructures;

namespace Jank.Props.Utilities
{
    public static class UTTokenZone
    {
        /// <summary>
        /// This is designed to be used in tandem with the ListBehaviourBind function found in state. It sets up a
        /// very common, but very basic, way of handling state changes to a list that should instantiate tokens.
        /// </summary>
        public static async UniTask<IDisposable> BindTokensToZone<TPropData, TProp, TGroup>(
            this IZone zone,
            PropMap<TPropData, TProp> props,
            TGroup group,
            FuncAsync<
                ActionAsync<IndexedChange<TPropData>>, 
                ActionAsync<List<TPropData>>, 
                ActionAsync<IndexedValue<TPropData>>, 
                ActionAsync<IndexedValue<TPropData>>, 
                IDisposable> listBehaviorBind
        ) where TProp : AProp, IToken
        {
            return await listBehaviorBind(Set, Clear, InsertAt, RemoveAt);

            async UniTask Set(IndexedChange<TPropData> change)
            {
                await RemoveAt(new(change.OldValue, change.Index));
                await InsertAt(new(change.NewValue, change.Index));
            }
            
            async UniTask Clear(List<TPropData> cleared)
            {
                foreach (TPropData propData in cleared)
                    await zone.Release(await props.GetProp(propData, group));

                await props.RemoveProps(cleared, group);
            }
            
            async UniTask InsertAt(IndexedValue<TPropData> change)
            {
                TProp prop = await props.GetProp(change.Value, group);
                await zone.Accept(prop, change.Index);
            }
            
            async UniTask RemoveAt(IndexedValue<TPropData> change)
            {
                TProp prop = await props.GetProp(change.Value, group);
                await zone.Release(prop);
                await props.RemoveProp(change.Value, group);
            }
        }
    }
}