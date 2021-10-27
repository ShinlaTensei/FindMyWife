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
        [SerializeField] protected Transform graphicsRoot;
        [SerializeField] protected CharacterStateController stateController;

        [SerializeField, ReadOnly] protected NpcStatisticParam npcStatisticParam = new NpcStatisticParam();

        protected NavMeshAgent _agent;
        
        public TargetData TargetData { get; protected set; }

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

        public bool IsTarget { get; protected set; }
        
        public bool IsCheck { get; protected set; }

        public Transform TransformToFollow { get; protected set; }
        public virtual void AddGraphic(TargetData child, bool isTarget = false)
        {
            graphicsRoot.DestroyAllChildren();
            Transform children = Instantiate(child.transform, graphicsRoot);
            children.localPosition = Vector3.zero;
            children.localRotation = Quaternion.identity;
            TargetData = child;
            IsTarget = isTarget;
            stateController.Animator = children.GetChild(0).GetComponent<Animator>();
        }

        public virtual void InteractReaction(Transform transformToFollow = null)
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
        public float idleValue = 0;
    }
}

