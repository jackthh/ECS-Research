using Unity.Entities;

namespace _ECS_Research.Scripts
{
    public struct RandomnessGeneratorComponentData : IComponentData
    {
        public Unity.Mathematics.Random value;
    }
}