using Game.Object;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cursor
{
    public class CameraMoveWithObject
    {
        private readonly float _scrollSpeed;
        private readonly float _cameraMoveBoundsXMin;
        private readonly float _cameraMoveBoundsXMax;

        private readonly DragAndDropLogic _dragAndDropLogic;
        private readonly Camera _mainCamera;

        private bool _isDragging = false;

        public CameraMoveWithObject(CursorSystemConfigs cursorSystemConfigs, DragAndDropLogic dragAndDropLogic)
        {
            _scrollSpeed = cursorSystemConfigs.ScrollSpeed;
            _cameraMoveBoundsXMin = cursorSystemConfigs.CameraMoveBoundsXMin;
            _cameraMoveBoundsXMax = cursorSystemConfigs.CameraMoveBoundsXMax;

            _dragAndDropLogic = dragAndDropLogic;

            _dragAndDropLogic.ObjectTaked += HandleObjectTaked;
            _dragAndDropLogic.ObjectDropped += HandleObjectDropped;

            _mainCamera = Camera.main;
        }

        public void Update()
        {
            if (_isDragging)
            {
                CheckAndTryCameraMove();
            }
        }

        private void HandleObjectTaked(ObjectItem _)
        {
            _isDragging = true;
        }

        private void HandleObjectDropped(Vector2 _)
        {
            _isDragging = false;
        }

        private void CheckAndTryCameraMove()
        {
            Vector2 cursorScreenPosition = Pointer.current.position.ReadValue();

            float newCameraPositionX;

            if (cursorScreenPosition.x < Screen.width * 0.1f)
            {
                newCameraPositionX = _mainCamera.transform.position.x - (_scrollSpeed * Time.deltaTime);
                CameraMove(newCameraPositionX);
            }
            else if (cursorScreenPosition.x > Screen.width * 0.9f)
            {
                newCameraPositionX = _mainCamera.transform.position.x + (_scrollSpeed * Time.deltaTime);
                CameraMove(newCameraPositionX);
            }
        }

        private void CameraMove(float newCameraPositionX)
        {
            newCameraPositionX = Mathf.Clamp(newCameraPositionX, _cameraMoveBoundsXMin, _cameraMoveBoundsXMax);

            _mainCamera.transform.position = new Vector3(
                    newCameraPositionX,
                    _mainCamera.transform.position.y,
                    _mainCamera.transform.position.z
                    );
        }
    }
}