using Base;
using Base.GameEventSystem;
using Base.Pattern;
using NaughtyAttributes;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField, ReadOnly] private GameStatisticParam gameStatisticParam = new GameStatisticParam();
        [SerializeField] private GameStateController gameStateController;
        [SerializeField] private ObjectiveController objectiveController;
        [SerializeField, Space] private GameEvent endGameNotify;
        public static GameStatisticParam GameStatisticParam => Instance.gameStatisticParam;
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;

            gameStateController.OnStateChanged += OnStateChanged;
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
            else if (to is EndState)
            {
                bool isWin = objectiveController.IsAllObjectiveCompleted;
                endGameNotify.InvokeEvent(new GameEventData(isWin));
                gameStatisticParam.levelIndex += isWin ? 1 : 0;
            }
        }
    }
    
    [System.Serializable]
    public class GameStatisticParam
    {
        public bool isEndPointReach = false;
        public bool isReplay = false;
        public bool isTimeOut = false;
        public int levelIndex = 1;

        public void Reset()
        {
            isReplay = false;
            isTimeOut = false;
            isEndPointReach = false;
        }
    }
    
    public enum GameMessage {RegisterTarget, ObjectiveComplete, ObjectiveCheck}
}

