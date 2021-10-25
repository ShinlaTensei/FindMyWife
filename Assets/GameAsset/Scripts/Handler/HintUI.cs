using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HintUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text text;

        public void SetHint(string _text, Sprite _icon = null)
        {
            if (_icon != null) icon.sprite = _icon;
            text.text = _text;
        }
    }
}

