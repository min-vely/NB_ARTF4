using System;
using Scripts.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Utility
{
    public class SceneUtility
    {
        public static void LoadScene(string nextScene)
        {
            Main.SceneClear();
            Main.NextScene = nextScene;
            SceneManager.LoadScene("LoadingScene");
        }

        public static T GetAddComponent<T>(GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() ?? obj.AddComponent<T>();  
        }
    }
}