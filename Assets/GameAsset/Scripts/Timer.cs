using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Base.MessageSystem;

namespace Game
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Text timeText;

        private float _totalTime;

        private float _timeCountInSeconds;

        private bool _isTimeRunning = false;

        void Update()
        {
            if (_isTimeRunning)
            {
                if (_timeCountInSeconds >= 0)
                {
                    _timeCountInSeconds -= Time.deltaTime;
                    int minutes = Mathf.FloorToInt(_timeCountInSeconds / 60f);
                    int seconds = Mathf.FloorToInt(_timeCountInSeconds % 60f);

                    timeText.text = $"{minutes:00}:{seconds:00}";
                }
                else
                {
                    _isTimeRunning = false;
                    timeText.text = "00 : 00";
                    GameManager.GameStatisticParam.isTimeOut = true;
                }
            }
        }

        public void OnTimerRun(bool isTimeRun)
        {
            _isTimeRunning = isTimeRun;
        }

        public void Init(int totalTimeInMinute)
        {
            _totalTime = totalTimeInMinute;
            _timeCountInSeconds = _totalTime * 60f;
            
            int minutes = Mathf.FloorToInt(_timeCountInSeconds / 60f);
            int seconds = Mathf.FloorToInt(_timeCountInSeconds % 60f);

            timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}

