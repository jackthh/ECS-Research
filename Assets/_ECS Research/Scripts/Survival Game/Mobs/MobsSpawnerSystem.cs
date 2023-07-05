using Unity.Burst;
using Unity.Entities;


namespace _ECS_Research.Scripts.Survival_Game.Mobs
{
    public partial struct MobsSpawnerSystem : ISystem
    {
        #region ISystem Callbacks

        [BurstCompile] public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            var ecb = GetEntityCommandBuffer(ref _state);

            new SpawnMobsJob {ecb = ecb}.ScheduleParallel();
        }



        [BurstCompile] public partial struct SpawnMobsJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;


            private void Execute()
            {
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