using _ECS_Research.Scripts.Survival_Game.General_Entities;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using Unity.Transforms;


namespace _ECS_Research.Scripts.Survival_Game.Mobs
{
    public partial struct MobSystem : ISystem
    {
        [BurstCompile] public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<MainScenePlayerRuntimeData>();
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            var playerPos = SystemAPI.GetSingleton<MainScenePlayerRuntimeData>();

            new MobTakeAction {playerPos = playerPos.playerPos}.ScheduleParallel();
        }



        [BurstCompile] public partial struct MobTakeAction : IJobEntity
        {
            public float3 playerPos;


            private void Execute(ref MobRuntimeData _mobRuntimeData, MobStats _mobStats, LocalTransform _localTransform, PhysicsBodyAuthoringData _physicsBody)
            {
                if (_mobRuntimeData.chasing)
                {
                    var currentDistance = playerPos - _localTransform.Position;
                    //  NOTE:   If out of attack range
                    if (math.length(currentDistance) > _mobStats.attackRange)
                    {
                        
                    }
                    //  NOTE:   To start attacking
                    else
                    {
                        
                    }
                }
                else
                {
                    _mobRuntimeData.currentAttackTime = 0f;
                    _mobRuntimeData.chasing = false;
                }
            }
        }
    }
}