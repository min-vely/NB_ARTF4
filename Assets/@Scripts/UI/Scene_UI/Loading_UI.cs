using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI.Scene_UI
{
    public class Loading_UI : UI_Base
    {
        private enum Images
        {
            LoadingBar
        }

        private enum Texts
        {
            AnyPressText
        }

        public Image _progressBar;
        public TextMeshProUGUI _anyPressText;

        private void Start()
        {
            Initialized();
        }

        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            SetImage(typeof(Images));
            SetText(typeof(Texts));
            _progressBar = GetImage((int)Images.LoadingBar);
            _anyPressText = GetText((int)Texts.AnyPressText);
            _anyPressText.gameObject.SetActive(false);
            StartCoroutine(LoadAsyncProgress());
            return true;
        }

        private IEnumerator LoadAsyncProgress()
        {
            string nextScene = Main.NextScene;
            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;

            float timer = 0;
            while (!op.isDone)
            {
                if (op.progress < 0.6) _progressBar.fillAmount = op.progress;
                else
                {
                    timer += Time.unscaledDeltaTime;
                    _progressBar.fillAmount = Mathf.Lerp(0.6f, 1f, timer);

                    if (!(_progressBar.fillAmount >= 1f))  continue;
                    _anyPressText.gameObject.SetActive(true);
                    while (true)
                    {
                        if (Input.anyKeyDown)
                        {
                            op.allowSceneActivation = true;
                            yield break;
                        }
                        yield return null;
                    }
                }
                yield return null;
            }
        }
    }
}