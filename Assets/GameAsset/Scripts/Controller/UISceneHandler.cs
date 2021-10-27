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
        [SerializeField] private List<HintUI> hintUiAnimals = new List<HintUI>();
        
        [SerializeField, Space] private GameObject womanHintPanel;
        [SerializeField] private GameObject animalHintPanel;

        private List<TargetData> _targetDataList = new List<TargetData>();

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
            pagesList[2].SetActive(false);
            
            InitHint();
        }

        public void OnEndStateNotify(GameEventData data)
        {
            pagesList[0].SetActive(false);
            pagesList[1].SetActive(false);
            pagesList[2].SetActive(true);
        }

        public void OnReplayClick()
        {
            GameManager.GameStatisticParam.isReplay = true;
        }

        private void InitHint()
        {
            if (_targetDataList.Count > 0)
            {
                TargetData data = _targetDataList[0];
                if (data.TargetType == TargetType.Woman)
                {
                    womanHintPanel.SetActive(true);
                    animalHintPanel.SetActive(false);
                    for (int i = 0; i < hintUis.Count; ++i)
                    {
                        hintUis[i].SetHint(data.Hint[i].hintInfo);
                    }
                }
                else
                {
                    womanHintPanel.SetActive(false);
                    animalHintPanel.SetActive(true);
                    for (int i = 0; i < hintUis.Count - 1; ++i)
                    {
                        hintUiAnimals[i].SetHint(data.Hint[i].hintInfo);
                    }
                }
            }
            
        }

        private void RegisterListener()
        {
            Messenger.RegisterListener<TargetData>(GameMessage.RegisterTarget, OnRegisterTarget);
            Messenger.RegisterListener<int>(GameMessage.ObjectiveComplete, OnObjectiveComplete);
        }

        private void RemoveListener()
        {
            Messenger.RemoveListener<TargetData>(GameMessage.RegisterTarget, OnRegisterTarget);
            Messenger.RemoveListener<int>(GameMessage.ObjectiveComplete, OnObjectiveComplete);
        }

        private void OnRegisterTarget(TargetData target)
        {
            _targetDataList.Add(target);
            //womanHintPanel.SetActive(target.TargetType == TargetType.Woman);
        }

        private void OnObjectiveComplete(int prefabId)
        {
            TargetData result = _targetDataList.Find(item => item.PrefabId == prefabId);
            if (result) _targetDataList.Remove(result);

            InitHint();
        }
    }
}

