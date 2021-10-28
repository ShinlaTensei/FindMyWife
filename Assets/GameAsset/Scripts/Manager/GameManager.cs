using System.Collections;
using System.Collections.Generic;
using Base;
using Base.Pattern;
using NaughtyAttributes;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField, ReadOnly] private GameStatisticParam gameStatisticParam = new GameStatisticParam();
        [SerializeField] private GameStateController gameStateController;
        public static GameStatisticParam GameStatisticParam => Instance.gameStatisticParam;
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;

            gameStateController.OnStateChanged += OnStateChanged;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            gameStateController.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameState from, GameState to)
        {
            if (to is InitializeState)
            {
                gameStatisticParam.Reset();
            }
        }
    }
    
    [System.Serializable]
    public class GameStatisticParam
    {
        public bool isEndPointReach = false;
        public bool isReplay = false;
        public bool isTimeOut = false;

        public void Reset()
        {
            isReplay = false;
            isTimeOut = false;
            isEndPointReach = false;
        }
    }
    
    public enum GameMessage {RegisterTarget, ObjectiveComplete, ObjectiveCheck}
}

