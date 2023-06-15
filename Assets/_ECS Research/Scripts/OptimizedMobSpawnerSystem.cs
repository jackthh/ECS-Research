using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _ECS_Research.Scripts
{
    [BurstCompile] public partial struct OptimizedMobSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            var ecb = GetEntityCommandBuffer(ref _state);
            var rdm = new Unity.Mathematics.Random((uint) Random.Range(0, 1000000));
            // var randomComponent = SystemAPI.GetSingletonRW<RandomnessGeneratorComponentData>();
            // randomComponent.ValueRW.value = new Unity.Mathematics.Random((uint) Random.Range(-100, 100));
            // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
            new ProcessSpawnerJob
            {
                elapsedTime = SystemAPI.Time.ElapsedTime,
                ecb = ecb,
                // seed = 10
                rdm = rdm
            }.ScheduleParallel();
        }


        private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState _state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(_state.WorldUnmanaged);
            return ecb.AsParallelWriter();
        }
    }



    [BurstCompile] public partial struct ProcessSpawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public double elapsedTime;
        public Unity.Mathematics.Random rdm;
        // public int seed;


        // IJobEntity generates a component data query based on the parameters of its `Execute` method.
        // This example queries for all Spawner components and uses `ref` to specify that the operation
        // requires read and write access. Unity processes `Execute` for each entity that matches the
        // component data query.
        private void Execute([ChunkIndexInQuery] int _chunkIndex, ref MobSpawnerComponentData _spawner)
        {
            // seed++;
            // rdm = new Unity.Mathematics.Random((uint) seed);

            // If the next spawn time has passed.
            if (_spawner.nextSpawnTime < elapsedTime)
            {
                for (var i = 0; i < _spawner.amountPerWave; i++)
                {
                    var instantiatePos = GetRandomPosition(rdm, _spawner.spawnBounds, _spawner.heightBounds);
                    var newEntity = ecb.Instantiate(_chunkIndex, _spawner.prefab);
                    ecb.SetComponent(_chunkIndex, newEntity, LocalTransform.FromPosition(instantiatePos));
                }
                // ecb.SetComponent(_chunkIndex, newEntity, LocalTransform.FromPosition(0f, 0f, 0f));

                // Resets the next spawn time.
                _spawner.nextSpawnTime = (float) elapsedTime + _spawner.spawnRate;
            }
        }


        private float3 GetRandomPosition(Unity.Mathematics.Random _rdm, float2 _spawnBounds, float2 _heightBounds)
        {
            return new float3
                (_rdm.NextFloat(_spawnBounds.x, _spawnBounds.y), _rdm.NextFloat(_heightBounds.x, _heightBounds.y), 0f);
        }
    }
}