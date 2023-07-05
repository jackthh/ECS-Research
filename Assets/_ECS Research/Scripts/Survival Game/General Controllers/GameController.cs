using System;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;


namespace _ECS_Research.Scripts.Survival_Game.Controllers
{
    public enum GameStates
    {
        READY,
        PLAYING,
        WON,
        LOST
    }



    public class GameController : MonoBehaviour
    {
        [BoxGroup("Singleton"), ShowInInspector, ReadOnly] public static GameController Instance { get; private set; }


        [BoxGroup("Status Properties"), ShowInInspector, ReadOnly] public MMStateMachine<GameStates> GameState { get; private set; }


        #region Mono Behaviour Callbacks

        private void Awake()
        {
            Instance = this;
            GameState = new MMStateMachine<GameStates>(gameObject, false);
            GameState.ChangeState(GameStates.READY);
        }

        #endregion
    }
}