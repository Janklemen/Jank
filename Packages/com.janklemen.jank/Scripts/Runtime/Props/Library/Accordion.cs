using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Jank.Inspector;
using Jank.Inspector.CustomEditorGenerator;
using Jank.Objects;
using Jank.Props.Architecture;
using Jank.Services.Animation;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Props.Library
{
    public partial class Accordion : AProp<List<AProp>>
    {
        [SerializeField] Vector3 Step;
        [JankInspect] int x;
        
        List<AProp> _currentToken = new();

        [JankInject] IAnimationService _anim;

        CancellationTokenSource _updateInstancePositionCancelation;
        
        public IReadOnlyList<AProp> CurrentToken => _currentToken;

        protected override UniTask OnSetInteractable(bool isInteractable)
        {
            return UniTask.CompletedTask;
        }

        protected override async UniTask OnLoadPropWithData(List<AProp> data)
        {
            foreach (AProp prop in data)
            {
                prop.Transform.SetParent(transform);
                await prop.Load();
                _currentToken.Add(prop);
            }
            
            await UpdateInstancePositions(false);
        }

        protected override async UniTask OnUnloadPropWithData(List<AProp> data)
        {
            foreach (AProp prop in _currentToken)
                await prop.Unload();
            
            _currentToken.Clear();
        }

        async UniTask UpdateInstancePositions(bool animate)
        {
            UTCancellationTokenSource.CancelAndRenew(ref _updateInstancePositionCancelation);
            
            Vector3 origin = Vector3.zero;

            List<UniTask> anims = new();
            
            foreach (AProp prop in _currentToken)
            {
                if (animate)
                    anims.Add(_anim.InterpolateToken(prop, origin, _updateInstancePositionCancelation.Token));
                else
                    prop.transform.localPosition = origin;
                origin += Step;
            }

            await UniTask.WhenAll(anims);
        }

        public async UniTask InsertItem(int index, AProp prop)
        {
            prop.Transform.SetParent(transform);
            _currentToken.Insert(index, prop);
            await UpdateInstancePositions(true);
        }

        public async UniTask RemoveItemAt(int index)
        {
            _currentToken.RemoveAt(index);
            await UpdateInstancePositions(true);
        }

        public async UniTask RemoveItem(AProp item)
        {
            if(!_currentToken.Contains(item))
                return;
            
            _currentToken.Remove(item);
            await UpdateInstancePositions(true);
        }
    }
}
