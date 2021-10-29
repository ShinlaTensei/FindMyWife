using System.Collections;
using System.Collections.Generic;
using Base.Pattern;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class ReplayState : GameState
    {
        private AsyncOperation mainSceneAsyncOperation;
        public override void EnterStateBehaviour(float dt, GameState fromState)
        {
            base.EnterStateBehaviour(dt, fromState);
            int levelIndex = (GameManager.GameStatisticParam.levelIndex) % 2;
            mainSceneAsyncOperation = SceneManager.UnloadSceneAsync($"Level {levelIndex}");
            SceneManager.UnloadSceneAsync("UIScene");
        }

        public override void CheckExitTransition()
        {
            if (mainSceneAsyncOperation != null && mainSceneAsyncOperation.progress >= .9f)
            {
                GameStateController.EnqueueTransition<InitializeState>();
            }
        }

        public override void UpdateBehaviour(float dt)
        {
            
        }
    }
}

