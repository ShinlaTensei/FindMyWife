using System;
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
        [SerializeField] private float speedLerpRotate = .1f;
        [SerializeField] private float moveSpeed = 1;
        [SerializeField] private string speedParam = "Velocity";
        
        private Vector3 _posClickOld;
        private Vector3 _deltaClick;
        private Vector3 _lookDir;
        private bool _isTouch = false;
        private float _horizontalMove = 0;
        
        private CharacterController _characterController = null;
        private PlayerController _playerController;

        protected override void Start()
        {
            base.Start();

            _characterController = this.GetComponentInBranch<PlayerController, CharacterController>();
            _playerController = this.GetComponentInBranch<PlayerController, PlayerController>();
        }

        public override bool CheckEnterTransition(CharacterState fromState)
        {
            return !_playerController.McStatisticParam.isNpcDetected;
        }

        public override void EnterState(float dt, CharacterState fromState)
        {
            Messenger.RegisterListener<InputPhase, Vector3>(SystemMessage.Input, OnInputResponse);
        }

        public override void ExitStateBehaviour(float dt, CharacterState toState)
        {
            Messenger.RemoveListener<InputPhase, Vector3>(SystemMessage.Input, OnInputResponse);
        }

        public override void CheckExitTransition()
        {
            if (_playerController.McStatisticParam.isNpcDetected)
            {
                CharacterStateController.EnqueueTransition<McKissState>();
            }
            else if (_playerController.McStatisticParam.isLevelEnd)
            {
                CharacterStateController.EnqueueTransition<EndBehaviorState>();
            }
        }

        public override void UpdateBehaviour(float dt)
        {
            if (_playerController.McStatisticParam.isCarDetected)
            {
                _playerController.McStatisticParam.isCarDetected = false;
                TargetData targetData = _playerController.McStatisticParam.carDetected;
                Objective currentObjective = _playerController.ObjectiveController.CurrentObjective;
                if (targetData.PrefabId == currentObjective.objectiveId)
                {
                    Messenger.RaiseMessage(GameMessage.ObjectiveCheck, targetData.TargetType, targetData.PrefabId, targetData.transform.root.position);
                }
            }
        }

        protected override void Update()
        {
            if (_isTouch && !_playerController.McStatisticParam.isLevelEnd)
            {
                HandlingMovement();
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
            
            CharacterStateController.Animator.SetFloat(speedParam, _horizontalMove);
        }


        private void OnInputResponse(InputPhase phase, Vector3 inputPos)
        {
            switch (phase)
            {
                case InputPhase.Began:
                    _posClickOld = inputPos;
                    _isTouch = true;
                    _horizontalMove = 1;
                    break;
                case InputPhase.Moved:
                    _deltaClick = inputPos - _posClickOld;
                    _posClickOld = inputPos;
                    break;
                case InputPhase.Ended:
                    _deltaClick = Vector3.zero;
                    _lookDir = Vector3.zero;
                    _horizontalMove = 0;
                    _playerController.PulseSensor();
                    _isTouch = false;
                    break;
                default:
                    break;
            }
        
            HandlingInput();
        }
        
        private void HandlingInput()
        {
            if (_deltaClick != Vector3.zero)
            {
                Vector3 deltaTouch = _deltaClick;
                deltaTouch.z = deltaTouch.y;
                deltaTouch.y = 0;

                _lookDir += deltaTouch;
                _lookDir = Vector3.ClampMagnitude(_lookDir, 150f);
            }

            if (_lookDir != Vector3.zero)
            {
                float angle = Mathf.Atan2(_lookDir.x, _lookDir.z) * Mathf.Rad2Deg;
                //float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref _turnSmoothTime, speedLerpRotate);
                //transform.rotation = Quaternion.Euler(0f, smoothAngle, 0);
                _playerController.Rotation = Quaternion.Lerp(_playerController.Rotation, Quaternion.Euler(0f, angle, 0), speedLerpRotate);
            }
        }
        
        private void HandlingMovement()
        {
            float angle = Mathf.Atan2(_lookDir.x, _lookDir.z) * Mathf.Rad2Deg;
            Vector3 moveDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 forward = CacheTransform.forward * _horizontalMove;
            if (_characterController.isGrounded == false)
            {
                forward.y += Physics.gravity.y;
            }
            else
            {
                forward.y = 0;
            }
            _characterController.Move(forward * (Time.fixedDeltaTime * moveSpeed));
        }
    }
}

