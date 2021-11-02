using System;
using Base;
using Base.AssetReference;
using Base.Audio;
using Base.GameEventSystem;
using Base.MessageSystem;
using Base.Module;
using Base.Pattern;
using Facebook.Unity;
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

        private void Awake()
        {
            if (!FB.IsInitialized)
            {
                FB.Init();
            }
            else
            {
                FB.ActivateApp();
            }
            
            SaveLoad.LoadFromJson(out ConfigData config, "config.json");
            if (config == null)
            {
                config = new ConfigData();
            }
            gameStatisticParam.configData = config;
            
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    var app = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    Debug.LogError(String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;

            gameStateController.OnStateChanged += OnStateChanged;
            
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init("10003aae1", IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL);
            IronSource.Agent.loadInterstitial();
            IronSource.Agent.shouldTrackNetworkState(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            gameStateController.OnStateChanged -= OnStateChanged;
            Messenger.CleanUp();

            gameStatisticParam.configData.isFirstLaunch = false;
            SaveLoad.SaveToJson(gameStatisticParam.configData, "config.json");
        }

        private void OnApplicationQuit()
        {
            
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            IronSource.Agent.onApplicationPause(pauseStatus);
        }

        private void OnEnable()
        {
            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        }

        private void OnDisable()
        {
            IronSourceEvents.onInterstitialAdReadyEvent -= InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent -= InterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent -= InterstitialAdShowSucceededEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent -= InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent -= InterstitialAdClosedEvent;
            
            IronSourceEvents.onRewardedVideoAdClosedEvent -= RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent -= RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardedVideoAdRewardedEvent;
        }

        private void OnStateChanged(GameState from, GameState to)
        {
            if (to is InitializeState)
            {
                gameStatisticParam.Reset();
            }
            else if (to is StartState)
            {
                SoundManager.PlayMusic(AssetReference.Instance.GetSoundAsset(SoundRefName.Background), 1f, true, true);
            }
            else if (to is EndState)
            {
                bool isWin = objectiveController.IsAllObjectiveCompleted;
                gameStatisticParam.isWin = isWin;
                endGameNotify.InvokeEvent(new GameEventData(isWin));
                gameStatisticParam.levelIndex += isWin ? 1 : 0;
                SoundManager.StopAllMusic();
                if (isWin) SoundManager.PlaySound(AssetReference.Instance.GetSoundAsset(SoundRefName.Victory));
                else SoundManager.PlaySound(AssetReference.Instance.GetSoundAsset(SoundRefName.Defeat));
            }
            else if (to is ReplayState)
            {
                
            }
        }
        
        private void InterstitialAdReadyEvent()
        {

        }

        private void InterstitialAdShowSucceededEvent()
        {

        }

        private void InterstitialAdLoadFailedEvent(IronSourceError error)
        {

        }

        private void InterstitialAdShowFailedEvent(IronSourceError error)
        {

        }

        private void InterstitialAdClosedEvent()
        {
            
        }

        private void RewardedVideoAdClosedEvent()
        {
            Debug.Log("Reward Ad Closed Event");
            gameStatisticParam.isSkipLevel = false;
        }

        private void RewardedVideoAdEndedEvent()
        {
            
        }

        private void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            Debug.Log("Reward Ad Reward Event");
            gameStatisticParam.levelIndex += 1;
            gameStatisticParam.isSkipLevel = false;
        }
    }
    
    [System.Serializable]
    public class GameStatisticParam
    {
        public bool isEndPointReach = false;
        public bool isReplay = false;
        public bool isWin = false;
        public bool isSkipLevel = false;
        public bool isTimeOut = false;
        public int levelIndex = 1;
        public ConfigData configData;

        public void Reset()
        {
            isReplay = false;
            isTimeOut = false;
            isEndPointReach = false;
            isSkipLevel = false;
            isWin = false;
        }
    }
    
    public enum GameMessage {RegisterTarget, ObjectiveComplete, ObjectiveCheck}
}

