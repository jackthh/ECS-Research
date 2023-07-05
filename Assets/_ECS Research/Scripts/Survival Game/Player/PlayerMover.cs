using System;
using _ECS_Research.Scripts.Survival_Game.Misc;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [BoxGroup("Singleton"), ShowInInspector, ReadOnly] public PlayerMover Instance { get; private set; }


        [BoxGroup("References")] private Rigidbody selfRb;


        // [BoxGroup("Misc Config"), SerializeField] private float 


        [BoxGroup("Status Properties"), ShowInInspector, ReadOnly] public Vector3 MoveDirection { get; set; } = Vector3.zero;
        [BoxGroup("Status Properties"), ShowInInspector, ReadOnly] public Vector3 TargetLookDirection { get; set; } = Vector3.forward;


        #region Mono Behaviour Callbacks

        private void Awake()
        {
            Instance = this;
            selfRb = GetComponent<Rigidbody>();
        }


        private void FixedUpdate()
        {
            if (InputHandler.Instance.GotTouch)
            {
                MoveDirection = new Vector3(InputHandler.Instance.CurrentDirection.x, 0f, InputHandler.Instance.CurrentDirection.y);

                var moveStep = SOAssetsReg.Instance.gameConfig.playerConfig.moveSpeed * Time.fixedDeltaTime;
                var moveTarget = selfRb.position + MoveDirection.normalized * moveStep;

                //  NOTE:   To gradually rotate player toward a virtual target
                var targetLookRotation = Quaternion.LookRotation(MoveDirection);
                TargetLookDirection = targetLookRotation.eulerAngles;
                var deltaAngle = SOAssetsReg.Instance.gameConfig.playerConfig.rotateSpeed * Time.fixedDeltaTime;
                var rotateStep = selfRb.rotation.eulerAngles.y > TargetLookDirection.y ? deltaAngle : -deltaAngle;
                selfRb.MoveRotation(Quaternion.Euler(TargetLookDirection + rotateStep));

                //  NOTE:   To assure player won't move outside the playground
                moveTarget = moveTarget.MMSetX(Mathf.Clamp(moveTarget.x, -24.5f, 24.5f));
                moveTarget = moveTarget.MMSetZ(Mathf.Clamp(moveTarget.z, -24.5f, 24.5f));
                selfRb.MovePosition(moveTarget);
            }
        }

        #endregion
    }
}