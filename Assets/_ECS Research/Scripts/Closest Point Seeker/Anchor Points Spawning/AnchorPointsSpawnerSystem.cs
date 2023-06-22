using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning
{
    public partial struct AnchorPointsSpawnerSystem : ISystem
    {
        public bool spawnedAnchorPoints;


        #region ISystem Callbacks

        [BurstCompile] public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            spawnedAnchorPoints = false;
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

            var ecb = GetEntityCommandBuffer(ref _state);

            if (!spawnedAnchorPoints)
            {
                new SpawnAnchorPointsJob
                {
                    ecb = ecb,
                    seed = Random.Range(1, 100)
                }.ScheduleParallel();
                spawnedAnchorPoints = true;
            }
        }

        #endregion


        #region Jobs

        [BurstCompile] public partial struct SpawnAnchorPointsJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            public Unity.Mathematics.Random rdm;
            public int seed;
            // public DynamicBuffer<LocalTransform> spawnedAnchors;


            private void Execute([EntityIndexInQuery] int _entityIndex, ref DynamicBuffer<AnchorPointBufferElement> _anchorPointsBuffer,
                ref AnchorPointSpawnerData _spawnerData)
            {
                seed += _entityIndex;
                rdm = new Unity.Mathematics.Random((uint) seed);

                for (var i = 0; i < _spawnerData.initSize; i++)
                {
                    var instantiatePos = Utils.GetRandomPosition(ref rdm, _spawnerData.xSpawnBounds, _spawnerData.ySpawnBounds);
                    var newEntity = ecb.Instantiate(_entityIndex, _spawnerData.prefab);
                    ecb.SetComponent(_entityIndex, newEntity, LocalTransform.FromPosition(instantiatePos));
                    _anchorPointsBuffer.Add(new AnchorPointBufferElement
                    {
                        id = i,
                        position = instantiatePos
                    });
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
}