using _ECS_Research.Scripts.Survival_Game.Misc;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.General_Entities
{
    public struct MainScenePlayerRuntimeData : IComponentData
    {
        public int playerHP;
        public float3 playerPos;
    }



    public class MainSceneDataAuthoring : MonoBehaviour
    {
    }



    public class MainSceneDataAuthoringBaker : Baker<MainSceneDataAuthoring>
    {
        public override void Bake(MainSceneDataAuthoring _authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new MainScenePlayerRuntimeData
            {
                playerHP = SOAssetsReg.Instance.gameConfig.playerConfig.hp,
                playerPos = float3.zero
            });
        }
    }
}