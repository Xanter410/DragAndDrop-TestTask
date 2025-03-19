using UnityEngine;
using Utils.ServiceLocator;

namespace Game.Object
{
    public class ObjectItem : MonoBehaviour
    {
        [SerializeField] private float _baseScale;
        public float BaseScale => _baseScale;

        public SpriteRenderer SpriteRenderer { get; private set; }

        private void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();

            int sortingOrder = ServiceLocator.Current.Get<DepthCalculatorService>().GetOrderLayerByPositionY(transform.position.y);
            SpriteRenderer.sortingOrder = sortingOrder;

            float scaleModifier = ServiceLocator.Current.Get<DepthCalculatorService>().GetDepthScaleByPositionY(transform.position.y);
            transform.localScale = transform.localScale * scaleModifier;
        }
    }
}
