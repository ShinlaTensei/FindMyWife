using System.Collections;
using Base;
using UnityEngine;
using System.Collections.Generic;
using Base.Module;

namespace Game
{
    public class TargetData : BaseMono
    {
        [SerializeField] private int prefabId = 0;
        [SerializeField, CustomClassDrawer] private List<Hint> hint;

        public int PrefabId => prefabId;

        public List<Hint>  Hint => hint;
    }
    
    [System.Serializable]
    public class Hint
    {
        public HintType hintType;
        public string hintInfo;
    }
    
    public enum HintType {Skin, Hair, Outfit}
}

