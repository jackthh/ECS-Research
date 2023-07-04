using System;
using _ECS_Research.Scripts.Survival_Game.Misc;
using Sirenix.OdinInspector;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [BoxGroup("Singleton"), ShowInInspector, ReadOnly] public PlayerMover Instance { get; private set; }


        [BoxGroup("References"), SerializeField] private Rigidbody selfRb;


        // [BoxGroup("Misc Config"), SerializeField] private float 


        [BoxGroup("Status Properties"), ShowInInspector, ReadOnly] public Vector3 MoveDirection { get; set; } = Vector3.zero;
        [BoxGroup("Status Properties"), ShowInInspector, ReadOnly] public Vector3 LookDirection { get; set; } = Vector3.forward;


        #region Mono Behaviour Callbacks

        private void Awake()
        {
            Instance = this;
        }


        private void FixedUpdate()
        {
            selfRb.MovePosition(selfRb.position + MoveDirection.normalized * (SOAssetsReg.Instance.gameConfig.playerConfig.movementSpeed * Time.fixedDeltaTime));
            selfRb.MoveRotation(Quaternion.LookRotation(LookDirection));
        }

        #endregion
    }
}