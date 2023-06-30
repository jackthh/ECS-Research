using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace _ECS_Research.Scripts.Survival_Game
{
    public class InputHanndler : MonoBehaviour
    {
        public PlayerInput input;


        private void Awake()
        {
            input = GetComponent<PlayerInput>();
        }


        public void Move(InputAction.CallbackContext _context)
        {
            Debug.Log($"Received {_context.ReadValue<Vector2>()}" );
        }
    }
    
}
