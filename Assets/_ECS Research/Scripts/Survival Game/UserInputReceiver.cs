using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace _ECS_Research.Scripts.Survival_Game
{
    public class InputHandler : MonoBehaviour
    {
        public PlayerInput input;


        private void Awake()
        {
            input = GetComponent<PlayerInput>();
        }


       


        public void Move(InputAction.CallbackContext _context)
        {
            Debug.Log($"Received {_context.ReadValue<Vector2>().normalized}" );
        }


        public void TestTouch(InputAction.CallbackContext _context)
        {
            Debug.Log($"Received Phase: {_context.phase} \n" +
                      $"Pos: {_context.ReadValue<Vector2>()}");
        }
    }
    
}
