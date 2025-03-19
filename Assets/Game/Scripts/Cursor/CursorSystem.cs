using UnityEngine;

namespace Game.Cursor
{
    public class CursorSystem : MonoBehaviour
    {
        [SerializeField] CursorSystemConfigs _cursorSystemConfigs;

        private DragAndDropLogic _dragAndDropLogic;
        private DragAndDropRenderer _dragAndDropRenderer;

        private CameraMoveLogic _cameraMoveLogic;
        private CameraMoveWithObject _cameraMoveWithObject;

        public void Initialize() 
        {
            _dragAndDropLogic = new DragAndDropLogic(_cursorSystemConfigs);
            _dragAndDropRenderer = new(_cursorSystemConfigs, _dragAndDropLogic);

            _cameraMoveLogic = new CameraMoveLogic(_cursorSystemConfigs);
            _cameraMoveWithObject = new CameraMoveWithObject(_cursorSystemConfigs, _dragAndDropLogic);
        }

        private void Update()
        {
            _dragAndDropLogic.Update();

            _cameraMoveLogic.Update();
            _cameraMoveWithObject.Update();
        }
    }
}
