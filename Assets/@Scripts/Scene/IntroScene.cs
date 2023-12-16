using System;
using Scripts.UI.Scene_UI;
using UnityEngine;

namespace Scripts.Scene
{
    public class IntroScene : BaseScene
    {
        // TODO  : 임시 팝업 프리팹 (나중에 ResourceManager 생성시 변경)
        [SerializeField] protected GameObject optionPopup;
        [SerializeField] protected GameObject introUI;
        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            // TODO : 인트로 씬 실행시 Context 작성
            Main.Scene.CurrentScene = this;
            Main.UI.SetSceneUI<Intro_UI>();
          
            // TODO : -----------------------
            return true;
        }
    }
}