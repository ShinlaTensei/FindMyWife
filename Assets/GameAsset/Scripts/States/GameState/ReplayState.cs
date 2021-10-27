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

            mainSceneAsyncOperation = SceneManager.UnloadSceneAsync("MainGameScene");
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

