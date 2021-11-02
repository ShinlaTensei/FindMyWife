using Base;
using Base.Module;
using Base.Pattern;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Base.MessageSystem;

namespace Game
{
    public class NpcController : BaseMono
    {
        [SerializeField] protected Transform graphicsRoot;
        [SerializeField] protected CharacterStateController stateController;
        [SerializeField] protected ObjectiveController objectiveController;

        [SerializeField, ReadOnly] protected NpcStatisticParam npcStatisticParam = new NpcStatisticParam();

        protected NavMeshAgent _agent;
        protected bool _isTarget = false;
        
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

        public bool IsTarget
        {
            get
            {
                if (objectiveController.CurrentObjective.objectiveId == TargetData.PrefabId)
                {
                    return _isTarget;
                }

                return false;
            }
            protected set => _isTarget = value;
        }

        public bool IsCheck { get; protected set; }
        
        public bool IsGet { get; protected set; }

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
            if (!IsTarget)
            {
                //IsCheck = true;
                npcStatisticParam.isAngry = true;
            }
            else
            {
                IsCheck = true;
                npcStatisticParam.isFollow = true;
                TransformToFollow = transformToFollow;
                IsGet = true;
                Messenger.RaiseMessage(GameMessage.ObjectiveCheck, TargetData.TargetType, TargetData.PrefabId, Position);
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

