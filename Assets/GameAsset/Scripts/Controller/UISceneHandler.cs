using System;
using System.Collections;
using System.Collections.Generic;
using Base.GameEventSystem;
using Base.Module;
using UnityEngine;

public class UISceneHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> pagesList = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < pagesList.Count; i++)
        {
            pagesList[i].SetActive(false);
        }
        
        pagesList[0].SetActive(true);
    }

    public void OnStartStateNotify(GameEventData data)
    {
        
    }

    public void OnPlayingStateNotify(GameEventData data)
    {
        pagesList[0].SetActive(false);
        pagesList[1].SetActive(true);
    }
}
