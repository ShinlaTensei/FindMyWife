using System.Collections;
using System.Collections.Generic;
using Base.AssetReference;
using Base.Audio;
using Base.Module;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class AnimalAngryState : CharacterState
    {
        private float _totalTime = 0;
        private float _crrClipLength = 0;
        private int _barkingSoundId = 0;

        private AnimalController _animalController;

        protected override void Awake()
        {
            base.Awake();

            _animalController = this.GetComponentInBranch<AnimalController>();
        }

        public override bool CheckEnterTransition(CharacterState fromState)
        {
            return  _totalTime == 0;
        }

        public override void EnterState(float dt, CharacterState fromState)
        {
            CharacterStateController.Animator.GetCurrentClipLength(ref _crrClipLength);
            _barkingSoundId = SoundManager.PlaySound(AssetReference.Instance.GetSoundAsset(SoundRefName.Barking), true);
        }

        public override void UpdateBehaviour(float dt) { }

        public override void PostUpdateBehaviour(float dt)
        {
            _totalTime += dt;
        }

        public override void CheckExitTransition()
        {
            if (_totalTime >= _crrClipLength * 7f)
            {
                SoundManager.GetAudio(_barkingSoundId).Stop();
                _animalController.NpcStatisticParam.isAngry = false;
                CharacterStateController.EnqueueTransition<AnimalNormalState>();
            }
        }

        public override void ExitStateBehaviour(float dt, CharacterState toState)
        {
            _totalTime = 0;
            _crrClipLength = 0;
        }
    }
}

