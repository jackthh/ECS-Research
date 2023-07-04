/*using Unity.Entities;
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
            // var rdm = new Unity.Mathematics.Random((uint) Random.Range(0, 1000000));
            // var randomComponent = SystemAPI.GetSingletonRW<RandomnessGeneratorComponentData>();
            // randomComponent.ValueRW.value = new Unity.Mathematics.Random((uint) Random.Range(-100, 100));
            // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
            new ProcessSpawnerJob
            {
                elapsedTime = SystemAPI.Time.ElapsedTime,
                ecb = ecb,
                seed = Random.Range(1, 100)
                // rdm = rdm
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
        public int seed;


        // IJobEntity generates a component data query based on the parameters of its `Execute` method.
        // This example queries for all Spawner components and uses `ref` to specify that the operation
        // requires read and write access. Unity processes `Execute` for each entity that matches the
        // component data query.
        private void Execute([EntityIndexInQuery] int _entityIndex, Entity _e, ref MobSpawnerComponentData _spawner)
        {
            seed += _entityIndex;
            rdm = new Unity.Mathematics.Random((uint) seed);
            // If the next spawn time has passed.
            if (_spawner.nextSpawnTime < elapsedTime)
            {
                for (var i = 0; i < _spawner.amountPerWave; i++)
                {
                    if (_spawner.spawnedMobs >= _spawner.maxMobs)
                    {
                        ecb.SetEnabled(_entityIndex, _e, false);

                        return;
                    }

                    var instantiatePos = Utils.GetRandomPosition(ref rdm, _spawner.spawnBounds, _spawner.heightBounds);
                    var newEntity = ecb.Instantiate(_entityIndex, _spawner.prefab);
                    _spawner.spawnedMobs++;
                    ecb.SetComponent(_entityIndex, newEntity, LocalTransform.FromPosition(instantiatePos));
                    ecb.AddComponent(_entityIndex, newEntity, new MobMovementComponentData
                    {
                        speed = _spawner.movementSpeed,
                        minY = _spawner.heightBounds.x,
                        maxY = _spawner.heightBounds.y,
                        movingUp = true
                    });
                }

                // Resets the next spawn time.
                _spawner.nextSpawnTime = (float) elapsedTime + _spawner.spawnRate;
            }
        }
    }
}*/