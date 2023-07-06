using _ECS_Research.Scripts.Survival_Game.Player;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.General_Entities
{
    public struct MainWorldPlayerData : IComponentData
    {
        public float3 playerPos;
    }



    public class MainWorldDataAuthoring : MonoBehaviour
    {
    }



    public class MainWorldDataAuthoringBaker : Baker<MainWorldDataAuthoring>
    {
        public override void Bake(MainWorldDataAuthoring _authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new MainWorldPlayerData {playerPos = float3.zero});
        }
    }
}