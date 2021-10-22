using System.Collections;
using System.Collections.Generic;
using Base;
using Base.MessageSystem;
using Base.Module;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class NormalMovementState : CharacterState
    {
        private CharacterController _characterController = null;

        protected override void Start()
        {
            base.Start();

            _characterController = this.GetComponentInBranch<CharacterController, CharacterController>();
        }

        public override void EnterState(float dt, CharacterState fromState)
        {
            Messenger.RegisterListener<InputPhase, Vector3>(SystemMessage.Input, OnInputResponse);
        }

        public override void ExitStateBehaviour(float dt, CharacterState toState)
        {
            Messenger.RemoveListener<InputPhase, Vector3>(SystemMessage.Input, OnInputResponse);
        }

        public override void UpdateBehaviour(float dt)
        {
            
        }

        private void OnInputResponse(InputPhase phase, Vector3 inputPos)
        {
            
        }
    }
}

