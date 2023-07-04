using System;
using Sirenix.OdinInspector;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Misc
{
    [Serializable] public class PlayerConfig
    {
        public float movementSpeed;
    }



    [CreateAssetMenu(menuName = "_L4D/Game Config")] public class GameConfig : ScriptableObject
    {
        [TabGroup("Player")] public PlayerConfig playerConfig;
        
        
        [TabGroup("Temp")] public float asdsdplayerConfig;
    }
}