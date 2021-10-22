using System.Collections;
using System.Collections.Generic;
using Base.Pattern;
using Base;
using UnityEngine;

namespace Game
{
    public class StartState : GameState
    {
        public override void CheckExitTransition()
        {
            if (GameStateController.InputAction.Phase == InputPhase.Began)
            {
                GameStateController.EnqueueTransition<PlayingState>();
            }
        }

        public override void UpdateBehaviour(float dt)
        {
            
        }
    }
}

