using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class Objective
    {
        public TargetType objectiveType;
        public int objectiveId;
        public bool isCompleted;
    }
}

