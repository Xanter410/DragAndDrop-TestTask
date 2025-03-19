using UnityEngine;
using Utils.ServiceLocator;

namespace Game.Physics
{
    public interface IPhysics : IService
    {
        public void DropObject(GameObject gameObject);
        public Vector2 CalculateDropPosition(GameObject droppingObject);
    }
}
