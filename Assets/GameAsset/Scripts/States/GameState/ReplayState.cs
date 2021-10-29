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
            SceneManager.UnloadSceneAsync("UIScene");
            if (!GameManager.GameStatisticParam.isWin)
            {
                int levelIndex = (GameManager.GameStatisticParam.levelIndex - 1) % 2;
                mainSceneAsyncOperation = SceneManager.UnloadSceneAsync($"Level {levelIndex}");
                if (GameManager.GameStatisticParam.isSkipLevel)
                {
                    if (IronSource.Agent.isRewardedVideoAvailable())
                    {
                        IronSource.Agent.showRewardedVideo();
                    }
                    else
                    {
                        GameManager.GameStatisticParam.isSkipLevel = false;
                    }
                }
            }
            else
            {
                int levelIndex = (GameManager.GameStatisticParam.levelIndex) % 2;
                mainSceneAsyncOperation = SceneManager.UnloadSceneAsync($"Level {levelIndex}");
                if (IronSource.Agent.isInterstitialReady())
                {
                    IronSource.Agent.showInterstitial();
                }
            }
            
        }

        public override void CheckExitTransition()
        {
            if (mainSceneAsyncOperation != null && mainSceneAsyncOperation.progress >= .9f && !GameManager.GameStatisticParam.isSkipLevel)
            {
                GameStateController.EnqueueTransition<InitializeState>();
            }
        }

        public override void UpdateBehaviour(float dt)
        {
            
        }
    }
}

