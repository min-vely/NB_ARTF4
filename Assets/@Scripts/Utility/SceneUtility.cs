using System;
using Scripts.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Utility
{
    public static class SceneUtility
    {
        public static void LoadScene(string nextScene)
        {
            Main.SceneClear();
            SceneManager.LoadScene("LoadingScene");
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }

        public static T GetAddComponent<T>(GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() ?? obj.AddComponent<T>();  
        }
    }
}