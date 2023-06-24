using System.Collections;
using System.Collections.Generic;
using _ECS_Research.Scripts;
using _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning;
using _ECS_Research.Scripts.Closest_Point_Seeker.Seekers_Spawning;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;



public partial struct SeekersSpawnerSystem : ISystem
{
    [BurstCompile] public void OnCreate(ref SystemState _state)
    {
        _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }


    [BurstCompile] public void OnUpdate(ref SystemState _state)
    {
        _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = GetEntityCommandBuffer(ref _state);

        new SpawnSeekersJob
        {
            elapsedTime = SystemAPI.Time.ElapsedTime,
            ecb = ecb,
            seed = Random.Range(1, 1000)
        }.ScheduleParallel();
    }


    #region Jobs

    [BurstCompile] public partial struct SpawnSeekersJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public double elapsedTime;
        public Unity.Mathematics.Random rdm;
        public int seed;


        private void Execute([EntityIndexInQuery] int _entityIndex, ref SeekersSpawnerData _spawnerData)
        {
            seed += _entityIndex;
            rdm = new Unity.Mathematics.Random((uint) seed);
            // If the next spawn time has passed.
            if (_spawnerData.nextSpawnTime < elapsedTime)
            {
                for (var i = 0; i < _spawnerData.amountPerWave; i++)
                {
                    var instantiatePos = Utils.GetRandomPosition(ref rdm, _spawnerData.xSpawnBounds, _spawnerData.ySpawnBounds, -0.1f);
                    var newEntity = ecb.Instantiate(_entityIndex, _spawnerData.prefab);

                    ecb.SetComponent(_entityIndex, newEntity, LocalTransform.FromPosition(instantiatePos));
                    ecb.AddComponent(_entityIndex, newEntity, new SeekerConfigData
                    {
                        movementSpeed = _spawnerData.seekerMovementSpeed,
                        searchingRadius = _spawnerData.seekerSearchingRadius,
                    });
                    ecb.AddComponent(_entityIndex, newEntity, new SeekerRuntimeData
                    {
                        currentTarget = new AnchorPointData {id = -100},
                    });

                    ecb.AddBuffer<AnchorPointDataElement>(_entityIndex, newEntity);
                }

                _spawnerData.nextSpawnTime = (float) elapsedTime + _spawnerData.interval;
            }
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