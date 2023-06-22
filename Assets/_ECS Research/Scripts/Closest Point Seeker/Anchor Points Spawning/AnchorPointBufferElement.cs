using Unity.Entities;
using Unity.Mathematics;



namespace _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning
{
    public struct AnchorPointBufferElement : IBufferElementData
    {
        public int id;
        public float3 position;
    }
}