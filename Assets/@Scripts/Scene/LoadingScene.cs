using Scripts.UI.Scene_UI;
using UnityEngine;

namespace Scripts.Scene
{
    public class LoadingScene : BaseScene
    {
        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            Main.SetCurrentScene(this, Label.LoadingScene);
            LoadResource();
            return true;
        }

        private void LoadResource()
        {
            if (Main.Resource.LoadLoading)
            {
                Main.UI.SetSceneUI<Loading_UI>();
            }
            else
            {
                string sceneType = CurrentScene.ToString();
                Main.Resource.AllLoadAsync<Object>($"{sceneType}", (key, count, totalCount) =>
                {
                    Debug.Log($"[{sceneType}] Load asset {key} ({count}/{totalCount})");
                    if (count < totalCount) return;
                    Main.Resource.LoadLoading = true;
                    // TODO : 추가적인 초기화 필요
                    Main.UI.SetSceneUI<Loading_UI>();
                });
            }
        }
    }
}