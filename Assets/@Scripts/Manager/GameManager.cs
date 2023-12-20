using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager
{
    public bool canLook { get; set; } = true;

    public readonly List<Vector3> checkPoints = new List<Vector3> 
    {
        new (0, 2.7f, 6.6f),    // CheckPoint 1 
        new (0, 2.7f, 58f)      // CheckPoint 2 
    };

    public GameObject Obstacle
    {
        get
        {
            GameObject map = GameObject.Find("@Obstacle");
            if (map == null) map = new GameObject { name = "@Obstacle" };
            return map;
        }
    }

    public GameObject CheckPoint
    {
        get
        {
            GameObject checkPoint = GameObject.Find("@CheckPoint");
            if (checkPoint == null) checkPoint = new GameObject { name = "@CheckPoint" };
            return checkPoint;
        }
    }

    public GameObject LimitOutside
    {
        get
        {
            GameObject limitOutSide = GameObject.Find("@LimitOutSide");
            if (limitOutSide == null) limitOutSide = new GameObject { name = "@LimitOutSide" };
            return limitOutSide;
        }
    }

    public void SpawnPlayer()
    {
        GameObject playerObject = Main.Resource.InstantiatePrefab("Player.prefab");
        playerObject.name = "@Player";
        playerObject.transform.position = new Vector3(0, 0, -10);
    }
}
