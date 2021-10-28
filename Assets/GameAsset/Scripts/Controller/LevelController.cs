using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.MessageSystem;
using SensorToolkit;
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
        [SerializeField] private ObjectiveController objectiveController;
        [SerializeField] private List<TargetData> targetCarArr;
        [SerializeField, Space] private ParticleSystem objectiveVfx;

        private void Start()
        {
            objectiveController.ClearAll();
            SetUpTarget();
            Messenger.RegisterListener<TargetType, int, Vector3>(GameMessage.ObjectiveCheck, OnObjectiveCheck);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Messenger.RemoveListener<TargetType, int, Vector3>(GameMessage.ObjectiveCheck, OnObjectiveCheck);
        }

        private void OnObjectiveCheck(TargetType type, int prefabId, Vector3 position)
        {
            bool isComplete = objectiveController.CheckObjective(type, prefabId);
            if (isComplete)
            {
                Messenger.RaiseMessage(GameMessage.ObjectiveComplete, prefabId);
                objectiveVfx.transform.position = position;
                objectiveVfx.Play(true);
            }

            if (objectiveController.IsAllObjectiveCompleted)
            {
                GameManager.GameStatisticParam.isEndPointReach = true;
            }
        }
        
        private void SetUpTarget()
        {
            // ---------------------------------------------- Set up woman target
            TargetData randomTarget = targetDataArr.GetRandom();

            Objective objective1 = new Objective() { isCompleted = false, objectiveId = randomTarget.PrefabId, objectiveType = randomTarget.TargetType };

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

                npc.NpcStatisticParam.idleValue = i % 5f;
            }

            for (int i = 0; i < spawnPointsGroup.Count; ++i)
            {
                for (int j = 0; j < spawnPointsGroup[i].childCount; ++j)
                {
                    NpcController npc = Instantiate(npcPrefab, spawnPointsGroup[i].GetChild(j).position, spawnPointsGroup[i].GetChild(j).rotation, sceneObject);
                    listNpc.Add(npc);
                    npc.NpcStatisticParam.idleValue = j % 5f;
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

            Objective objective2 = new Objective() { isCompleted = false, objectiveId = randomAnimalTarget.PrefabId, objectiveType = randomAnimalTarget.TargetType };
            
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
                animal.NpcStatisticParam.idleValue = i % 4f;
            }
            
            listAnimal.Shuffle();
            AnimalController targetAnimal = listAnimal[0];
            listAnimal.Remove(targetAnimal);
            targetAnimal.AddGraphic(randomAnimalTarget, true);
            int animalLength = listWithoutAnimalTarget.Count;
            for (int i = 0; i < listAnimal.Count; ++i)
            {
                listAnimal[i].AddGraphic(listWithoutAnimalTarget[i % animalLength]);
            }
            
            objectiveController.AddObjective(new[] {objective1, objective2});

            TargetData car = targetCarArr.GetRandom();
            objectiveController.AddObjective(new Objective() {isCompleted = false, objectiveId = car.PrefabId, objectiveType = car.TargetType});
        }
    }
}