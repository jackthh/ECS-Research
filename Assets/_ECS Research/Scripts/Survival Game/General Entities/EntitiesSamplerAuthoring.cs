using System.Collections.Generic;
using _ECS_Research.Scripts.Survival_Game.Mob;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ECS_Research.Scripts.Survival_Game.General_Entities
{
    public class EntitiesSamplerAuthoring : MonoBehaviour
    {
        [BoxGroup("Config Data")] public List<EntityIdentifierAuthoring> entitySamples = new();


        #region Editor Tools

#if UNITY_EDITOR
        [Button(ButtonSizes.Large)] public void AssignId()
        {
            for (var i = 0; i < entitySamples.Count; i++)
            {
                entitySamples[i].id = i;
            }
        }
#endif

        #endregion
    }
}