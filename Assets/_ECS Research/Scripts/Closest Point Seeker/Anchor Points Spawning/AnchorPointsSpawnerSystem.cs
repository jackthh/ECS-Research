using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning
{
    public partial struct AnchorPointsSpawnerSystem : ISystem
    {
        #region ISystem Callbacks

        [BurstCompile] public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {

            var ecb = GetEntityCommandBuffer(ref _state);

            new SpawnAnchorPointsJob
            {
                ecb = ecb,
                seed = Random.Range(1, 100)
            }.ScheduleParallel();
        }

        #endregion


        #region Jobs

        [BurstCompile] public partial struct SpawnAnchorPointsJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            public Unity.Mathematics.Random rdm;
            public int seed;


            private void Execute([EntityIndexInQuery] int _entityIndex, Entity _entity, ref AnchorPointSpawnerData _spawnerData, in SpawningAnchors _spawningAnchors)
            {
                seed += _entityIndex;
                rdm = new Unity.Mathematics.Random((uint) seed);

                for (var i = 0; i < _spawnerData.initSize; i++)
                {
                    var instantiatePos = Utils.GetRandomPosition(ref rdm, _spawnerData.xSpawnBounds, _spawnerData.ySpawnBounds);
                    var newEntity = ecb.Instantiate(_entityIndex, _spawnerData.prefab);
                    ecb.SetComponent(_entityIndex, newEntity, LocalTransform.FromPosition(instantiatePos));
                    ecb.AddComponent(_entityIndex, newEntity, new AnchorPointData {id = i, position = instantiatePos.xy});
                }

                ecb.RemoveComponent<SpawningAnchors>(_entityIndex, _entity);
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