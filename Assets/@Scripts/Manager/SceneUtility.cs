using UnityEngine.SceneManagement;

namespace Scripts.Scene
{
    public class SceneUtility
    {
        #region Fields

        public enum SceneName
        {
            IntroScene,
            GameScene,
            LoadingScene
        }

        #endregion

        #region Properties

        public BaseScene CurrentScene { get; set; }

        #endregion

        public void LoadScene(SceneName sceneName)
        {
            Main.SceneClear();
            string scene = sceneName.ToString();
            SceneManager.LoadScene(scene);
        }
    }
}