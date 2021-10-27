using System.Collections;
using System.Collections.Generic;
using Base.Module;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class WomanNormalState : CharacterState
    {
        [SerializeField] private string animatorParamName = "Normal";

        private NpcController _npcController;

        protected override void Awake()
        {
            base.Awake();

            _npcController = this.GetComponentInBranch<CharacterController, NpcController>();
        }

        public override void UpdateBehaviour(float dt)
        {
            
        }

        public override bool CheckEnterTransition(CharacterState fromState)
        {
            return !_npcController.NpcStatisticParam.isAngry && !_npcController.NpcStatisticParam.isFollow;
        }

        public override void CheckExitTransition()
        {
            if (_npcController.NpcStatisticParam.isAngry)
            {
                _npcController.NpcStatisticParam.isAngry = false;
                CharacterStateController.EnqueueTransition<WomanAngryState>();
            }
            else if (_npcController.NpcStatisticParam.isFollow)
            {
                CharacterStateController.EnqueueTransition<FollowState>();
            }
        }

        public override void PostUpdateBehaviour(float dt)
        {
            if (CharacterStateController.Animator == null)
            {
                return;
            }

            if (CharacterStateController.Animator.runtimeAnimatorController == null)
            {
                return;
            }

            if (!CharacterStateController.Animator.gameObject.activeSelf)
            {
                return;
            }
            
            CharacterStateController.Animator.SetFloat(animatorParamName, _npcController.NpcStatisticParam.idleValue);
        }
    }
}

