using Unity.Entities;
using Unity.Mathematics;


namespace _ECS_Research.Scripts
{
    public struct MobSpawnerComponentData : IComponentData
    {
        public Entity prefab;
        public float3 spawnPosition;
        public float nextSpawnTime;
        public float spawnRate;
    }
}