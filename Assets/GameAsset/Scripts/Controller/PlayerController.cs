using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Base;
using Base.GameEventSystem;
using Base.MessageSystem;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using SensorToolkit;
using UnityEngine;

namespace Game
{
    public class PlayerController : BaseMono
    {
        [SerializeField] private RangeSensor rangeSensor;
        [SerializeField] private Transform kissPoint;
        [SerializeField] private ObjectiveController objectiveController;
        [SerializeField] private Transform arrowTransform;
        [SerializeField, ReadOnly] private McStatisticParam mcStatisticParam;

        public McStatisticParam McStatisticParam => mcStatisticParam;
        public ObjectiveController ObjectiveController => objectiveController;
        public NpcController NpcDetected { get; private set; }
        
        private CancellationTokenSource _checkTargetToken = new CancellationTokenSource();

        private GameObject _sensorTarget = null;

        private void Update()
        {
            if (_sensorTarget != null)
            {
                Vector3 pos = _sensorTarget.transform.position;
                //arrowTransform.position = new Vector3(pos.x, pos.y + 4, pos.z);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_checkTargetToken != null)
            {
                _checkTargetToken.Cancel();
                _checkTargetToken.Dispose();
                _checkTargetToken = null;
            }
            
        }

        public void PulseSensor()
        {
            _checkTargetToken = new CancellationTokenSource();
            //rangeSensor.Pulse();
            if (_sensorTarget != null) CheckTarget(_sensorTarget).Forget();
        }
        public void OnDetectNpc(GameObject obj, Sensor sensor)
        {
            //CheckTarget(obj).Forget();
            NpcController npc = obj.GetComponent<NpcController>();
            if (npc && !npc.IsGet)
            {
                //arrowTransform.gameObject.SetActive(true);
                _sensorTarget = sensor.DetectedObjectsOrderedByDistance[0];
            }
            else if (!npc)
            {
                _sensorTarget = sensor.DetectedObjectsOrderedByDistance[0];
            }
        }

        public void OnLoseDetection(GameObject obj, Sensor sensor)
        {
            if (obj == _sensorTarget)
            {
                //arrowTransform.gameObject.SetActive(false);
                _sensorTarget = null;
            }
        }

        public void OnEndStateNotify(GameEventData data)
        {
            bool isAllObjectiveComplete = (bool)data.Data;
            mcStatisticParam.isLevelEnd = true;
            mcStatisticParam.isPlayerWin = isAllObjectiveComplete;
        }

        private async UniTaskVoid CheckTarget(GameObject obj)
        {
            if (!obj.CompareTag("Car"))
            {
                List<NpcController> listNpcDetected = new List<NpcController>();
                NpcController npc = obj.GetComponent<NpcController>();
                if (npc != null && !npc.IsCheck) listNpcDetected.Add(npc);
                await UniTask.WaitForEndOfFrame(_checkTargetToken.Token);
            
                _checkTargetToken.Cancel();
                _checkTargetToken.Dispose();
                _checkTargetToken = null;

                float maxDistance = 1000000f;
                NpcController finalTarget = null;
                for (int i = 0; i < listNpcDetected.Count; ++i)
                {
                    float minDistance = Vector3.Distance(Position, listNpcDetected[i].Position);
                    if (minDistance < maxDistance)
                    {
                        maxDistance = minDistance;
                        finalTarget = listNpcDetected[i];
                    }
                }

                if (finalTarget != null)
                {
                    mcStatisticParam.isNpcDetected = true;
                    mcStatisticParam.isCorrectTarget = finalTarget.IsTarget;
                    finalTarget.Position = kissPoint.position;
                    finalTarget.Rotation = kissPoint.rotation;
                    NpcDetected = finalTarget;
                    if (NpcDetected is AnimalController) mcStatisticParam.actionValue = 1f;
                    else mcStatisticParam.actionValue = 0;
                }
            }
            else
            {
                mcStatisticParam.isCarDetected = true;
                mcStatisticParam.carDetected = obj.GetComponent<TargetData>();
            }
        }
    }

    [System.Serializable]
    public class McStatisticParam
    {
        public bool isCarDetected = false;
        public bool isNpcDetected = false;
        public bool isCorrectTarget = false;
        public float actionValue = 0;
        public bool isLevelEnd = false;
        public bool isPlayerWin = false;
        public TargetData carDetected = null;
    }
}

