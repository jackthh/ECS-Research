using Unity.Mathematics;


namespace _ECS_Research.Scripts
{
    public static class Utils
    {
        public static float3 GetRandomPosition(ref Unity.Mathematics.Random _rdm, float2 _xBounds, float2 _yBounds)
        {
            return new float3(_rdm.NextFloat(_xBounds.x, _xBounds.y), _rdm.NextFloat(_yBounds.x, _yBounds.y), 0f);
        }
    }
}