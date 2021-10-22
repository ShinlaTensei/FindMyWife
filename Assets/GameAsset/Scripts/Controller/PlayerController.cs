using System.Collections;
using System.Collections.Generic;
using Base;
using NaughtyAttributes;
using SensorToolkit;
using UnityEngine;

namespace Game
{
    public class PlayerController : BaseMono
    {
        [SerializeField] private RangeSensor rangeSensor;

        [SerializeField, ReadOnly] private McStatisticParam mcStatisticParam;

        public McStatisticParam McStatisticParam => mcStatisticParam;

        public void PulseSensor()
        {
            rangeSensor.Pulse();
        }
        public void OnDetectNpc(GameObject obj, Sensor sensor)
        {
            mcStatisticParam.isNpcDetected = true;
        }
    }

    [System.Serializable]
    public class McStatisticParam
    {
        public bool isNpcDetected = false;
        public bool beSlap = false;
    }
}

