using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jank.Props.Architecture.Token
{
    public static class UTIToken
    {
        public static UniTask DestroyAllUntrackedTokens(this IEnumerable<IToken> currentPieces, Transform transform)
        {
            foreach (Transform trans in transform)
            {
                IToken token = trans.GetComponent<IToken>();
                if (token != null && !currentPieces.Contains(token))
                    Object.Destroy(trans.gameObject);
            }

            return UniTask.CompletedTask;
        }
    }
}