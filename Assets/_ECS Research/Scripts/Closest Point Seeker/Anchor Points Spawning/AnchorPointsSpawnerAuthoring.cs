using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace _ECS_Research.Scripts.Closest_Point_Seeker
{
    public class AnchorPointsSpawnerAuthoring : MonoBehaviour
    {
        public GameObject anchorPointPrefab;
        public int initSize;
        public Vector2 xSpawnBounds;
        public Vector2 ySpawnBounds;
    }



    public class AnchorPointsSpawnerBaker : Baker<AnchorPointsSpawnerAuthoring>
    {
        public override void Bake(AnchorPointsSpawnerAuthoring _authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new AnchorPointSpawnerData
            {
                prefab = GetEntity(_authoring.anchorPointPrefab, TransformUsageFlags.Dynamic),
                initSize = _authoring.initSize,
                xSpawnBounds = _authoring.xSpawnBounds,
                ySpawnBounds = _authoring.ySpawnBounds,
                assignedPointsPointer = 0,
                anchorPoints = new NativeArray<LocalTransform>(_authoring.initSize, Allocator.Persistent)
            });
        }
    }
}