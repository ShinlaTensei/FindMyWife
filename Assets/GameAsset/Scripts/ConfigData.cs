using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ConfigData
    {
        public bool isFirstLaunch;
        public bool isSound;
        public bool isVibrate;

        public ConfigData()
        {
            isFirstLaunch = true;
            isSound = false;
            isVibrate = false;
        }
    }
}

