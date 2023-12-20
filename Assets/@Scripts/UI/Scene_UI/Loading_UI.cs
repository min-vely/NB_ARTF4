using System.Collections;
using Scripts.Event;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI.Scene_UI
{
    public class Loading_UI : UI_Base
    {
        #region Fields
        private enum Images
        {
            LoadingBar,
            TutorialImage
        }

        private enum Texts
        {
            AnyPressText
        }

        private int _totalCount;
        private bool _setTotalCount;
        public const string AnyPressKeyText = "아무키나 누르세요";
        #endregion

        public Image ProgressBar { get; set; }
        public TextMeshProUGUI AnyPressText { get; set; }


        private void Start()
        {
            Initialized();
        }

        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            SetImage(typeof(Images));
            SetText(typeof(Texts));
            ProgressBar = GetImage((int)Images.LoadingBar);
            AnyPressText = GetText((int)Texts.AnyPressText);
            return true;
        }

        public void SetTotalCount(int count)
        {
            if (_setTotalCount) return;
            _setTotalCount = true;
            _totalCount = count;
        }

        public void UpdateProgress(int count, string key)
        {
            if (count == 0 && key == null) return;
            ProgressBar.fillAmount = (float)count / _totalCount;
            AnyPressText.text = key;
        }

        public void SetAnyPress()
        {
            AnyPressText.text = AnyPressKeyText;
        }
    }
}