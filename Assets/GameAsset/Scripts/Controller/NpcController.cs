using Base;
using Base.Module;
using Base.Pattern;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class NpcController : BaseMono
    {
        [SerializeField] private Transform graphicsRoot;
        [SerializeField] private CharacterStateController stateController;

        [SerializeField, ReadOnly] private NpcStatisticParam npcStatisticParam = new NpcStatisticParam();

        private NavMeshAgent _agent;

        public NavMeshAgent Agent
        {
            get
            {
                if (_agent == null)
                {
                    _agent = GetComponent<NavMeshAgent>();
                }

                return _agent;
            }
        }

        public NpcStatisticParam NpcStatisticParam => npcStatisticParam;

        public bool IsTarget { get; private set; }
        
        public bool IsCheck { get; private set; }

        public Transform TransformToFollow { get; private set; }
        public void AddGraphic(TargetData child, bool isTarget = false)
        {
            graphicsRoot.DestroyAllChildren();
            Transform children = Instantiate(child.transform, graphicsRoot);
            children.localPosition = Vector3.zero;
            children.localRotation = Quaternion.identity;
            
            IsTarget = isTarget;
            stateController.Animator = children.GetChild(0).GetComponent<Animator>();
        }

        public void KissReaction(Transform transformToFollow = null)
        {
            IsCheck = true;
            
            if (!IsTarget)
            {
                npcStatisticParam.isAngry = true;
            }
            else
            {
                npcStatisticParam.isFollow = true;
                TransformToFollow = transformToFollow;
            }
        }
    }
    
    [System.Serializable]
    public class NpcStatisticParam
    {
        public bool isAngry = false;
        public bool isFollow = false;
    }
}

