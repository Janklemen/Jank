using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Jank.Props.Architecture.Token;
using UnityEngine;

namespace Jank.Props.Architecture.Zone
{
    public abstract class AZone<TData> : AStandardProp<TData>, IZone<TData>
    {
        protected List<IToken> CurrentTokens = new();
        
        public IReadOnlyList<IToken> Tokens => CurrentTokens;

        public virtual Transform TokenParent => transform;

        protected override async UniTask OnLoadStandard(TData path)
        {
            await OnLoadZone(path);
        }
        protected abstract UniTask OnLoadZone(TData data);

        protected override async UniTask OnUnloadStandard(TData data)
        {
            await OnUnloadZone(data);

            List<IToken> tokens = Tokens.ToList();
            
            foreach (IToken token in tokens)
                await token.Unload();
            
            CurrentTokens.Clear();
        }

        protected abstract UniTask OnUnloadZone(TData data);

        public async UniTask Accept(IToken token, int order = 0)
        {
            int index = GetTokenOrder(token);
            
            if (index != -1)
            {
                if (index != order)
                    await Reorder();
                
                return;
            }
            
            token.Transform.SetParent(TokenParent);
            CurrentTokens.Insert(order, token);

            await token.MoveTo(this, order);
            _ = Reorder();
        }

        public async UniTask Release(IToken token)
        {
            if(!CurrentTokens.Contains(token))
                return;
            
            await token.MoveToLimbo();
            CurrentTokens.Remove(token);
        }

        public int GetTokenOrder(IToken token) => CurrentTokens.IndexOf(token);
        
        public abstract UniTask Reorder();
    }
}