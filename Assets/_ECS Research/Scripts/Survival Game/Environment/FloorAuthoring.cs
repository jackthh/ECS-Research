using Unity.Entities;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Environment
{
    public struct FloorData : IComponentData
    {
        public float edgeSize;
    }



    public class FloorAuthoring : MonoBehaviour
    {
        public float edgeSize;
    }



    public class FloorAuthoringBaker : Baker<FloorAuthoring>
    {
        public override void Bake(FloorAuthoring _authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new FloorData
            {
                edgeSize = _authoring.edgeSize
            });
        }
    }
}