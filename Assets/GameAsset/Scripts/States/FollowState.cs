using Base.Module;
using Base.Pattern;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class FollowState : CharacterState
    {
        [SerializeField] private string moveParam = "Move";

        private NpcController _npcController = null;
        
        private NavMeshAgent _agent;

        private Transform _followTarget = null;

        protected override void Awake()
        {
            base.Awake();

            _agent = this.GetComponentInBranch<NpcController, NavMeshAgent>();
            _npcController = this.GetComponentInBranch<NpcController, NpcController>();
        }

        public override void EnterState(float dt, CharacterState fromState)
        {
            if (_agent == null)
            {
                _agent = this.GetComponentInBranch<PlayerController, NavMeshAgent>();
            }

            _followTarget = _npcController.TransformToFollow;
        }

        public override void ExitStateBehaviour(float dt, CharacterState toState)
        {
            _followTarget = null;
        }

        public override void UpdateBehaviour(float dt)
        {
            if (_followTarget != null)
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
    }
}

