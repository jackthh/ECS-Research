using Unity.Mathematics;


namespace _ECS_Research.Scripts
{
    public static class Utils
    {
        public static float3 GetRandomPosition(ref Unity.Mathematics.Random _rdm, float2 _spawnBounds, float2 _heightBounds)
        {
            return new float3(_rdm.NextFloat(_spawnBounds.x, _spawnBounds.y), _rdm.NextFloat(_heightBounds.x, _heightBounds.y), 0f);
        }
    }
}