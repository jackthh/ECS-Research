using _ECS_Research.Scripts.Survival_Game.Misc;
using Sirenix.OdinInspector;
using UnityEngine;


namespace _ECS_Research.Z_Testing
{
    public class GlobalSetup : MonoBehaviour
    {
        [BoxGroup("References"), SerializeField] private SOAssetsReg soAssetsReg;


        private void Start()
        {
            Application.targetFrameRate = 120;
        }
    }
}