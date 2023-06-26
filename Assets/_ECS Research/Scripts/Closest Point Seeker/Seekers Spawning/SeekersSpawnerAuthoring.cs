using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace _ECS_Research.Scripts.Closest_Point_Seeker.Seekers_Spawning
{
	public class SeekersSpawnerAuthoring : MonoBehaviour
	{
		public GameObject seekerPrefab;
		public float interval;
		public int amountPerWave;
		public Vector2 xSpawnBounds;
		public Vector2 ySpawnBounds;
		public float seekerMovementSpeed;
		public float seekerSearchingRadius;
	}

	public class SeekersSpawnerBaker : Baker<SeekersSpawnerAuthoring>
	{
		public override void Bake(SeekersSpawnerAuthoring _authoring)
		{
			var entity = GetEntity(TransformUsageFlags.None);
		  
			AddComponent(entity, new SeekersSpawnerData
			{
				prefab = GetEntity(_authoring.seekerPrefab, TransformUsageFlags.Dynamic),
				interval = _authoring.interval,
				nextSpawnTime = _authoring.interval,
				amountPerWave = _authoring.amountPerWave,
				xSpawnBounds = _authoring.xSpawnBounds,
				ySpawnBounds = _authoring.ySpawnBounds,
				seekerMovementSpeed = _authoring.seekerMovementSpeed,
				seekerSearchingRadius = _authoring.seekerSearchingRadius		  
			});
		}
	}
}