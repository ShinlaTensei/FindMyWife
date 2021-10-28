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
        
        [SerializeField, ReadOnly] private McStatisticParam mcStatisticParam;

        public McStatisticParam McStatisticParam => mcStatisticParam;
        
        public NpcController NpcDetected { get; private set; }
        
        private CancellationTokenSource _checkTargetToken = new CancellationTokenSource();

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
            rangeSensor.Pulse();
        }
        public void OnDetectNpc(GameObject obj, Sensor sensor)
        {
            CheckTarget(obj).Forget();
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

