using Sirenix.OdinInspector;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [BoxGroup("Singleton"), ShowInInspector, ReadOnly] public PlayerMover Instance { get; private set; }


        #region Mono Behaviour Callbacks

        private void Awake()
        {
            Instance = this;
        }

        #endregion
    }
}