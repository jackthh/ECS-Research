using System;
using System.Collections;
using _ECS_Research.Scripts.Closest_Point_Seeker.Anchor_Points_Spawning;
using _ECS_Research.Scripts.Closest_Point_Seeker.Seekers_Spawning;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


namespace _ECS_Research.Scripts.Closest_Point_Seeker.UI_Stuffs
{
    public class UIController : MonoBehaviour
    {
        public TMP_Text anchorsCountText;
        public TMP_Text seekerssCountText;


        private EntityQuery archonsQuery;
        private EntityQuery seekersQuery;


        private IEnumerator Start()
        {
            Application.targetFrameRate = 300;
            yield return new WaitForSeconds(0.5f);

            archonsQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<AnchorPointData>());
            seekersQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<SeekerConfigData>());


            while (true)
            {
                yield return new WaitForSeconds(0.5f);

                anchorsCountText.text = $"Anchors: {archonsQuery.CalculateEntityCount()}";
                seekerssCountText.text = $"Seekers: {seekersQuery.CalculateEntityCount()}";
            }
        }
    }
}