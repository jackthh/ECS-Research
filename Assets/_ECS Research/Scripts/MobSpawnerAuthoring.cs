using Unity.Entities;
using UnityEngine;


namespace _ECS_Research.Scripts
{
    public class MobSpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public float spawnRate;
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
                spawnPosition = _authoring.transform.position,
                nextSpawnTime = 0.0f,
                spawnRate = _authoring.spawnRate
            });
        }
    }
}

