using _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning;
using Unity.Entities;


namespace _ECS_Research.Scripts.Closest_Point_Seeker.Seekers_Spawning
{
    public struct SeekerConfigData : IComponentData
    {
        public float movementSpeed;
        public float searchingRadius;
     
    }



    public struct SeekerRuntimeData : IComponentData
    {
        public AnchorPointBufferElement currentTarget;
        public DynamicBuffer<AnchorPointBufferElement> reachedPoints;
    }
}