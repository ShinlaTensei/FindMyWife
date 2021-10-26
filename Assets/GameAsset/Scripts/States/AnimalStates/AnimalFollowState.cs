using System.Collections;
using System.Collections.Generic;
using Base.Pattern;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class AnimalFollowState : CharacterState
    {
        [SerializeField] private string moveParam = "Move";
        private NavMeshAgent _agent;
        public override void UpdateBehaviour(float dt)
        {
            
        }

        public override void PostUpdateBehaviour(float dt)
        {
            if (CharacterStateController.Animator == null) return;

            if (CharacterStateController.Animator.runtimeAnimatorController == null) return;

            if (!CharacterStateController.Animator.gameObject.activeSelf) return;
            
            if (_agent != null)
                CharacterStateController.Animator.SetFloat(moveParam, _agent.velocity.magnitude);
        }
    }
}

