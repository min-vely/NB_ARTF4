using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Sector;

public enum Sector { Sector1, Sector2, Sector3, Sector4, Sector5, Sector6, Sector7 }

public class ObstacleManager
{
    public event Action OnInitializedObstacle;

    public void Initialized()
    {
        KillZone.OnDeath += InitializedObstacle;
        SetLimit();
        SetCheckPoint();
        SetMap(Sector1);
        SetMap(Sector2);
        SetMap(Sector3);
        SetMap(Sector4);
        SetMap(Sector5);
        SetMap(Sector6);
        SetMap(Sector7);
    }

    private void SetLimit()
    {
        Transform limitTransform = Main.Game.LimitOutside.transform;
        GameObject limit = Main.Resource.InstantiatePrefab("Limits.prefab", limitTransform);
        limit.name = "@LimitZone";
    }


    private void SetCheckPoint()
    {
        List<Vector3> checkPoints = Main.Game.checkPoints;
        Transform mapTransform = Main.Game.CheckPoint.transform;
        int checkPointNumber = 0;
        foreach (var obj in checkPoints.Select(checkPoint => Main.Resource.InstantiatePrefab("CheckPoint.prefab", mapTransform)))
        {
            checkPointNumber++;
            obj.name = $"CheckPoint {checkPointNumber}";
        }
    }

    private void SetMap(Sector sector)
    {
        string sectorKey = sector.ToString();
        Transform obstacleTransform = Main.Game.Obstacle.transform;

        GameObject sectorObject = Main.Resource.InstantiatePrefab($"{sectorKey}.prefab", obstacleTransform);
        sectorObject.name = $"{sectorKey}";
        sectorObject.transform.position = sectorObject.transform.position;
    }

    private void InitializedObstacle()
    {
        OnInitializedObstacle?.Invoke();
    }
}
