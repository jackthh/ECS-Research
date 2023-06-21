using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;


namespace _ECS_Research.Scripts
{
    // [BurstCompile] public partial struct MobSpawnerSystem : ISystem
    // {
    //     [BurstCompile] public void OnUpdate(ref SystemState _state)
    //     {
    //         // Queries for all Spawner components. Uses RefRW because this system wants
    //         // to read from and write to the component. If the system only needed read-only
    //         // access, it would use RefRO instead.
    //         foreach (var spawner in SystemAPI.Query<RefRW<MobSpawnerComponentData>>())
    //         {
    //             ProcessSpawner(ref _state, spawner);
    //         }
    //     }
    //
    //
    //     private void ProcessSpawner(ref SystemState _state, RefRW<MobSpawnerComponentData> _spawner)
    //     {
    //         // If the next spawn time has passed.
    //         if (_spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
    //         {
    //             // Spawns a new entity and positions it at the spawner.
    //             var newEntity = _state.EntityManager.Instantiate(_spawner.ValueRO.prefab);
    //             // LocalPosition.FromPosition returns a Transform initialized with the given position.
    //             _state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(_spawner.ValueRO.spawnPosition));
    //
    //             // Resets the next spawn time.
    //             _spawner.ValueRW.nextSpawnTime = (float) SystemAPI.Time.ElapsedTime + _spawner.ValueRO.spawnRate;
    //         }
    //     }
    // }
}