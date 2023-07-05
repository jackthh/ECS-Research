using _ECS_Research.Scripts.Survival_Game.Misc;
using Unity.Entities;
using UnityEngine;

namespace _ECS_Research.Scripts.Survival_Game.Mobs
{
    public struct MobsSpawnerData : IComponentData
    {
        public int spawnedWaveId;
        public float playgroundEdgeSize;
    }



    public class MobsSpawnerAuthoring : MonoBehaviour
    {
    }



    public class MobsSpawnerAuthoringBaker : Baker<MobsSpawnerAuthoring>
    {
        public override void Bake(MobsSpawnerAuthoring _authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new MobsSpawnerData
            {
                spawnedWaveId = -1,
                playgroundEdgeSize = SOAssetsReg.Instance.gameConfig.playgroundEdgeSize
            });
        }
    }
}