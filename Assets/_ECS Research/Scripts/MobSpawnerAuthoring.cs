using Unity.Entities;
using UnityEngine;


namespace _ECS_Research.Scripts
{
    public class MobSpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public float spawnRate;
        public int amountPerWave;
        public Vector2 spawnBounds;
        public float movementSpeed;
        public Vector2 heightBounds;
    }



    public class MobSpawnerBaker : Baker<MobSpawnerAuthoring>
    {
        public override void Bake(MobSpawnerAuthoring _authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new MobSpawnerComponentData
            {
                // By default, each authoring GameObject turns into an Entity.
                // Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
                prefab = GetEntity(_authoring.prefab, TransformUsageFlags.Dynamic),
                spawnRate = _authoring.spawnRate,
                nextSpawnTime = 0.0f,
                amountPerWave = _authoring.amountPerWave,
                spawnBounds = _authoring.spawnBounds,
                movementSpeed = _authoring.movementSpeed,
                heightBounds = _authoring.heightBounds
            });
            AddComponent(entity, new RandomnessGeneratorComponentData
            {
                value = new Unity.Mathematics.Random(1000)
            });
        }
    }
}