using Unity.Entities;


namespace _ECS_Research.Scripts
{
    public class MobMovementComponentData : IComponentData
    {
        public float speed;
        public float maxY;
        public float minY;
    }
}