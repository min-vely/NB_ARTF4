using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Scene;
using Scripts.UI;
using Scripts.Utility;
using UnityEngine;

public class Main : MonoBehaviour
{
    #region Singileton

    private static Main _instance;
    private static bool _initialized;
    private static Main Instance
    {
        get
        {
            if (_initialized) return _instance;
            _initialized = true;
            GameObject main = GameObject.Find("@Main");
            if (main != null) return _instance;
            main = new GameObject { name = "@Main" };
            main.AddComponent<Main>();
            DontDestroyOnLoad(main);
            _instance = main.GetComponent<Main>();
            return _instance;
        }
    }
    #endregion

    #region Fields

    private readonly UI_Manager _ui = new();
    private readonly ResourceManager _resource = new();
    private readonly PlayerController _playerControl = new();
    private readonly DataManager _dataManager = new();
    private BaseScene _scene;
    private readonly ObstacleManager _obstacle = new();
    private readonly GameManager _game = new();
    private readonly ItemManager _item = new();

    #endregion

    #region Properties
    public static string NextScene { get; set; }
    public static UI_Manager UI => Instance._ui;
    public static ResourceManager Resource => Instance._resource;
    public static BaseScene Scene => Instance._scene;
    public static PlayerController PlayerControl => Instance._playerControl;
    public static DataManager DataManager => Instance._dataManager;
    public static ObstacleManager Obstacle => Instance._obstacle;
    public static GameManager Game => Instance._game;
    public static ItemManager Item => Instance._item;

    #endregion

    public static void SetCurrentScene(BaseScene scene, Label sceneLabel)
    {
        Instance._scene = scene;
        Instance._scene.CurrentScene = sceneLabel;
    }

    public static void SceneClear()
    {
        // TODO : 씬 전환시 필요한 로직 추가
    }
}
