using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using Base.Pattern;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class AnimalController : BaseMono
    {
        [SerializeField] private Transform graphicsRoot;
        [SerializeField] private CharacterStateController stateController;
        private NavMeshAgent _agent = null;
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
    }
}

