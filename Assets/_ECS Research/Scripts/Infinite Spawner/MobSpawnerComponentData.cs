using Unity.Entities;
using Unity.Mathematics;


namespace _ECS_Research.Scripts
{
    public struct MobSpawnerComponentData : IComponentData
    {
        public Entity prefab;
        public float spawnRate;
        public float nextSpawnTime;
        public int amountPerWave;
        public float2 spawnBounds;
        public float movementSpeed;
        public float2 heightBounds;
        public int spawnedMobs;
        public int maxMobs;
    }
}