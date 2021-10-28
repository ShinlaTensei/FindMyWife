using System.Collections;
using System.Collections.Generic;
using Base.Module;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Objective controller")]
    public class ObjectiveController : ScriptableObject
    {
        [SerializeField, CustomClassDrawer] private List<Objective> objectiveList = new List<Objective>();

        public List<Objective> ObjectiveList => objectiveList;

        public Objective CurrentObjective
        {
            get
            {
                return objectiveList.Find(item => !item.isCompleted);
            }
        }

        public bool IsAllObjectiveCompleted
        {
            get
            {
                return objectiveList.TrueForAll(item => item.isCompleted);
            }
        }

        public bool CheckObjective(TargetType type, int prefabId)
        {
            Objective obj = objectiveList.Find(item => item.objectiveType == type && item.objectiveId == prefabId);
            if (obj != null && CurrentObjective.Equals(obj))
            {
                obj.isCompleted = true;
                return true;
            }

            return false;
        }

        public void AddObjective(Objective obj)
        {
            objectiveList.Add(obj);
        }

        public void AddObjective(Objective[] objArr)
        {
            objectiveList.AddRange(objArr);
        }

        public Objective Next()
        {
            return objectiveList.Find(item => !item.isCompleted);
        }

        public void ClearAll()
        {
            objectiveList.Clear();
        }
    }
}

