using _ECS_Research.Scripts.Survival_Game.Misc;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.General_Entities
{
    public struct WaveConfigElementData : IBufferElementData
    {
        public int mobId;
        public int quantity;
        public float delayTimeSinceStart;
        public float2 spawningOffsetRange;
    }



    public struct EntitySampleElementData : IBufferElementData
    {
        public int entityId;
        public Entity entitySanmple;
    }



    public class GameConfigAuthoring : MonoBehaviour
    {
    }



    public class GameConfigAuthoringBaker : Baker<GameConfigAuthoring>
    {
        public override void Bake(GameConfigAuthoring _authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            //  NOTE:   Prefab samples data
            var newEntityBuffer = AddBuffer<EntitySampleElementData>(entity);
            foreach (var entitySample in SOAssetsReg.Instance.gameConfig.entitySamples)
            {
                newEntityBuffer.Add(new EntitySampleElementData
                {
                    entityId = entitySample.id,
                    entitySanmple = GetEntity(entitySample.gameObject, TransformUsageFlags.None)
                });
            }


            //  NOTE:   Waves config data
            var newConfigBuffer = AddBuffer<WaveConfigElementData>(entity);
            foreach (var waveConfig in SOAssetsReg.Instance.gameConfig.wavesConfig)
            {
                newConfigBuffer.Add(new WaveConfigElementData
                {
                    mobId = waveConfig.mobPrefab.id,
                    quantity = waveConfig.quantity,
                    spawningOffsetRange = waveConfig.spawningOffsetRange,
                    delayTimeSinceStart = waveConfig.delayTimeSinceStart
                });
            }
        }
    }
}