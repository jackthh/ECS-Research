using System;
using System.Collections.Generic;
using _ECS_Research.Scripts.Survival_Game.General_Entities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Misc
{
    [Serializable] public struct PlayerConfig
    {
        public float moveSpeed;
        public Vector3 rotateSpeed;
    }



    [Serializable] public struct WaveConfig
    {
        public EntityIdentifierAuthoring mobPrefab;
        public int quantity;
        public float delayTimeSinceStart;
        public Vector2 spawningOffsetRange;
    }



    [CreateAssetMenu(menuName = "_L4D/Game Config")] public class GameConfig : ScriptableObject
    {
        [TabGroup("Player")] public PlayerConfig playerConfig;


        [TabGroup("Spawning Config")] public float playgroundEdgeSize;
        [TabGroup("Spawning Config")] public List<WaveConfig> wavesConfig = new();


        [TabGroup("Entity Samples"), OnCollectionChanged(nameof(AutoAssignSamplesId))] public List<EntityIdentifierAuthoring> entitySamples = new();


        #region Editor Tools

#if UNITY_EDITOR
        public void AutoAssignSamplesId()
        {
            for (var i = 0; i < entitySamples.Count; i++)
            {
                entitySamples[i].id = i;
                EditorUtility.SetDirty(entitySamples[i]);
            }
        }
#endif

        #endregion
    }
}