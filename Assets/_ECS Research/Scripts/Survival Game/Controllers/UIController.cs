using Sirenix.OdinInspector;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Controllers
{
    public class UIController : MonoBehaviour
    {
        [BoxGroup("Singleton"), ShowInInspector, ReadOnly] public static UIController Instance { get; private set; }


        private void Awake()
        {
            Instance = this;
        }
    }
}