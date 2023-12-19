using System.Collections.Generic;
using Scripts.Event.UI;
using Scripts.UI.Popup;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Option_Popup : Popup
{
    #region Field

    private enum Buttons
    {
        OptionCloseBtn,
        WindowCheckIcon,
        FullCheckIcon
    }

    private enum Objects
    {
        BGM_Slider,
        SFX_Slider
    }

    private Button _closeBtn;
    private Slider _bgm_slider;
    private Slider _sfx_slider;
    private Button _windowBtn;
    private Button _fullBtn;
    private int _windowedWidth = 1280;
    private int _windowedHeight = 720;

    #endregion

    private void Start()
    {
        Initialized();
    }

    protected override bool Initialized()
    {
        if (!base.Initialized()) return false;
        SetButton(typeof(Buttons));
        _closeBtn = GetButton((int)Buttons.OptionCloseBtn);
        _closeBtn.gameObject.SetEvent(UIEventType.Click,Close);
        _bgm_slider = GetObject((int)Objects.BGM_Slider).GetComponent<Slider>();
        _sfx_slider = GetObject((int)Objects.SFX_Slider).GetComponent<Slider>();
        _windowBtn = GetButton((int)Buttons.WindowCheckIcon);
        _windowBtn.gameObject.SetEvent(UIEventType.Click, WindowMode);
        _fullBtn = GetButton((int)Buttons.FullCheckIcon);
        _fullBtn.gameObject.SetEvent(UIEventType.Click, FullScreenMode);
        return true;
    }

    private void FullScreenMode(PointerEventData obj)
    {

        Screen.fullScreen = true;
    }

    private void WindowMode(PointerEventData obj)
    {
        Screen.SetResolution(_windowedWidth, _windowedHeight, false);
    }

    private void Close(PointerEventData obj)
    {
        List<UIEventType> eventTypes = new List<UIEventType> { UIEventType.Click };
        Main.UI.ClosePopUp(this, eventTypes);
    }
}
