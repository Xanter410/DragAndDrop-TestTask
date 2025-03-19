using System;
using Game.Input;
using Game.Object;
using Game.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.ServiceLocator;

namespace Game.Cursor
{
    public class DragAndDropLogic
    {
        public event Action<ObjectItem> ObjectTaked;
        public event Action<Vector2> ObjectDropped;

        private readonly Camera _camera;

        private LayerMask _layerMaskDraggableObjects;

        private ObjectItem _draggedObjectItem;
        private bool _isDraggable => _draggedObjectItem != null;

        public DragAndDropLogic(CursorSystemConfigs cursorSystemConfigs)
        {
            _layerMaskDraggableObjects = cursorSystemConfigs.LayerMaskDraggableObjects;

            IInput input = ServiceLocator.Current.Get<IInput>();

            _camera = Camera.main;

            input.ClickStarted += HandleClickStarted;
            input.ClickCanceled += HandleClickCanceled;
        }
        public void Update()
        {
            if (_isDraggable)
            {
                Vector2 worldPosition = _camera.ScreenToWorldPoint(Pointer.current.position.ReadValue());
                OnDragObject(worldPosition);
            }
        }

        private void HandleClickStarted()
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(
                    _camera.ScreenPointToRay(Pointer.current.position.ReadValue()),
                    float.MaxValue,
                    _layerMaskDraggableObjects);

            if (rayHit.collider != null)
            {
                OnTakeObject(rayHit.collider.gameObject.GetComponent<ObjectItem>());
            }
        }

        private void HandleClickCanceled()
        {
            if (_isDraggable)
            {
                OnDropObject();
            }
        }

        private void OnTakeObject(ObjectItem objectItem)
        {
            ObjectTaked?.Invoke(objectItem);
            
            _draggedObjectItem = objectItem;
        }

        private void OnDragObject(Vector2 worldPosition)
        {
            _draggedObjectItem.transform.position = worldPosition;
        }

        private void OnDropObject()
        {
            IPhysics physics = ServiceLocator.Current.Get<IPhysics>();

            Vector2 dropPosition = physics.CalculateDropPosition(_draggedObjectItem.gameObject);

            ObjectDropped?.Invoke(dropPosition);

            physics.DropObject(_draggedObjectItem.gameObject);

            _draggedObjectItem = null;
        }
    }
}