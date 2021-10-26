using Base;
using Base.Module;
using Base.Pattern;
using NaughtyAttributes;
using UnityEngine;

namespace Game
{
    public class NpcController : BaseMono
    {
        [SerializeField] private Transform graphicsRoot;
        [SerializeField] private CharacterStateController stateController;

        [SerializeField, ReadOnly] private NpcStatisticParam npcStatisticParam = new NpcStatisticParam();

        public NpcStatisticParam NpcStatisticParam => npcStatisticParam;

        public bool IsTarget { get; private set; }


        public void AddGraphic(TargetData child, bool isTarget = false)
        {
            graphicsRoot.DestroyAllChildren();
            Transform children = Instantiate(child.transform, graphicsRoot);
            children.localPosition = Vector3.zero;
            children.localRotation = Quaternion.identity;
            
            IsTarget = isTarget;
            stateController.Animator = children.GetChild(0).GetComponent<Animator>();
        }

        public void KissReaction()
        {
            npcStatisticParam.isAngry = true;
        }
    }
    
    [System.Serializable]
    public class NpcStatisticParam
    {
        public bool isAngry = false;
    }
}

