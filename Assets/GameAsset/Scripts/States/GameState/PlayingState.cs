using System.Collections;
using System.Collections.Generic;
using Base;
using Base.GameEventSystem;
using Base.MessageSystem;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class PlayingState : GameState
    {
        [SerializeField] private GameEvent playingStateNotify;
        private InputAction inputAction => GameStateController.InputAction;

        public override void EnterStateBehaviour(float dt, GameState fromState)
        {
            base.EnterStateBehaviour(dt, fromState);
            playingStateNotify.InvokeEvent();
        }

        public override void UpdateBehaviour(float dt)
        {
            Messenger.RaiseMessage(SystemMessage.Input, inputAction.Phase, inputAction.Position);
        }
    } 
}

