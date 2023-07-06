using _ECS_Research.Scripts.Survival_Game.General_Entities;
using _ECS_Research.Scripts.Survival_Game.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;


namespace _ECS_Research.Scripts.Survival_Game.Mobs
{
    public partial struct MobsSpawnerSystem : ISystem
    {
        #region ISystem Callbacks

        [BurstCompile] public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<MainScenePlayerRuntimeData>();
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            var ecb = GetEntityCommandBuffer(ref _state);
            var gameConfig = SystemAPI.GetSingletonBuffer<WaveConfigElementData>(true);
            var entitySamples = SystemAPI.GetSingletonBuffer<EntitySampleElementData>(true);
            var playerPos = SystemAPI.GetSingleton<MainScenePlayerRuntimeData>();

            new SpawnMobsJob
            {
                ecb = ecb,
                wavesConfig = gameConfig,
                entitySamples = entitySamples,
                seed = Random.Range(1, 100),
                elapsedTime = SystemAPI.Time.ElapsedTime,
                playerPos = playerPos.playerPos
            }.ScheduleParallel();
        }



        [BurstCompile] public partial struct SpawnMobsJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            [ReadOnly] public DynamicBuffer<WaveConfigElementData> wavesConfig;
            [ReadOnly] public DynamicBuffer<EntitySampleElementData> entitySamples;
            public double elapsedTime;
            public float3 playerPos;

            public int seed;
            private Unity.Mathematics.Random rdm;


            private void Execute([EntityIndexInQuery] int _entityIndex, ref MobsSpawnerData _mobsSpawnerData)
            {
                //  NOTE:   To verify spawning conditions
                if (_mobsSpawnerData.spawning)
                    return;

                var spawningWave = _mobsSpawnerData.lastSpawnedWaveId + 1;
                if (spawningWave >= wavesConfig.Length)
                    return;

                if (elapsedTime < wavesConfig[spawningWave].delayTimeSinceStart)
                    return;

                _mobsSpawnerData.spawning = true;

                //  NOTE:   Spawning progress
                //  NOTE:   To find expected entity sample to spawn
                Entity mobSample = default;
                foreach (var entitySample in entitySamples)
                {
                    if (entitySample.entityId == wavesConfig[spawningWave].mobId)
                    {
                        mobSample = entitySample.entitySample;
                        break;
                    }
                }

                //  NOTE:   To spawn new entities at random pos
                rdm = new Unity.Mathematics.Random((uint) seed);
                for (var i = 0; i < wavesConfig[spawningWave].quantity; i++)
                {
                    var instantiatePos = Utils.GetRandPosWithConditions(ref rdm, wavesConfig[spawningWave].spawningOffsetRange, 0f, playerPos.xz,
                        _mobsSpawnerData.playgroundEdgeSize);
                    // var instantiatePos = float3.zero;
                    var newEntity = ecb.Instantiate(_entityIndex, mobSample);
                    ecb.SetComponent(_entityIndex, newEntity, LocalTransform.FromPosition(instantiatePos).Rotate(quaternion.identity));
                }

                _mobsSpawnerData.spawning = false;
                _mobsSpawnerData.lastSpawnedWaveId = spawningWave;
            }
        }

        #endregion


        #region Utils

        private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState _state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(_state.WorldUnmanaged);
            return ecb.AsParallelWriter();
        }

        #endregion
    }
}