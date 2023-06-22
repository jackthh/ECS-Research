using Unity.Entities;
using Unity.Mathematics;

namespace _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning
{
    public struct AnchorPointSpawnerData : IComponentData
    {
        public Entity prefab;
        public int initSize;
        public float2 xSpawnBounds;
        public float2 ySpawnBounds;
    }
}