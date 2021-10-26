using System.Collections;
using System.Collections.Generic;
using Base.Pattern;
using Base;
using Base.GameEventSystem;
using UnityEngine;

namespace Game
{
    public class StartState : GameState
    {
        [SerializeField] private GameEvent startStateNotify;
        public override void CheckExitTransition()
        {
            if (GameStateController.InputAction.Phase == InputPhase.Began)
            {
                GameStateController.EnqueueTransition<PlayingState>();
            }
        }

        public override void EnterStateBehaviour(float dt, GameState fromState)
        {
            base.EnterStateBehaviour(dt, fromState);
            startStateNotify.InvokeEvent(new GameEventData());
        }

        public override void ExitStateBehaviour(float dt, GameState toState)
        {
            base.ExitStateBehaviour(dt, toState);
        }

        public override void UpdateBehaviour(float dt)
        {
            
        }
    }
}

