using System.Collections;
using System.Collections.Generic;
using Base;
using Base.MessageSystem;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class PlayingState : GameState
    {
        private InputAction inputAction => GameStateController.InputAction;
        public override void UpdateBehaviour(float dt)
        {
            Messenger.RaiseMessage(SystemMessage.Input, inputAction.Phase, inputAction.Position);
        }
    } 
}

