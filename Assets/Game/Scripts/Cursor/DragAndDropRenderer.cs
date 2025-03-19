using UnityEngine;
using DG.Tweening;
using Game.Object;
using Utils.ServiceLocator;

namespace Game.Cursor
{
    public class DragAndDropRenderer
    {
        private readonly float _takeObjectScaleMultiplier;
        private readonly float _takeObjectScaleDuration;

        private ObjectItem _currentObjectItem;

        private Tween _takeTween;
        private Tween _dropTween;

        public DragAndDropRenderer(CursorSystemConfigs cursorSystemConfigs, DragAndDropLogic dragAndDropLogic)
        {
            _takeObjectScaleMultiplier = cursorSystemConfigs.TakeObjectScaleMultiplier;
            _takeObjectScaleDuration = cursorSystemConfigs.TakeObjectScaleDuration;

            dragAndDropLogic.ObjectTaked += HandleTakeObject;
            dragAndDropLogic.ObjectDropped += HandleDropObject;
        }

        private void HandleTakeObject(ObjectItem objectItem)
        {
            if (_currentObjectItem == objectItem.gameObject)
            {
                _takeTween.Complete();
                _dropTween.Complete();
            }
            else
            {
                _currentObjectItem = objectItem;
            }

            _takeTween = _currentObjectItem.transform
                .DOScale(_currentObjectItem.BaseScale * _takeObjectScaleMultiplier, _takeObjectScaleDuration)
                .SetEase(Ease.InOutSine);

            _ = _takeTween.Play();
        }

        private void HandleDropObject(Vector2 dropPosition)
        {
            float scaleModifier = ServiceLocator.Current.Get<DepthCalculatorService>().GetDepthScaleByPositionY(dropPosition.y);

            int sortingOrder = ServiceLocator.Current.Get<DepthCalculatorService>().GetOrderLayerByPositionY(dropPosition.y);
            _currentObjectItem.SpriteRenderer.sortingOrder = sortingOrder;

            _dropTween = _currentObjectItem.transform
                .DOScale(_currentObjectItem.BaseScale * scaleModifier, _takeObjectScaleDuration)
                .SetEase(Ease.InOutSine);

            _ = _dropTween.Play();
        }
    }
}