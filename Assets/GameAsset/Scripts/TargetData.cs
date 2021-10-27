using System.Collections;
using Base;
using UnityEngine;
using System.Collections.Generic;
using Base.Module;
using Base.Utilities;
using NaughtyAttributes;

namespace Game
{
    public class TargetData : BaseMono
    {
        [SerializeField] private int prefabId = 0;
        [SerializeField] private TargetType targetType;
        [SerializeField, CustomClassDrawer, ShowIf("targetType", TargetType.Woman)] private List<Hint> hint;

        [SerializeField, CustomClassDrawer, ShowIf("targetType", TargetType.Animal)]
        private List<Hint> hintAnimal;

        public int PrefabId => prefabId;

        public List<Hint> Hint
        {
            get
            {
                if (targetType == TargetType.Woman) return hint;

                return hintAnimal;
            }
        }

        //public List<HintAnimal> HintAnimals => hintAnimal;
        public TargetType TargetType => targetType;
    }
    
    [System.Serializable]
    public class Hint
    {
        public HintType hintType;
        public string hintInfo;
    }

    [System.Serializable]
    public class HintAnimal
    {
        public Hint hintTypeAnimal;
        public string hintInfo;
    }
    
    public enum HintType {Skin, Hair, Outfit, Bread, Color}
    
    //public enum HintTypeAnimal {Bread, CoatColor}
    
    public enum TargetType {Woman, Animal}
}

