using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace _ECS_Research.Scripts.Survival_Game.General_Entities
{
    public class EntityIdentifierAuthoring : MonoBehaviour
    {
        [Tooltip("Change id using GameConfig SO Asset"), ReadOnly] public int id;
    }



    public class EntityIdentifierAuthoringBaker : Baker<EntityIdentifierAuthoring>
    {
        public override void Bake(EntityIdentifierAuthoring _authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new EntitySampleIdentifier {id = _authoring.id});
        }
    }



    public struct EntitySampleIdentifier : IComponentData
    {
        public int id;
    }
}