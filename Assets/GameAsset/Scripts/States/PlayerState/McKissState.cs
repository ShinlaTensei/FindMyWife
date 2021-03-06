using Base;
using Base.Module;
using Base.Pattern;
using UnityEngine;
using Base.MessageSystem;

namespace Game
{
    public class McKissState : CharacterState
    {
        [SerializeField] private string actionParam = "Action";
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
            CharacterStateController.Animator.SetFloat(actionParam, Mathf.Clamp01(playerController.McStatisticParam.actionValue));
        }

        public override void ExitStateBehaviour(float dt, CharacterState toState)
        {
            totalTime = 0;
            playerController.NpcDetected.InteractReaction(CacheTransform);
            // if (playerController.McStatisticParam.isCorrectTarget)
            // {
            //     TargetData targetData = playerController.NpcDetected.TargetData;
            //     Messenger.RaiseMessage(GameMessage.ObjectiveCheck, targetData.TargetType, targetData.PrefabId);
            // }
        }

        public override void CheckExitTransition()
        {
            if (totalTime >= animationClipLength)
            {
                playerController.McStatisticParam.isNpcDetected = false;
                if (!playerController.McStatisticParam.isCorrectTarget)
                {
                    CharacterStateController.EnqueueTransition<WaitingResultState>();
                }
                else
                {
                    CharacterStateController.EnqueueTransition<NormalMovementState>();
                }
            }
        }

        public override void PreUpdateBehaviour(float dt)
        {
            CharacterStateController.Animator.GetCurrentClipLength(ref animationClipLength);
        }

        public override void UpdateBehaviour(float dt)
        {
            totalTime += dt;
        }

        public override void PostUpdateBehaviour(float dt)
        {
            if (CharacterStateController.Animator == null) return;
            if (CharacterStateController.Animator.runtimeAnimatorController == null) return;
            if (!CharacterStateController.Animator.gameObject.activeSelf) return;
            
            CharacterStateController.Animator.SetFloat(actionParam, Mathf.Clamp01(playerController.McStatisticParam.actionValue));
        }
    }
}

