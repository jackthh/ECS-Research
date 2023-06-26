using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning
{
    public struct AnchorPointData : IComponentData
    {
        public int id;
        public float2 position;
    }



    public struct AnchorPointDataElement : IBufferElementData
    {
        public int id;
    }
}