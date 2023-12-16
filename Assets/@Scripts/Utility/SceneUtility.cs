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

        public static void LoadScene(SceneName sceneName)
        {
            Main.SceneClear();
            
            string loadingScene = SceneName.Loading.ToString();
            SceneManager.LoadScene(loadingScene);
            
            string nextScene = sceneName.ToString();
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }
    }
}