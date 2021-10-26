using System.Collections;
using System.Collections.Generic;
using Base.Module;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class WomanAngryState : CharacterState
    {
        private float _crrClipLength = 0;
        private float _totalTime = 0;

        public override void EnterState(float dt, CharacterState fromState)
        {
            CharacterStateController.Animator.GetCurrentClipLength(ref _crrClipLength);
        }

        public override void ExitStateBehaviour(float dt, CharacterState toState)
        {
            _totalTime = 0;
        }

        public override void CheckExitTransition()
        {
            if (_totalTime >= _crrClipLength)
            {
                CharacterStateController.EnqueueTransition<WomanNormalState>();
            }
        }

        public override void UpdateBehaviour(float dt)
        {
            _totalTime += dt;
        }
    }
}

