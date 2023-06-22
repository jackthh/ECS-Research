using System;
using System.Linq;
using _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;


namespace _ECS_Research.Scripts.Closest_Point_Seeker.Seekers_Spawning
{
    public partial struct SeekersSystem : ISystem
    {
        public DynamicBuffer<AnchorPointBufferElement> allAnchorPoints;


        #region ISystem Callbacks

        [BurstCompile] public void OnCreate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

            var anchorsSpawnerEntity = SystemAPI.GetSingletonEntity<AnchorPointSpawnerData>();
            allAnchorPoints = SystemAPI.GetBuffer<AnchorPointBufferElement>(anchorsSpawnerEntity);
        }


        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            _state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = GetEntityCommandBuffer(ref _state);

            new HandlerSeekersJob()
            {
                ecb = ecb,
                currentDeltaTime = SystemAPI.Time.DeltaTime,
                allAnchorPoints = allAnchorPoints
            }.ScheduleParallel();
        }

        #endregion


        #region Jobs

        [BurstCompile] public partial struct HandlerSeekersJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            public float currentDeltaTime;
            public DynamicBuffer<AnchorPointBufferElement> allAnchorPoints;


            private void Execute([EntityIndexInQuery] int _entityIndex, Entity _e, ref LocalTransform _localTransform, in SeekerConfigData _seekerConfigData,
                ref SeekerRuntimeData _seekerRuntimeData)
            {
                //  NOTE:   Null target
                if (_seekerRuntimeData.currentTarget.id == -100)
                {
                    AssignNewTarget(ref ecb, _entityIndex, _e, _seekerConfigData, ref _seekerRuntimeData, _localTransform.Position, allAnchorPoints);
                }
                else
                {
                    var step = _seekerConfigData.movementSpeed * currentDeltaTime;
                    //  NOTE:   To check if entity reached its target
                    if (math.distance(_localTransform.Position, _seekerRuntimeData.currentTarget.position) <= step)
                    {
                        AssignNewTarget(ref ecb, _entityIndex, _e, _seekerConfigData, ref _seekerRuntimeData, _localTransform.Position, allAnchorPoints);
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

        private static void AssignNewTarget(ref EntityCommandBuffer.ParallelWriter _ecb, int _entityIndex, Entity _e, SeekerConfigData _seekerConfigData,
            ref SeekerRuntimeData _seekerRuntimeData, float3 _seekerPos, DynamicBuffer<AnchorPointBufferElement> _allAnchorPoints)
        {
            var newTarget = FindNewTarget(_seekerConfigData, ref _seekerRuntimeData, _seekerPos, _allAnchorPoints);
            //  NOTE:   To check if there's no target available
            if (newTarget.id == -100)
            {
                //  NOTE:   To self destruct
                _ecb.DestroyEntity(_entityIndex, _e);
            }
            else
            {
                //  NOTE:   To assign new target
                _seekerRuntimeData.currentTarget = newTarget;
            }
        }


        private static AnchorPointBufferElement FindNewTarget(SeekerConfigData _seekerConfigData, ref SeekerRuntimeData _seekerRuntimeData, float3 _seekerPos,
            DynamicBuffer<AnchorPointBufferElement> _allAnchorPoints)
        {
            //  NOTE:   Init 
            var result = new AnchorPointBufferElement
            {
                id = -100
            };

            //  NOTE:   To find all anchors in searching radius
            var processingAnchors = new DynamicBuffer<AnchorPointBufferElement>();
            foreach (var anchor in _allAnchorPoints)
            {
                if (math.distance(_seekerPos, anchor.position) <= _seekerConfigData.searchingRadius)
                {
                    processingAnchors.Add(anchor);
                }
            }
            
            //  NOTE:   To validate the list
            for (var i = processingAnchors.Length - 1; i >= 0; i--)
            {
                if (_seekerRuntimeData.reachedPoints.Contains(processingAnchors[i]))
                {
                    processingAnchors.RemoveAt(i);
                }
            }
            
            
            //  NOTE:   To find the nearest validate anchor
            var minDistance = 2 * _seekerConfigData.searchingRadius;
            foreach (var anchor in processingAnchors)
            {
                if (math.distance(_seekerPos, anchor.position) < minDistance)
                {
                    result = anchor;
                }
            }

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