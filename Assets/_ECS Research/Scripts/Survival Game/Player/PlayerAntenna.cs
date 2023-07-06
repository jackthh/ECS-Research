using System;
using _ECS_Research.Scripts.Survival_Game.General_Entities;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Player
{
    public class PlayerAntenna : MonoBehaviour
    {
        private EntityQuery componentDataQuery;


        private void Start()
        {
            componentDataQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<MainScenePlayerRuntimeData>().Build(World.DefaultGameObjectInjectionWorld.EntityManager);
        }


        private void Update()
        {
            //  NOTE:   To sync player pos
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(componentDataQuery.GetSingletonEntity(),
                new MainScenePlayerRuntimeData {playerPos = transform.position});
            
            //  NOTE:   To check 
        }
    }
}