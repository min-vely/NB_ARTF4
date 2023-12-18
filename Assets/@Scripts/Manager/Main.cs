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
    private readonly SetBinder _setBinder = new();
    private readonly ResourceManager _resource = new();
    private BaseScene _scene;

    #endregion

    #region Properties

    public static UI_Manager UI => Instance._ui;
    public static SetBinder SetBinder => Instance._setBinder;
    public static ResourceManager Resource => Instance._resource;
    public static BaseScene Scene => Instance._scene;

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
