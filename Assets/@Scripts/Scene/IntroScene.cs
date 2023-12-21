using System;
using Scripts.UI.Scene_UI;
using UnityEngine;
using Object = System.Object;

namespace Scripts.Scene
{
    public class IntroScene : BaseScene
    {
        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            Main.SetCurrentScene(this, Label.IntroScene);
            IntroSetup();
            AudioClip clip = Main.Resource.Load<AudioClip>("IntroBGM1.clip");
            Main.Sound.StartBGM();
            Main.Sound.PlayBGM(clip.name);
            return true;
        }

        private void IntroSetup()
        {
            Main.UI.SetSceneUI<Intro_UI>();
        }
    }
}