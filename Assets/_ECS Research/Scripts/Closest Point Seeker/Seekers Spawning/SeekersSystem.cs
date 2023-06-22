using System;
using System.Linq;
using _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;


namespace _ECS_Research.Scripts.Closest_Point_Seeker.Seekers_Spawning
{
    [UpdateAfter(typeof(SeekersSpawnerSystem))] public partial struct SeekersSystem : ISystem
    {
        private BufferLookup<AnchorPointBufferElement> anchorElementBufferLookup;


        #region ISystem Callbacks

        [BurstCompile] public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            anchorElementBufferLookup = _state.GetBufferLookup<AnchorPointBufferElement>(true);
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = GetEntityCommandBuffer(ref _state);
            anchorElementBufferLookup.Update(ref _state);

            new HandlerSeekersJob()
            {
                ecb = ecb,
                currentDeltaTime = SystemAPI.Time.DeltaTime,
                anchorElementBufferLookup = anchorElementBufferLookup,
                spawnerEntity = SystemAPI.GetSingletonEntity<AnchorPointSpawnerData>()
            }.ScheduleParallel();
        }

        #endregion


        #region Jobs

        [BurstCompile] public partial struct HandlerSeekersJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            public float currentDeltaTime;
            [ReadOnly] public BufferLookup<AnchorPointBufferElement> anchorElementBufferLookup;
            public Entity spawnerEntity;


            private void Execute([EntityIndexInQuery] int _entityIndex, Entity _seekerEntity, ref LocalTransform _localTransform, in SeekerConfigData _seekerConfigData,
                ref SeekerRuntimeData _seekerRuntimeData)
            {
                //  NOTE:   Null target
                if (_seekerRuntimeData.currentTarget.id == -100)
                {
                    AssignNewTarget(ref ecb, _entityIndex, _seekerEntity, spawnerEntity, _seekerConfigData, ref _seekerRuntimeData, _localTransform.Position,
                        anchorElementBufferLookup);
                }
                else
                {
                    var step = _seekerConfigData.movementSpeed * currentDeltaTime;
                    //  NOTE:   To check if entity reached its target
                    if (math.distance(_localTransform.Position, _seekerRuntimeData.currentTarget.position) <= step)
                    {
                        anchorElementBufferLookup[_seekerEntity].Add(_seekerRuntimeData.currentTarget);
                        AssignNewTarget(ref ecb, _entityIndex, _seekerEntity, spawnerEntity, _seekerConfigData, ref _seekerRuntimeData, _localTransform.Position,
                            anchorElementBufferLookup);
                    }
                    else
                    {
                        MovePosition(ref _localTransform, step, _seekerRuntimeData.currentTarget.position);
                    }
                }
            }
        }

        #endregion


        #region Workers

        private static void AssignNewTarget(ref EntityCommandBuffer.ParallelWriter _ecb, int _entityIndex, Entity _seekerEntity, Entity _spawnerEntity,
            SeekerConfigData _seekerConfigData, ref SeekerRuntimeData _seekerRuntimeData, float3 _seekerPos,
            BufferLookup<AnchorPointBufferElement> _anchorElementsBufferLookup)
        {
            var newTarget = FindNewTarget(_seekerEntity, _spawnerEntity, _seekerConfigData, _seekerPos, _anchorElementsBufferLookup);
            //  NOTE:   To check if there's no target available
            if (newTarget.id == -100)
            {
                //  NOTE:   To self destruct
                _ecb.DestroyEntity(_entityIndex, _seekerEntity);
            }
            else
            {
                //  NOTE:   To assign new target
                _seekerRuntimeData.currentTarget = newTarget;
            }
        }


        private static AnchorPointBufferElement FindNewTarget(Entity _seekerEntity, Entity _spawnerEntity, SeekerConfigData _seekerConfigData, float3 _seekerPos,
            BufferLookup<AnchorPointBufferElement> _anchorElementBufferLookup)
        {
            //  NOTE:   Init 
            var result = new AnchorPointBufferElement
            {
                id = -100
            };

            //  NOTE:   To find all anchors in searching radius
            var processingAnchors = new NativeList<AnchorPointBufferElement>();
            // var processingAnchors = new NativeList<int>();
            var processingAnchorsPos = new NativeList<float3>();
            foreach (var anchor in _anchorElementBufferLookup[_spawnerEntity])
            {
                if (math.distance(_seekerPos, anchor.position) <= _seekerConfigData.searchingRadius)
                {
                    processingAnchors.Add(anchor);
                    //     processingAnchors.Add(anchor.id);
                    //     processingAnchorsPos.Add(anchor.position);
                }
            }

            //  NOTE:   To validate the list
            // for (var i = processingAnchors.Length - 1; i >= 0; i--)
            // {
            //     if (_anchorElementBufferLookup[_seekerEntity].Contains(processingAnchors[i]))
            //     {
            //         processingAnchors.RemoveAt(i);
            //     }
            // }
            //
            //
            // //  NOTE:   To find the nearest validate anchor
            // var minDistance = 2 * _seekerConfigData.searchingRadius;
            // foreach (var anchor in processingAnchors)
            // {
            //     if (math.distance(_seekerPos, anchor.position) < minDistance)
            //     {
            //         result = anchor;
            //     }
            // }

            return result;
        }


        private static void MovePosition(ref LocalTransform _localTransform, float _step, float3 _targetPos)
        {
            var direction = math.normalize(_targetPos - _localTransform.Position);
            _localTransform.Position += direction * _step;
        }


        private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState _state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(_state.WorldUnmanaged);
            return ecb.AsParallelWriter();
        }

        #endregion
    }
}