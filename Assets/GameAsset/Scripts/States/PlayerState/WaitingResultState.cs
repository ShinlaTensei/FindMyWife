using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Base.AssetReference;
using Base.Audio;
using Base.Module;
using Base.Pattern;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Base.MessageSystem;

namespace Game
{
    public class WaitingResultState : CharacterState
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private string reactParam = "React";
        [SerializeField] private GameObject hitBox;

        private CancellationTokenSource _cancellation;
        
        private PlayerController _playerController;

        private bool _isHit = false;
        private float _totalTime = 0;
        private float _crrClipTime = 0;

        private TargetType _crrTargetType = TargetType.Woman;
        public override void EnterState(float dt, CharacterState fromState)
        {
            _cancellation = new CancellationTokenSource();
            _playerController = characterController.GetComponent<PlayerController>();
            if (_playerController.NpcDetected)
            {
                _crrTargetType = _playerController.NpcDetected.TargetData.TargetType;
                if (_crrTargetType == TargetType.Woman)
                {
                    WaitForGetSlap().Forget();
                }
                else
                {
                    _crrClipTime = 2f;
                    #if !UNITY_EDITOR
                    Taptic.AndroidTaptic.Vibrate();
                    #endif
                }
            }
        }

        public override void ExitStateBehaviour(float dt, CharacterState toState)
        {
            _crrClipTime = 0;
            _totalTime = 0;
            if (_cancellation != null)
            {
                _cancellation.Cancel();
                _cancellation.Dispose();
                _cancellation = null;
            }
        }

        public override void CheckExitTransition()
        {
            if (_totalTime > _crrClipTime)
            {
                CharacterStateController.EnqueueTransition<NormalMovementState>();
            }
        }

        public override void UpdateBehaviour(float dt)
        {
            if (_crrClipTime > 0)
            {
                _totalTime += dt;
            }
        }

        public override void PostUpdateBehaviour(float dt)
        {
            if (CharacterStateController.Animator == null) return;
            
            if (CharacterStateController.Animator.runtimeAnimatorController == null) return;
            
            if (!CharacterStateController.Animator.gameObject.activeSelf)
            {
                return;
            }

            if (_isHit)
            {
                _isHit = false;
                CharacterStateController.Animator.SetTrigger(reactParam);
                CharacterStateController.Animator.GetCurrentClipLength(ref _crrClipTime);
                _totalTime = 0;
            }
        }

        private async UniTaskVoid WaitForGetSlap()
        {
            var trigger = hitBox.GetAsyncCollisionEnterTrigger();
            var result = await trigger.OnCollisionEnterAsync(_cancellation.Token);
            if (result.collider.CompareTag("NpcHand"))
            {
                _isHit = true;
                SoundManager.PlaySound(AssetReference.Instance.GetSoundAsset(SoundRefName.Slaped));
                #if !UNITY_EDITOR
                Taptic.AndroidTaptic.Vibrate();
                #endif
            }
        }
    }
}

