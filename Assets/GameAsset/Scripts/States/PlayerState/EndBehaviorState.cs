using System;
using System.Collections;
using System.Collections.Generic;
using Base.Module;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class EndBehaviorState : CharacterState
    {
        [SerializeField] private string animatorParam = String.Empty;

        private PlayerController _playerController;

        protected override void Awake()
        {
            base.Awake();

            _playerController = this.GetComponentInBranch<PlayerController>();
        }

        public override void EnterState(float dt, CharacterState fromState)
        {
            
        }

        public override void UpdateBehaviour(float dt)
        {
            
        }

        public override void PostUpdateBehaviour(float dt)
        {
            if (CharacterStateController.Animator == null) return;
            if (CharacterStateController.Animator.runtimeAnimatorController == null) return;
            if (!CharacterStateController.Animator.gameObject.activeSelf) return;
            
            var @this = Convert.ToInt32(_playerController.McStatisticParam.isPlayerWin);
            CharacterStateController.Animator.SetFloat(animatorParam, @this);
        }
    }
}

