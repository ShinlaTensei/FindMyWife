using System.Collections;
using System.Collections.Generic;
using Base.GameEventSystem;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class EndState : GameState
    {

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

