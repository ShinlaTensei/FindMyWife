using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Game
{
    
}
public class UiTouchHandler : MonoBehaviour, IPointerClickHandler
{
    public event Action onPointerClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        onPointerClick?.Invoke();
    }
}
