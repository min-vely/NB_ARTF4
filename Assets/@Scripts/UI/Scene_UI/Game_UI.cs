using Scripts.Event.UI;
using Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Scene_UI
{
    public class Game_UI : UI_Base
    {
        #region Fields

        private enum Buttons
        {
            ContinueBtn,
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
            GetButton((int)Buttons.ContinueBtn).gameObject.SetEvent(UIEventType.Click, StartGame);
            GetButton((int)Buttons.ExitBtn).gameObject.SetEvent(UIEventType.Click, ShutdownGame);

            return true;
        }

        private void ShutdownGame(PointerEventData obj)
        {
            Application.Quit();
        }

        private void StartGame(PointerEventData obj)
        {
            SceneUtility.LoadScene("GameScene");
        }

        #endregion
    }
}

