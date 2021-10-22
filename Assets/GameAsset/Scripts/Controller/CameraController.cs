using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;

namespace Game
{
    public class CameraController : CameraFollow
    {
        private void Start()
        {
            InitOffset();
        }

        private void LateUpdate()
        {
            FollowTarget();
        }
    }
}

