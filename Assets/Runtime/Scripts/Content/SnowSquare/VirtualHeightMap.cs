using UnityEngine;

public sealed class VirtualHeightMap
{
    private const float DEFAULT_MAX = 1;
    private const float DEFAULT_MIN = 0;
    private const int DIMENSION = 10;
    private float[,] _heights;

    public VirtualHeightMap()
        => ResetHeight();

    public void ResetHeight()
    {
        _heights = new float[DIMENSION, DIMENSION];
        for (int i = 0; i < DIMENSION; i++)
        {
            for (int j = 0; j < DIMENSION; j++)
                _heights[i, j] = DEFAULT_MAX;
        }
    }
    public float GetHeightByCoordinates(Vector2 coordinate)
    {
        int i = (int)((DIMENSION - 1) * (coordinate.x / 1));
        int j = (int)((DIMENSION - 1) * (coordinate.y / 1));
        return _heights[i, j];
    }
    public void SetHeightByCoordinates(Vector2 coordinate, float value)
    {
        int i = (int)((DIMENSION - 1) * (coordinate.x / 1));
        int j = (int)((DIMENSION - 1) * (coordinate.y / 1));
        _heights[i, j] = Mathf.Clamp(value, DEFAULT_MIN, DEFAULT_MAX);
    }
}