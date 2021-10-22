using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;

namespace Game
{
    public class LevelController : BaseMono
    {
        [SerializeField] private MapController[] mapsArr;
        [SerializeField] private NpcController npcPrefab;

        private void Start()
        {
            List<Vector3> spawnPointsGroup = mapsArr[0].GetGroupSpawnPosition(0);
            for (int i = 0; i < 20; ++i)
            {
                Transform npc = Instantiate(npcPrefab.transform, mapsArr[0].GetSpawnPosition(i), Quaternion.Euler(mapsArr[0].GetSpawnRotation(i)));
            }
        }
    }
}

