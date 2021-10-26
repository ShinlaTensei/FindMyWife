using System.Collections;
using System.Collections.Generic;
using Base.Module;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class McKissState : CharacterState
    {
        private PlayerController playerController;

        private float animationClipLength = 0;

        private float totalTime = 0;

        protected override void Start()
        {
            base.Start();

            playerController = this.GetComponentInBranch<PlayerController, PlayerController>();
        }

        public override void EnterState(float dt, CharacterState fromState)
        {
            CharacterStateController.Animator.GetCurrentClipLength(ref animationClipLength);
        }

        public override void ExitStateBehaviour(float dt, CharacterState toState)
        {
            totalTime = 0;
        }

        public override void CheckExitTransition()
        {
            if (totalTime >= animationClipLength)
            {
                playerController.NpcDetected.KissReaction();
                playerController.McStatisticParam.isNpcDetected = false;
                if (!playerController.McStatisticParam.isCorrectTarget)
                {
                    CharacterStateController.EnqueueTransition<WaitingResultState>();
                }
                else
                {
                    CharacterStateController.EnqueueTransition<EndBehaviorState>();
                }
            }
        }

        public override void UpdateBehaviour(float dt)
        {
            totalTime += dt;
        }
    }
}

