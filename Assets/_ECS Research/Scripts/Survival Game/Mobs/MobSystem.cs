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
            var playerRuntimeData = SystemAPI.GetSingleton<MainScenePlayerRuntimeData>();

            new MobTakeAction
            {
                playerRuntimeData = playerRuntimeData,
                deltaTime = SystemAPI.Time.DeltaTime,
                elapsedTime = SystemAPI.Time.ElapsedTime
            }.ScheduleParallel();
        }



        [BurstCompile] public partial struct MobTakeAction : IJobEntity
        {
            public MainScenePlayerRuntimeData playerRuntimeData;
            public float deltaTime;
            public double elapsedTime;


            private void Execute(ref MobRuntimeData _mobRuntimeData, MobStats _mobStats, ref LocalTransform _localTransform)
            {
                if (_mobRuntimeData.chasing)
                {
                    var currentDistance = playerRuntimeData.playerPos - _localTransform.Position;
                    //  NOTE:   If out of attack range
                    if (math.length(currentDistance) > _mobStats.attackRange)
                    {
                        var step = math.normalize(currentDistance) * deltaTime * _mobStats.movementSpeed;
                        // var step = new float3(1f,0f,1f);
                        _localTransform.Position += step;
                    }
                    //  NOTE:   To start attacking
                    else
                    {
                        _mobRuntimeData.currentAttackTime = 0f;
                        _mobRuntimeData.chasing = false;
                    }
                }
                //  NOTE:   While attacking
                else
                {
                    _mobRuntimeData.currentAttackTime += deltaTime;
                    if (_mobRuntimeData.currentAttackTime >= _mobStats.attackTime)
                    {
                        //  TODO: To attack
                        _mobRuntimeData.nextAttackCDTime = elapsedTime + _mobStats.attackInterval;
                        playerRuntimeData.playerHP -= _mobStats.attackDmg;
                    }
                }
            }
        }
    }
}