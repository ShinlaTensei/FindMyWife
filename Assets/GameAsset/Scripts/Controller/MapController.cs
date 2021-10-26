using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;

namespace Game
{
    public class MapController : BaseMono
    {
        [SerializeField] private List<Transform> groupsList = new List<Transform>();
        [SerializeField] private List<Transform> pointsList = new List<Transform>();
        [SerializeField] private List<Transform> pointAnimalList = new List<Transform>();

        public List<Transform> SpawnPointList => pointsList;
        public List<Transform> SpawnGroupList => groupsList;

        public List<Transform> SpawnPointAnimalList => pointAnimalList;
        
        /// <summary>
        /// Get the spawn point position in points list
        /// </summary>
        /// <param name="pointIndex"> The index of point in points list </param>
        /// <returns></returns>
        public Vector3 GetSpawnPosition(int pointIndex)
        {
            if (pointIndex < pointsList.Count)
            {
                return pointsList[pointIndex].position;
            }

            return Vector3.zero;
        }

        public Vector3 GetSpawnRotation(int pointIndex)
        {
            if (pointIndex < pointsList.Count)
            {
                return pointsList[pointIndex].eulerAngles;
            }

            return Vector3.zero;
        }

        public List<Vector3> GetGroupSpawnPosition(int groupIndex)
        {
            List<Vector3> result = new List<Vector3>();
            if (groupIndex < groupsList.Count)
            {
                List<Transform> list = groupsList[groupIndex].GetAllChildren();
                for (int i = 0; i < list.Count; ++i)
                {
                    result.Add(list[i].position);
                }
            }

            return result;
        }
    }
}

