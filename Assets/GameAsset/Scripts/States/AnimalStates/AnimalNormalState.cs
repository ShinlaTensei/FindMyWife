using System.Collections;
using System.Collections.Generic;
using Base.Module;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class AnimalNormalState : CharacterState
    {
        [SerializeField] private string animatorParamName = "Idle";

        private AnimalController _animalController = null;

        protected override void Awake()
        {
            base.Awake();

            _animalController = this.GetComponentInBranch<AnimalController>();
        }

        public override bool CheckEnterTransition(CharacterState fromState)
        {
            return !_animalController.NpcStatisticParam.isAngry && !_animalController.NpcStatisticParam.isFollow;
        }

        public override void UpdateBehaviour(float dt) { }

        public override void CheckExitTransition()
        {
            if (_animalController.NpcStatisticParam.isAngry)
            {
                CharacterStateController.EnqueueTransition<AnimalAngryState>();
            }
            else if (_animalController.NpcStatisticParam.isFollow)
            {
                CharacterStateController.EnqueueTransition<AnimalFollowState>();
            }
        }

        public override void PostUpdateBehaviour(float dt)
        {
            if (CharacterStateController.Animator == null) return;
            if (CharacterStateController.Animator.runtimeAnimatorController == null) return;
            if (!CharacterStateController.Animator.gameObject.activeSelf) return;
            
            CharacterStateController.Animator.SetFloat(animatorParamName, _animalController.NpcStatisticParam.idleValue);
        }
    }
}

