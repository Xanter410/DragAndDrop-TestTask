using UnityEngine;

namespace Game.Cursor
{
    [CreateAssetMenu(fileName = "CursorSystemConfig", menuName = "Configs/CursorSystem")]
    public class CursorSystemConfigs : ScriptableObject
    {
        [Header("General")]
        public LayerMask LayerMaskDraggableObjects;

        public float CameraMoveBoundsXMin;
        public float CameraMoveBoundsXMax;

        [Header ("Drag and Drop")]
        public float TakeObjectScaleMultiplier = 1.1f;
        public float TakeObjectScaleDuration = 0.35f;

        [Header("Camera Move Logic")]
        public float ScrollSpeedScale = 9f;

        [Header("Camera Move With ObjectItem")]
        public float ScrollSpeed = 3f;
    }
}