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
            return true;
        }

        private void GameSetup()
        {
            Main.UI.SetSceneUI<Game_UI>(); 
            SubscribeEvent();
            Main.Obstacle.Initialized();
            Main.Game.SpawnPlayer();
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
    }
}