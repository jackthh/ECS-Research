using System;
using Sirenix.OdinInspector;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [BoxGroup("Singleton"), ShowInInspector, ReadOnly] public PlayerAnimator Instance { get; private set; }


        [BoxGroup("References"), SerializeField] private Animator selfAnimator;


        #region Mono Behaviour Callbacks

        private void Awake()
        {
            Instance = this;
        }

        #endregion
    }
}