using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;


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
            var random = new Unity.Mathematics.Random((uint) Random.Range(1, 100000));
            var ecb = GetEntityCommandBuffer(ref _state);

            // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
            new ProcessSpawnerJob
            {
                elapsedTime = SystemAPI.Time.ElapsedTime,
                ecb = ecb,
                updatedRandom = random
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
        public Unity.Mathematics.Random updatedRandom;


        // IJobEntity generates a component data query based on the parameters of its `Execute` method.
        // This example queries for all Spawner components and uses `ref` to specify that the operation
        // requires read and write access. Unity processes `Execute` for each entity that matches the
        // component data query.
        private void Execute([ChunkIndexInQuery] int _chunkIndex, ref MobSpawnerComponentData _spawner)
        {
            // If the next spawn time has passed.
            if (_spawner.nextSpawnTime < elapsedTime)
            {
                var randX = updatedRandom.NextFloat(_spawner.spawnBounds.x, _spawner.spawnBounds.y);
                var randY = updatedRandom.NextFloat(_spawner.heightBounds.x, _spawner.heightBounds.y);
                var newEntity = ecb.Instantiate(_chunkIndex, _spawner.prefab);
                ecb.SetComponent(_chunkIndex, newEntity, LocalTransform.FromPosition(randX, randY, 0f));
                // ecb.SetComponent(_chunkIndex, newEntity, LocalTransform.FromPosition(0f, 0f, 0f));

                // Resets the next spawn time.
                _spawner.nextSpawnTime = (float) elapsedTime + _spawner.spawnRate;
            }
        }
    }



    [BurstCompile] public partial struct MovementJob : IJobEntity
    {
        private void Execute([ChunkIndexInQuery] int _chunkIndex)
        {
        }
    }
}