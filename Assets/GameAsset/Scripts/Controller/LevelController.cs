using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.MessageSystem;
using UnityEngine;

namespace Game
{
    public class LevelController : BaseMono
    {
        [SerializeField] private MapController[] mapsArr;
        [SerializeField] private NpcController npcPrefab;
        [SerializeField] private List<TargetData> targetDataArr;
        [SerializeField] private Transform sceneObject;

        private void Start()
        {
            SetUpTarget();
        }


        private void SetUpTarget()
        {
            TargetData randomTarget = targetDataArr.GetRandom();
            List<TargetData> listWithoutTarget = targetDataArr.ToList();
            listWithoutTarget.Remove(randomTarget);
            
            Messenger.RaiseMessage(GameMessage.RegisterTarget, randomTarget);

            List<Transform> spawnPointsGroup = mapsArr[0].SpawnGroupList.ToList();
            List<Transform> spawnPointList = mapsArr[0].SpawnPointList.ToList();
            spawnPointList.Shuffle();
            
            List<NpcController> listNpc = new List<NpcController>();
            for (int i = 0; i < spawnPointList.Count; ++i)
            {
                NpcController npc = Instantiate(npcPrefab, spawnPointList[i].position, spawnPointList[i].rotation, sceneObject);
                listNpc.Add(npc);
            }

            for (int i = 0; i < spawnPointsGroup.Count; ++i)
            {
                for (int j = 0; j < spawnPointsGroup[i].childCount; ++j)
                {
                    NpcController npc = Instantiate(npcPrefab, spawnPointsGroup[i].GetChild(j).position, spawnPointsGroup[i].GetChild(j).rotation, sceneObject);
                    listNpc.Add(npc);
                }
            }
            
            listNpc.Shuffle();
            NpcController targetNpc = listNpc[0];
            listNpc.Remove(targetNpc);
            targetNpc.AddGraphic(randomTarget, true);
            
            int length = listWithoutTarget.Count;
            for (int i = 0; i < listNpc.Count; ++i)
            {
                listNpc[i].AddGraphic(listWithoutTarget[i % length]);
            }
        }
    }
}