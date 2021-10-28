using UnityEngine;
using Base.MessageSystem;

namespace Game
{
    public class AnimalController : NpcController
    {
        public override void InteractReaction(Transform transformToFollow = null)
        {
            if (IsTarget)
            {
                TransformToFollow = transformToFollow;
                npcStatisticParam.isFollow = true;
                Messenger.RaiseMessage(GameMessage.ObjectiveCheck, TargetData.TargetType, TargetData.PrefabId);
            }
            else
            {
                //IsCheck = true;
                npcStatisticParam.isAngry = true; 
            }
        }
    }
}

