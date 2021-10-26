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
        [SerializeField] private AnimalController animalPrefab;
        [SerializeField] private List<TargetData> targetDataArr;
        [SerializeField] private List<TargetData> targetAnimalArr;
        [SerializeField] private Transform sceneObject;

        private void Start()
        {
            SetUpTarget();
        }


        private void SetUpTarget()
        {
            // ---------------------------------------------- Set up woman target
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
            
            // -------------------------------------------------- Set up animal target
            TargetData randomAnimalTarget = targetAnimalArr.GetRandom();
            List<TargetData> listWithoutAnimalTarget = targetAnimalArr.ToList();
            listWithoutAnimalTarget.Remove(randomAnimalTarget);
            
            Messenger.RaiseMessage(GameMessage.RegisterTarget, randomAnimalTarget);
            
            List<Transform> spawnPointAnimalList = mapsArr[0].SpawnPointAnimalList.ToList();
            spawnPointAnimalList.Shuffle();
            List<AnimalController> listAnimal = new List<AnimalController>();
            for (int i = 0; i < spawnPointAnimalList.Count; ++i)
            {
                AnimalController animal = Instantiate(animalPrefab, spawnPointAnimalList[i].position, spawnPointAnimalList[i].rotation, sceneObject);
                listAnimal.Add(animal);
            }
            
            listAnimal.Shuffle();
            AnimalController targetAnimal = listAnimal[0];
            listAnimal.Remove(targetAnimal);
            targetAnimal.AddGraphic(randomAnimalTarget, true);
            int animalLength = listWithoutAnimalTarget.Count;
            for (int i = 0; i < animalLength; ++i)
            {
                listAnimal[i].AddGraphic(listWithoutAnimalTarget[i % animalLength]);
            }
        }
    }
}