using System;
using System.Collections;
using System.Collections.Generic;
using Base.GameEventSystem;
using Base.MessageSystem;
using Base.Module;
using UnityEngine;

namespace Game
{
    public class UISceneHandler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> pagesList = new List<GameObject>();
        [SerializeField] private List<HintUI> hintUis = new List<HintUI>();

        private void Start()
        {
            for (int i = 0; i < pagesList.Count; i++)
            {
                pagesList[i].SetActive(false);
            }
        
            pagesList[0].SetActive(true);
            
            RegisterListener();
        }

        private void OnDestroy()
        {
            RemoveListener();
        }

        public void OnStartStateNotify(GameEventData data)
        {
        
        }

        public void OnPlayingStateNotify(GameEventData data)
        {
            pagesList[0].SetActive(false);
            pagesList[1].SetActive(true);
        }

        private void RegisterListener()
        {
            Messenger.RegisterListener<TargetData>(GameMessage.RegisterTarget, OnRegisterTarget);
        }

        private void RemoveListener()
        {
            Messenger.RemoveListener<TargetData>(GameMessage.RegisterTarget, OnRegisterTarget);
        }

        private void OnRegisterTarget(TargetData target)
        {
            for (int i = 0; i < hintUis.Count; ++i)
            {
                hintUis[i].SetHint(target.Hint[i].hintInfo);
            }
        }
    }
}

