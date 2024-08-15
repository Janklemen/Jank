using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.Props.Architecture.Token;
using UnityEngine;

namespace Jank.Props.Utilities.Arrangers
{
    public abstract class ATokenArranger : MonoBehaviour
    {
        public abstract UniTask Reorder(IEnumerable<IToken> currentTokens);
    }
}