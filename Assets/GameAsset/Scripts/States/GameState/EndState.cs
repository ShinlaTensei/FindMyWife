using System.Collections;
using System.Collections.Generic;
using Base.GameEventSystem;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class EndState : GameState
    {
        [SerializeField] private GameEvent endGameNotify;

        public override void EnterStateBehaviour(float dt, GameState fromState)
        {
            base.EnterStateBehaviour(dt, fromState);
            
            endGameNotify.InvokeEvent();
        }

        public override void CheckExitTransition()
        {
            if (GameManager.GameStatisticParam.isReplay)
            {
                GameStateController.EnqueueTransition<ReplayState>();
            }
        }

        public override void UpdateBehaviour(float dt)
        {
            
        }
    }
}

