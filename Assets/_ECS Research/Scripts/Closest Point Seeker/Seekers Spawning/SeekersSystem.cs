/*using System;
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
        private EntityQuery anchorsQuery;


        #region ISystem Callbacks

        [BurstCompile] public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            anchorsQuery = new EntityQueryBuilder(Allocator.Persistent).WithAll<AnchorPointData>().Build(ref _state);
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            var ecb = GetEntityCommandBuffer(ref _state);

            var anchorsData = anchorsQuery.ToComponentDataArray<AnchorPointData>(Allocator.TempJob);

            new HandlerSeekersJob
            {
                ecb = ecb,
                currentDeltaTime = SystemAPI.Time.DeltaTime,
                anchorPointsData = anchorsData,
            }.ScheduleParallel();
        }

        #endregion


        #region Jobs

        [BurstCompile] public partial struct HandlerSeekersJob : IJobEntity
        {
           public EntityCommandBuffer.ParallelWriter ecb;
            [DeallocateOnJobCompletion] public float currentDeltaTime;
            [DeallocateOnJobCompletion, ] public NativeArray<AnchorPointData> anchorPointsData;


            [BurstCompile] private void Execute([EntityIndexInQuery] int _entityIndex, Entity _seekerEntity, ref LocalTransform _localTransform,
                in SeekerConfigData _seekerConfigData,
                ref SeekerRuntimeData _seekerRuntimeData, ref DynamicBuffer<AnchorPointDataElement> _reachedAnchors)
            {
                //  NOTE:   Null target
                if (_seekerRuntimeData.currentTarget.id == -100)
                {
                    AssignNewTarget(ref ecb, _entityIndex, _seekerEntity, _seekerConfigData, ref _seekerRuntimeData, _reachedAnchors, _localTransform.Position.xy,
                        anchorPointsData);
                }
                else
                {
                    var step = _seekerConfigData.movementSpeed * currentDeltaTime;
                    //  NOTE:   To check if entity reached its target
                    if (math.distance(_localTransform.Position.xy, _seekerRuntimeData.currentTarget.position) <= step)
                    {
                        _reachedAnchors.Add(new AnchorPointDataElement {id = _seekerRuntimeData.currentTarget.id});
                        AssignNewTarget(ref ecb, _entityIndex, _seekerEntity, _seekerConfigData, ref _seekerRuntimeData, _reachedAnchors, _localTransform.Position.xy,
                            anchorPointsData);
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

        private static void AssignNewTarget(ref EntityCommandBuffer.ParallelWriter _ecb, int _entityIndex, Entity _seekerEntity, SeekerConfigData _seekerConfigData,
            ref SeekerRuntimeData _seekerRuntimeData, DynamicBuffer<AnchorPointDataElement> _reachedAnchors, float2 _seekerPos, NativeArray<AnchorPointData> 
            _anchorPointsData)
        {
            var newTarget = FindNewTarget(_seekerConfigData, _reachedAnchors, _seekerPos, _anchorPointsData);
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


        private static AnchorPointData FindNewTarget(SeekerConfigData _seekerConfigData, DynamicBuffer<AnchorPointDataElement> _reachedAnchors, float2 _seekerPos,
           NativeArray<AnchorPointData> _anchorPointsData)
        {
            //  NOTE:   Init 
            var result = new AnchorPointData
            {
                id = -100
            };

            //  NOTE:   To find all anchors in searching radius
            var processingAnchors = new NativeList<AnchorPointData>(Allocator.Temp);
            
            foreach (var anchor in _anchorPointsData)
            {
                if (math.distance(_seekerPos, anchor.position) <= _seekerConfigData.searchingRadius)
                {
                    processingAnchors.Add(anchor);
                }
            }
            
            //  NOTE:   To validate the list
            for (var i = processingAnchors.Length - 1; i >= 0; i--)
            {
                foreach (var reachedAnchor in _reachedAnchors)
                {
                    if (reachedAnchor.id == processingAnchors[i].id)
                    {
                        processingAnchors.RemoveAt(i);
                        break;
                    }
                }
            }
            
            // //  NOTE:   To find the nearest validate anchor
            var minDistance = 2 * _seekerConfigData.searchingRadius;
            foreach (var anchor in processingAnchors)
            {
                var tempDistance = math.distance(_seekerPos, anchor.position);
                if (tempDistance < minDistance)
                {
                    minDistance = tempDistance;
                    result = anchor;
                }
            }

            return result;
        }


        private static void MovePosition(ref LocalTransform _localTransform, float _step, float2 _targetPos)
        {
            var direction = math.normalize(_targetPos - _localTransform.Position.xy);
            _localTransform.Position += new float3(direction.x, direction.y, 0f) * _step;
        }


        private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState _state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(_state.WorldUnmanaged);
            return ecb.AsParallelWriter();
        }

        #endregion
    }
}*/