using Cysharp.Threading.Tasks;
using Jank.Inspector.CustomEditorGenerator;
using Jank.Objects;
using Jank.Props.Architecture.Zone;
using UnityEngine;

namespace Jank.Props.Architecture.Token
{
    public abstract class AToken<TData> : AStandardProp<TData>, IToken
    {
        protected override async UniTask OnLoadStandard(TData path)
        {
            await OnLoadToken(path);
        }

        protected abstract UniTask OnLoadToken(TData data);
        
        protected override async UniTask OnUnloadStandard(TData data)
        {
            await OnUnloadToken(data);
            
            if (CurrentZone != null)
                await CurrentZone.Release(this);
        }

        protected abstract UniTask OnUnloadToken(TData data);

        public IZone CurrentZone { get; private set; }
        
        public async UniTask MoveTo(IZone zone, int order = 0)
        {
            if (zone == null)
            {
                if (CurrentZone != null)
                    await CurrentZone.Release(this);

                CurrentZone = null;
                return;
            }

            if (CurrentZone == zone && zone.GetTokenOrder(this) == order)
                return;
            
            if (CurrentZone != zone && CurrentZone != null)
                await CurrentZone.Release(this);

            CurrentZone = zone;
            await zone.Accept(this, order);
        }

        public async UniTask MoveToLimbo()
        {
            if (CurrentZone == null)
                return;

            IZone oldZone = CurrentZone;
            CurrentZone = null;
            await oldZone.Release(this);
            transform.parent = null;
        }
    }
}