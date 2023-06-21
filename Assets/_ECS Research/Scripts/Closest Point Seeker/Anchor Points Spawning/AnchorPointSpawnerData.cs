using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _ECS_Research.Scripts.Closest_Point_Seeker
{
    public struct AnchorPointSpawnerData : IComponentData
    {
        public Entity prefab;
        public int initSize;
        public float2 xSpawnBounds;
        public float2 ySpawnBounds;
        public int assignedPointsPointer;
        public NativeArray<LocalTransform> anchorPoints;
    }
}