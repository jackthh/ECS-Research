using Unity.Entities;
using Unity.Mathematics;


namespace _ECS_Research.Scripts.Closest_Point_Seeker.Seekers_Spawning
{
	public struct SeekersSpawnerData : IComponentData
	{
		public Entity prefab;
		public float interval;
		public float nextSpawnTime;
		public int amountPerWave;
		public float2 xSpawnBounds;
		public float2 ySpawnBounds;
		public float seekerMovementSpeed;
		public float seekerSearchingRadius;
	}
}
