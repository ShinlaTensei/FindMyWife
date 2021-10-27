using System.Collections;
using System.Collections.Generic;
using Base.Module;
using Base.Pattern;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class AnimalFollowState : CharacterState
    {
        [SerializeField] private string moveParam = "Move";

        private AnimalController _animalController;

        private NavMeshAgent _agent;

        private Transform _followTarget = null;

        protected override void Awake()
        {
            base.Awake();

            _animalController = this.GetComponentInBranch<AnimalController>();
            _agent = _animalController.Agent;
        }

        public override void EnterState(float dt, CharacterState fromState)
        {
            _followTarget = _animalController.TransformToFollow;
        }

        public override void UpdateBehaviour(float dt)
        {
            if (_followTarget)
            {
                _agent.destination = _followTarget.position;
            }
        }

        public override void PostUpdateBehaviour(float dt)
        {
            if (CharacterStateController.Animator == null) return;

            if (CharacterStateController.Animator.runtimeAnimatorController == null) return;

            if (!CharacterStateController.Animator.gameObject.activeSelf) return;
            
            if (_agent != null)
                CharacterStateController.Animator.SetFloat(moveParam, _agent.velocity.magnitude);
        }

        public override void ExitStateBehaviour(float dt, CharacterState toState)
        {
            _followTarget = null;
        }
    }
}

