using System.Collections;
using Scripts.UI.Scene_UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Scripts.Scene
{
    public class LoadingScene : BaseScene
    {

        private Loading_UI _loadingUI;
        private int _count;
        private int _totalCount;
        private string _key;
        private bool _resourcesLoaded = false; 

        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            Main.NextScene ??= "IntroScene";
            Main.Sound.PlayBGM("BGM");
            LoadResourcesAndScene();
            return true;
        }

        private void LoadResourcesAndScene()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            _loadingUI = Main.UI.SetSceneUI<Loading_UI>();

            Main.Resource.AllLoadAsync<Object>($"{Main.NextScene}", (key, count, totalCount) =>
            {
                _key = key;
                _count = count;
                _totalCount = totalCount;
                _loadingUI.SetTotalCount(_totalCount);
                _loadingUI.UpdateProgress(_count, _key);
                Debug.Log($"[{Main.NextScene}] Load asset {_key} ({_count}/{_totalCount})");
                if (_count != _totalCount) return;
                _resourcesLoaded = true; // 리소스 로딩 완료
                LoadNextSceneAsync(); // 리소스 로딩 완료 후 씬 로딩 시작
            });
        }

        private void LoadNextSceneAsync()
        {
            AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(Main.NextScene);
            sceneLoadOperation.allowSceneActivation = false;
            StartCoroutine(UpdateLoadingProgress(sceneLoadOperation));
        }

        private IEnumerator UpdateLoadingProgress(AsyncOperation sceneLoadOperation)
        {
            while (!_resourcesLoaded || sceneLoadOperation.progress < 0.9f) yield return null;
            _loadingUI.SetAnyPress();
            while (!Input.anyKeyDown) yield return null;
            sceneLoadOperation.allowSceneActivation = true;
        }
    }
}