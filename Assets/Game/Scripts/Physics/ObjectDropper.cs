using Game.Physics;
using UnityEngine;
using DG.Tweening;
using Utils.ServiceLocator;

public class ObjectDropper : IPhysics
{
    private LayerMask groundLayer = LayerMask.GetMask("SurfaceToDropObjects");
    private readonly float gravity = 9.81f;

    private readonly float _positionThreshold = 0.01f;

    public void DropObject(GameObject gameObject)
    {
        Vector2 startPosition = gameObject.transform.position;
        Vector2 targetPosition = CalculateDropPosition(gameObject);

        float distanceFall = Vector2.Distance(startPosition, targetPosition);

        if (distanceFall > _positionThreshold)
        {
            float duration = Mathf.Sqrt(2 * distanceFall / gravity);

            Sequence dropSequence = DOTween.Sequence();

            _ = dropSequence
                .Append(gameObject.transform.DOMove(targetPosition, duration).SetEase(Ease.InQuad))
                .Append(gameObject.transform.DOJump(targetPosition, 0.05f, 1, 0.25f));

            _ = dropSequence.Play();
        }
    }

    public Vector2 CalculateDropPosition(GameObject droppingObject)
    {
        if (!droppingObject.TryGetComponent(out Collider2D objectCollider))
        {
            Debug.LogError("Object doesn't have a 2D collider!");
            return droppingObject.transform.position;
        }

        Bounds colliderBounds = objectCollider.bounds;

        Vector2 rayOrigin = new(colliderBounds.center.x, colliderBounds.min.y);

        RaycastHit2D hit = Physics2D.Raycast(
            rayOrigin,
            Vector2.down,
            Mathf.Infinity,
            groundLayer
        );

        if (hit.collider != null)
        {
            Bounds hitColliderBounds = hit.collider.bounds;
            
            float newPositionX = Mathf.Clamp(
                droppingObject.transform.position.x,
                hitColliderBounds.min.x + colliderBounds.extents.x,
                hitColliderBounds.max.x - colliderBounds.extents.x
            );

            float objectHalfHeight = colliderBounds.extents.y;

            float scaleModifier = ServiceLocator.Current.Get<DepthCalculatorService>().GetDepthScaleByPositionY(hit.point.y);

            float newPositionY = hit.point.y + (objectHalfHeight * scaleModifier);

            return new Vector2(
                newPositionX,
                newPositionY
            );
        }

        Debug.LogWarning("No ground found below the object!");
        return droppingObject.transform.position;
    }
}
