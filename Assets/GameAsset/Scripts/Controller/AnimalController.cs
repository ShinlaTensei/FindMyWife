using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using Base.Pattern;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class AnimalController : NpcController
    {
        public override void InteractReaction(Transform transformToFollow = null)
        {
            IsCheck = true;
            if (IsTarget)
            {
                TransformToFollow = transformToFollow;
                npcStatisticParam.isFollow = true;
            }
            else
            {
                npcStatisticParam.isAngry = true;
            }
        }
    }
}

