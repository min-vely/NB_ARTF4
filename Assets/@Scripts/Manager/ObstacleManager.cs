using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Scene;
using UnityEngine;

public class ObstacleManager
{
    public event Action OnInitializedObstacle;

    public void Initialized()
    {
        KillZone.OnDeath += InitializedObstacle;
    }

    private void InitializedObstacle()
    {
        OnInitializedObstacle?.Invoke();
    }
}
