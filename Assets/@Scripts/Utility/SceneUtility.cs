using Scripts.Scene;
using UnityEngine.SceneManagement;

namespace Scripts.Utility
{
    public class SceneUtility
    {
        #region Fields

        public enum SceneName
        {
            Intro,
            Game,
            Loading
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