using UnityEngine;
using Utils.ServiceLocator;

public class DepthCalculatorService : IService
{
    private readonly float _maxDepthScale;
    private readonly float _minDepthScale;

    private readonly float _maxDepthPositionY;
    private readonly float _minDepthPositionY;

    public DepthCalculatorService(
        float maxDepthScale = 0.8f,
        float minDepthScale = 1f,
        float maxDepthPositionY = -0.72f,
        float minDepthPositionY = -1.9f
        )
    {
        _maxDepthScale = maxDepthScale;
        _minDepthScale = minDepthScale;
        _maxDepthPositionY = maxDepthPositionY;
        _minDepthPositionY = minDepthPositionY;
    }

    public float GetDepthScaleByPositionY(float worldPositionY)
    {
        float t = Mathf.InverseLerp(_minDepthPositionY, _maxDepthPositionY, worldPositionY);

        float scaleModifier = Mathf.Lerp(_minDepthScale, _maxDepthScale, t);

        return scaleModifier;
    }

    public int GetOrderLayerByPositionY(float worldPositionY)
    {
        float t = Mathf.InverseLerp(-2, 2, worldPositionY);

        int layer = Mathf.RoundToInt(Mathf.Lerp(1000, 0, t));
        return layer;
    }
}
