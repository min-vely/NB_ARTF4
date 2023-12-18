using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Event.UI;
using Scripts.UI;
using Scripts.UI.Popup;
using UnityEngine;
using UnityEngine.EventSystems;

public class Option_Popup : Popup
{
    #region Field

    private enum Buttons
    {
        OptionCloseBtn
    }

    #endregion

    private void Start()
    {
        Initialized();
    }

    protected override bool Initialized()
    {
        if (!base.Initialized()) return false;
        SetButton(typeof(Buttons));
        GetButton((int)Buttons.OptionCloseBtn).gameObject.SetEvent(UIEventType.Click,Close);
        return true;
    }

    private void Close(PointerEventData obj)
    {
        Main.UI.ClosePopUp(this);
    }
}
