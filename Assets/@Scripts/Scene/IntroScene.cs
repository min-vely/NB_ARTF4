using System;
using Scripts.UI.Scene_UI;
using UnityEngine;
using Object = System.Object;

namespace Scripts.Scene
{
    public class IntroScene : BaseScene
    {
        public override Label CurrentScene { get; set; }

        protected override void Start()
        {
            base.Start();
            CurrentScene = Label.IntroScene;
            if (Main.Resource.LoadIntro)
            {
                // TODO : 로드가 되어있다면, 추가적인 초기화 필요
                Initialized();
            }
            else
            {
                string sceneType = CurrentScene.ToString();
                Debug.Log($"CurrentScene : {sceneType}");
                Main.Resource.AllLoadAsync<UnityEngine.Object>($"{sceneType}", (key, count, totalCount) =>
                {
                    Debug.Log($"[{sceneType}] Load asset {key} ({count}/{totalCount})");
                    if (count < totalCount) return;
                    Main.Resource.LoadIntro = true;
                    // TODO : 추가적인 초기화 필요
                    Initialized();
                });
            }
        }

        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            // TODO : 인트로 씬 실행시 Context 작성
            Main.UI.SetSceneUI<Intro_UI>();
            
            // TODO : -----------------------
            return true;
        }
    }
}