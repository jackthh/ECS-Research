using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace _ECS_Research.Scripts
{
    public partial struct MobMovementSystem : ISystem
    {
        [BurstCompile] public void OnUpdate(ref SystemState _state)
        {
            new Move
            {
                deltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel();
        }
    }



    [BurstCompile] public partial struct Move : IJobEntity
    {
        public float deltaTime;


        private void Execute(ref MobMovementComponentData _movementComponent, ref LocalTransform _localTransform)
        {
            if (_movementComponent.movingUp)
            {
                if (_localTransform.Position.y >= _movementComponent.maxY)
                {
                    _movementComponent.movingUp = false;
                }
            }
            else
            {
                if (_localTransform.Position.y <= _movementComponent.minY)
                {
                    _movementComponent.movingUp = true;
                }
            }

            _localTransform.Position += _movementComponent.movingUp
                ? new float3(0f, deltaTime * _movementComponent.speed, 0f)
                : new float3(0f, -deltaTime * _movementComponent.speed, 0f);
        }
    }
}