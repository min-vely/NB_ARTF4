using System.Collections.Generic;
using Scripts.Event.UI;
using Scripts.UI.Popup;
using System;
using Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

public class PausePanel_Popup : Popup
{
    #region Fields

    private enum Buttons
    {
        ContinueBtn,
        OptionBtn
    }
    #endregion

    #region Initialized

    private void Start()
    {
        Initialized();
    }


    protected override bool Initialized()
    {
        if (!base.Initialized()) return false;
        SetButton(typeof(Buttons));
        GetButton((int)Buttons.ContinueBtn).gameObject.SetEvent(UIEventType.Click, ReStartGame);
        GetButton((int)Buttons.OptionBtn).gameObject.SetEvent(UIEventType.Click, OptionOpen);
        return true;
    }

    private void ReStartGame(PointerEventData obj)
    {
        // TODO : Restart인데 씬을 다시 불러와야 하는건 비 효율적 일 것 같아요
        Main.UI.ClosePausePanel();
        DescribeEventTypes();
        // SceneUtility.LoadScene("GameScene"); 
    }

    private void OptionOpen(PointerEventData obj)
    {
        Main.UI.OpenPopup<Option_Popup>();
    }

    private void DescribeEventTypes()
    {
        List<UIEventType> eventTypes = new List<UIEventType> { UIEventType.Click };
        Main.UI.ClosePopUp(this, eventTypes);
    }

    #endregion
}
