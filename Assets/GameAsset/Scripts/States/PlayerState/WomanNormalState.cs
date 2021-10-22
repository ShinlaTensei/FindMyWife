using System.Collections;
using System.Collections.Generic;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class WomanNormalState : CharacterState
    {
        [SerializeField] private string animatorParamName = "Normal";
        public override void UpdateBehaviour(float dt)
        {
            
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
            
            CharacterStateController.Animator.SetFloat(animatorParamName, 0);
        }
    }
}

