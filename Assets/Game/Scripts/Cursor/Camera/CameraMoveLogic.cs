using Game.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.ServiceLocator;

namespace Game.Cursor
{
    public class CameraMoveLogic
    {
        private readonly LayerMask _layerMaskDraggableObjects;
        private readonly float _speedScrollScale;
        private readonly float _cameraMoveBoundsXMin;
        private readonly float _cameraMoveBoundsXMax;

        private readonly Camera _mainCamera;

        private float _startCursorPositionX;
        private float _startCameraPositionX;

        public CameraMoveLogic(CursorSystemConfigs cursorSystemConfigs)
        {
            _layerMaskDraggableObjects = cursorSystemConfigs.LayerMaskDraggableObjects;
            _speedScrollScale = cursorSystemConfigs.ScrollSpeedScale;
            _cameraMoveBoundsXMin = cursorSystemConfigs.CameraMoveBoundsXMin;
            _cameraMoveBoundsXMax = cursorSystemConfigs.CameraMoveBoundsXMax;

            _mainCamera = Camera.main;

            IInput input = ServiceLocator.Current.Get<IInput>();

            input.ClickStarted += HandleClickStarted;
            input.ClickCanceled += HandleClickCanceled;
        }
        public void Update()
        {
            if (_startCursorPositionX != 0)
            {
                CameraMove(Pointer.current.position.ReadValue());
            }
        }

        private void HandleClickStarted()
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(
                    _mainCamera.ScreenPointToRay(Pointer.current.position.ReadValue()),
                    float.MaxValue,
                    _layerMaskDraggableObjects);

            if (rayHit.collider == null)
            {
                _startCameraPositionX = _mainCamera.transform.position.x;
                _startCursorPositionX = Pointer.current.position.ReadValue().x;
            }
        }

        private void HandleClickCanceled()
        {
            _startCursorPositionX = 0;
        }

        private void CameraMove(Vector2 cursorScreenPosition)
        {
            float currentOffsetX = (_startCursorPositionX - cursorScreenPosition.x) / Screen.width;

            float newCameraPositionX = _startCameraPositionX + (currentOffsetX * _speedScrollScale);

            newCameraPositionX = Mathf.Clamp(newCameraPositionX, _cameraMoveBoundsXMin, _cameraMoveBoundsXMax);

            _mainCamera.transform.position = new Vector3(
                newCameraPositionX,
                _mainCamera.transform.position.y,
                _mainCamera.transform.position.z
                );
        }
    }
}