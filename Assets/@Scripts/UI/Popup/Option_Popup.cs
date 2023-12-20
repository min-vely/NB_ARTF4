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

    private enum GameObjects
    {
        BGMSlider,
        SFXSlider
    }

    private enum Images
    {
        WindowCheckIcon,
        FullCheckIcon
    }

    private Button _closeBtn;
    private Slider _bgm_slider;
    private Slider _sfx_slider;
    private Button _windowBtn;
    private Button _fullBtn;
    private Sprite _unCheckSprite;
    private Sprite _checkSprite;
    private Image _windowCheckIcon;
    private Image _fullCheckIcon;
    private int _windowedWidth = 1280;
    private int _windowedHeight = 720;
    private bool _isFullScreenMode = false;

    #endregion

    private void Start()
    {
        Initialized();
    }

    protected override bool Initialized()
    {
        if (!base.Initialized()) return false;
        SetButton(typeof(Buttons));
        SetObject(typeof(GameObjects));
        SetImage(typeof(Images));
        _closeBtn = GetButton((int)Buttons.OptionCloseBtn);
        _closeBtn.gameObject.SetEvent(UIEventType.Click,Close);
        _bgm_slider = GetObject((int)GameObjects.BGMSlider).GetComponent<Slider>(); 
        _sfx_slider = GetObject((int)GameObjects.SFXSlider).GetComponent<Slider>();;
        _windowCheckIcon = GetImage((int)Images.WindowCheckIcon);
        _fullCheckIcon = GetImage((int)Images.FullCheckIcon);
        _windowBtn = GetButton((int)Buttons.WindowCheckIcon);
        _windowBtn.gameObject.SetEvent(UIEventType.Click, WindowMode);
        _fullBtn = GetButton((int)Buttons.FullCheckIcon);
        _fullBtn.gameObject.SetEvent(UIEventType.Click, FullScreenMode);
        _checkSprite = Main.Resource.Load<Sprite>("buttons.atlas[35]");
        _unCheckSprite = Main.Resource.Load<Sprite>("buttons.atlas[23]");
        return true;
    }

    private void ToggleIcon()
    {
        if (_isFullScreenMode)
        {
            _fullCheckIcon.sprite = _checkSprite;
            _windowCheckIcon.sprite = _unCheckSprite;
        }
        else
        {
            _fullCheckIcon.sprite = _unCheckSprite;
            _windowCheckIcon.sprite = _checkSprite;
        }
    }

    private void FullScreenMode(PointerEventData obj)
    {
        Screen.fullScreen = true;
        _isFullScreenMode = true;
        ToggleIcon();
    }

    private void WindowMode(PointerEventData obj)
    {
        Screen.SetResolution(_windowedWidth, _windowedHeight, false);
        _isFullScreenMode = false;
        ToggleIcon();
    }

    private void Close(PointerEventData obj)
    {
        List<UIEventType> eventTypes = new List<UIEventType> { UIEventType.Click };
        Main.UI.ClosePopUp(this, eventTypes);
    }

    // TODO : SoundManager 필요
    // TODO : 해당 SoundManager에서 Volume 값을 Slider의 Value와 연결 필요
}
