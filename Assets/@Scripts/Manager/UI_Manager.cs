using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Event.UI;
using Scripts.Scene;
using Scripts.UI;
using Scripts.UI.Popup;
using Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region Fields

    private int _sortByOrderLayer = 1;
    private Stack<Popup> _popupOrder = new();
    private event Action _onCloseDeathPanel;
    private event Action _onClosePausePanel;
    private bool isPausePanelOpen = false; // PausePanel이 중첩 띄워지는 현상 방지용

    #endregion

    #region Fields

    public bool IsPausePanelOpen
    {
        get { return isPausePanelOpen; }
        private set { isPausePanelOpen = value; }
    }
    public event Action OnCloseDeathPanel
    {
        add { _onCloseDeathPanel += value; }
        remove { _onCloseDeathPanel -= value; }
    }
    public event Action OnClosePausePanel
    {
        add { _onClosePausePanel += value; }
        remove { _onClosePausePanel -= value; }
    }
    #endregion

    private GameObject UIBase
    {
        get
        {
            GameObject uiBase = GameObject.Find("@UI_Base");
            if (uiBase == null)
            {
                uiBase = new GameObject { name = "@UI_Base" };
            }
            return uiBase;
        }
    }


    public T OpenPopup<T>(string objectName = null) where T : Popup
    {
        string objName = NameOfUI<T>(objectName);
        T popup = SetUI<T>(objName, UIBase.transform);
        popup.name = $"{objName}";
        _popupOrder.Push(popup);
        SetTimeScale();
        return popup;
    }

    public void ClosePopUp(Popup popup, List<UIEventType> eventTypes)
    { 
        if (_popupOrder.Count == 0) return;
        if (_popupOrder.Peek().name != popup.name) return;
        Popup openPopup = _popupOrder.Pop();
        UI_EventHandler[] eventHandlers =  openPopup.GetComponents<UI_EventHandler>();
        foreach (UI_EventHandler handler in eventHandlers)
        {
            foreach (UIEventType eventType in eventTypes)
            {
                handler.UnbindEvent(eventType);
            }
        }
        _sortByOrderLayer--;
        Destroy(openPopup.gameObject);
        SetTimeScale();
    }

    public T SetSceneUI<T>() where T : UI_Base
    {
        string sceneTypeName = typeof(T).Name;
        return SetUI<T>(sceneTypeName, UIBase.transform);
    }

    private static string NameOfUI<T>(string name)
    {
        return string.IsNullOrEmpty(name) ? typeof(T).Name : name;
    }

    private T SetUI<T>(string uiName, Transform parent = null) where T : Component
    {
        GameObject uiObject = Main.Resource.InstantiatePrefab($"{uiName}.prefab", parent);
        T ui = SceneUtility.GetAddComponent<T>(uiObject);
        ui.transform.SetParent(UIBase.transform);
        return ui;
    }

    public void OrderLayerToCanvas(GameObject uiObject, bool sort = true)
    {
        Canvas canvas = SceneUtility.GetAddComponent<Canvas>(uiObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        SortingOder(canvas, sort);
        CanvasScaler scales = SceneUtility.GetAddComponent<CanvasScaler>(canvas.gameObject);
        scales.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scales.referenceResolution = new Vector2(1920, 1080);
        canvas.referencePixelsPerUnit = 100;
    }

    private void SortingOder(Canvas canvas, bool sort)
    {
        if (sort)
        {
            canvas.sortingOrder = _sortByOrderLayer;
            _sortByOrderLayer++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    private void SetTimeScale()
    {
        if (Main.Scene.CurrentScene != Label.GameScene)
        {
            Time.timeScale = 1;
            return;
        }
        Time.timeScale = _popupOrder.Count > 0 ? 0 : 1;
    }

    public void CloseDeathPanel()
    {
        _onCloseDeathPanel?.Invoke();
        Main.PlayerControl.ToggleCursor(false); // 커서 잠금
    }

    public void ClosePausePanel()
    {
        _onClosePausePanel?.Invoke();
        Main.PlayerControl.ToggleCursor(false);
        SetIsPausePanelOpen(false);
    }

    public void SetIsPausePanelOpen(bool value)
    {
        IsPausePanelOpen = value;
    }
}
