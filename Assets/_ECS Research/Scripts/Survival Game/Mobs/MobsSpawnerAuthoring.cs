using _ECS_Research.Scripts.Survival_Game.Misc;
using Unity.Entities;
using UnityEngine;

namespace _ECS_Research.Scripts.Survival_Game.Mobs
{
    public struct MobsSpawnerData : IComponentData
    {
        public bool spawning;
        public int lastSpawnedWaveId;
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
                spawning = false,
                lastSpawnedWaveId = -1,
                playgroundEdgeSize = SOAssetsReg.Instance.gameConfig.playgroundEdgeSize
            });
        }
    }
}