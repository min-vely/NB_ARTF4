using Scripts.Event;
using UnityEngine;
namespace Scripts.Scene
{
    public class GameScene : BaseScene
    {
        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            Main.SetCurrentScene(this, Label.GameScene);
            GameSetup();
            AudioClip clip = Main.Resource.Load<AudioClip>("GameBGM1.clip");
            Main.Sound.StartBGM();
            Main.Sound.PlayBGM(clip.name);
            return true;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !Main.UI.IsPausePanelOpen) // ESC 누르면 Pause
            {
                OpenPausePopUp();
            }
        }
        private void GameSetup()
        {
            Main.UI.SetSceneUI<Game_UI>();
            SubscribeEvent();
            Main.Obstacle.Initialized();
            Main.Game.SpawnPlayer();
            Main.Item.Initialized();
        }
        private void SubscribeEvent()
        {
            KillZone.OnDeath -= OpenDeathPopUp;
            KillZone.OnDeath += OpenDeathPopUp;
        }
        private void OpenDeathPopUp()
        {
            Main.UI.OpenPopup<DiePanel_Popup>();
            Main.PlayerControl.ToggleCursor(true); // 커서 잠금해제
        }
        private void OpenPausePopUp()
        {
            Main.UI.OpenPopup<PausePanel_Popup>();
            Main.PlayerControl.ToggleCursor(true);
            Main.UI.SetIsPausePanelOpen(true);
        }
    }
}