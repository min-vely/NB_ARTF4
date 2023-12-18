using System;
using Scripts.Event.UI;
using Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Scene_UI
{
    public class Intro_UI : UI_Base
    {
        #region Fields

        private enum Buttons
        {
            StartBtn,
            ContinueBtn,
            OptionBtn,
            ExitBtn
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
            GetButton((int)Buttons.StartBtn).gameObject.SetEvent(UIEventType.Click, StartGame);
            GetButton((int)Buttons.ContinueBtn).gameObject.SetEvent(UIEventType.Click,StartGame);
            GetButton((int)Buttons.OptionBtn).gameObject.SetEvent(UIEventType.Click,OptionOpen);
            GetButton((int)Buttons.ExitBtn).gameObject.SetEvent(UIEventType.Click, ShutdownGame);
            return true;
        }
        
        private void ShutdownGame(PointerEventData obj)
        {
            Application.Quit();
        }

        private void OptionOpen(PointerEventData obj)
        {
            Main.UI.OpenOptionPopup<Option_Popup>();
        }

        private void StartGame(PointerEventData obj)
        {
            SceneUtility.LoadScene("GameScene");
        }

        #endregion
    }
}