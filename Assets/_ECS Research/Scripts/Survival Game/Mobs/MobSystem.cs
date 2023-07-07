using _ECS_Research.Scripts.Survival_Game.General_Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using Unity.Transforms;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Mobs
{
    public partial struct MobSystem : ISystem
    {
        private EntityQuery playerRuntimeDataEntityQuery;


        [BurstCompile] public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<MainScenePlayerRuntimeData>();
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

            playerRuntimeDataEntityQuery = new EntityQueryBuilder(Allocator.Persistent).WithAll<MainScenePlayerRuntimeData>().Build(ref _state);
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            var temp1 = playerRuntimeDataEntityQuery.GetSingletonRW<MainScenePlayerRuntimeData>();
            var temp = World.DefaultGameObjectInjectionWorld.EntityManager.component

            new MobTakeAction
            {
                playerRuntimeData = getcomponent
                deltaTime = SystemAPI.Time.DeltaTime,
                elapsedTime = SystemAPI.Time.ElapsedTime
            }.ScheduleParallel();
        }



        [BurstCompile] public partial struct MobTakeAction : IJobEntity
        {
            [DeallocateOnJobCompletion] public ComponentLookup<MainScenePlayerRuntimeData> playerRuntimeData;
            public float deltaTime;
            public double elapsedTime;


            private void Execute(ref MobRuntimeData _mobRuntimeData, MobStats _mobStats, ref LocalTransform _localTransform)
            {
                var currentDistance = playerRuntimeData.ValueRO.playerPos - _localTransform.Position;
                //  NOTE:   If out of attack range, chase
                if (math.length(currentDistance) > _mobStats.attackRange)
                {
                    _mobRuntimeData.chasing = true;
                    var step = math.normalize(currentDistance) * deltaTime * _mobStats.movementSpeed;
                    _localTransform.Position += step;
                    _localTransform.Rotation = Quaternion.LookRotation(math.normalize(currentDistance), Vector3.up);
                }
                //  NOTE:   If reached attack range, try to attack
                else
                {
                    //  NOTE:   To transit into attacking state
                    if (_mobRuntimeData.chasing)
                    {
                        _mobRuntimeData.chasing = false;
                        _mobRuntimeData.currentAttackTime = 0f;
                        return;
                    }

                    //  NOTE:   To strike
                    _mobRuntimeData.currentAttackTime += deltaTime;
                    if (_mobRuntimeData.currentAttackTime >= _mobStats.attackTime)
                    {
                        _mobRuntimeData.nextAttackCDTime = elapsedTime + _mobStats.attackInterval;
                        playerRuntimeData.ValueRW.playerHP -= _mobStats.attackDmg;
                        // damageQueue.Add(new DamageReceivedElement {damage = _mobStats.attackDmg});
                    }
                }
            }
        }
    }
}