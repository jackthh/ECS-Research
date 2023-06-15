using Unity.Entities;


namespace _ECS_Research.Scripts
{
    public struct MobMovementComponentData : IComponentData
    {
        public float speed;
        public float maxY;
        public float minY;
    }
}